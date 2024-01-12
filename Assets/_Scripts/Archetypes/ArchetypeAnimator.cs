using System;
using System.Collections.Generic;
using UnityEngine;
using ArchetypeStates;

[RequireComponent(typeof(Animator))]
public class ArchetypeAnimator : MonoBehaviour
{
    public event Action OnLethal;
    public event Action OnLethal2;
    public event Action OnNotLethal;
    public event Action OnActionDone;

    [Header("Idle")]
    [SerializeField] private AnimationInput idleInput;
    [Header("Block")]
    [SerializeField] private AttackInput blockInput;
    [Header("Parry")]
    [SerializeField] private AttackInput parryInput;
    [Header("Light attacks")]
    [SerializeField] private AttackInput[] lightAttackInputs;
    [Header("Heavy attacks")]
    [SerializeField] private AttackInput[] heavyAttackInputs;
    [Header("Unique attack")]
    [SerializeField] private AttackInput uniqueAttackInput;


    public Attack currentAction { get; private set; }

    public Anim idleAnim;
    public Attack block;
    public Attack parry;
    public Attack[] light;
    public Attack[] heavy;
    public Attack unique;


    
    private Animator archetypeAnim;

    private enum ActionType
    {
        light,
        heavy,
        unique,
        block,
        parry
    }

    private List<ActionType> attackQueue = new();

    private int queueCapasity = 2;
    public bool isAttacking { get; private set; }

    public bool isDefending { get; private set; }

    private int currentCombo;

    public ArchetypeState currentState;
    public IdleState idleState = new IdleState();
    public AttackState attackState = new AttackState();
    public UniqueState uniqueState = new UniqueState();
    public BlockingState blockState = new BlockingState();
    public ParryState parryState = new ParryState();





    private void Awake()
    {
        archetypeAnim = GetComponent<Animator>();

        SetUpAnimations();
        SwitchState(idleState);
    }

    private void SetUpAnimations()
    {
        idleAnim = new Anim(idleInput.animationClip);
        block = new Attack(blockInput.animationClip, blockInput.damage, blockInput.queuePoint, blockInput.damageType);
        parry = new Attack(parryInput.animationClip, parryInput.damage, parryInput.queuePoint, parryInput.damageType);
        light = new Attack[lightAttackInputs.Length];
        heavy = new Attack[heavyAttackInputs.Length];

        SetUpAttacks(light, lightAttackInputs);
        SetUpAttacks(heavy, heavyAttackInputs);
        unique = new Attack(uniqueAttackInput.animationClip, uniqueAttackInput.damage, uniqueAttackInput.queuePoint, uniqueAttackInput.damageType);
    }
    
    public void SetUpAttacks(Attack[] attacksToSetUp, AttackInput[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            attacksToSetUp[i] = new Attack(inputs[i].animationClip, inputs[i].damage, inputs[i].queuePoint, inputs[i].damageType);
        }
    }
    public void SwitchState(ArchetypeState state)
    {
        currentState = state;
        state.EnterState(this, archetypeAnim);
    }


    public void Fire()
    {
        currentState.Fire(this);
        CheckAction(ActionType.light);
    }
    public void HeavyFire()
    {
        CheckAction(ActionType.heavy);
    }
    public void UniqueFire()
    {
        CheckAction(ActionType.unique);
    }
    public void Block()
    {
        CheckAction(ActionType.block);
    }
    public void Parry()
    {
        CheckAction(ActionType.parry);
    }

    //Temporary fuction until weapon switching is proparly made
    public void Abort()
    {
        attackQueue.Clear();
        isAttacking = false;
    }

    //Checking if there is a queue
    private void CheckAction(ActionType actionType)
    {
        if(!isAttacking && !isDefending)
        {
            //If not currently using the sword

            if(actionType == ActionType.block || actionType == ActionType.parry)
            {
                Defence(actionType);
            }
            else
            {
                Attack(actionType);
            }
        }
        else if (isDefending && actionType == ActionType.parry)
        {
            Defence(actionType);
        }
        else if(isAttacking)
        {
            //If is currently attacking, try and add this attack to queue. Cannot queue unique attack
            if (actionType == ActionType.light || actionType == ActionType.heavy)
            {
                TryAddToQueue(actionType);
            }

        }
        
    }

    private void TryAddToQueue(ActionType attackType)
    {
        // if there is capasity in the queue, add the new attack
        if (attackQueue.Count < queueCapasity)
        {
            attackQueue.Add(attackType);
        }
    }

    private void Attack(ActionType attackType, float crossfade = 0)
    {
        isAttacking = true;
        if (attackType == ActionType.light)
        {
            currentAction = light[currentCombo];
        }
        else if(attackType == ActionType.heavy)
        {
            currentAction = heavy[currentCombo];
        }
        else
        {
            currentAction = unique;
        }


        archetypeAnim.CrossFade(currentAction.state, crossfade);

        //If not the third attack, check for more attacks in the queue after queuepoint in the attack
        if(currentCombo < 2)
        {
            float remapedValue = Remap(currentAction.queuePoint, 0, 60, 0, 1);
            Invoke(nameof(CheckQueue), remapedValue);
        }
        Invoke(nameof(ActionDone), currentAction.duration);
        Debug.Log("Attack");
        UpdateCurrentAttack();

    }

    private void Defence(ActionType defenceType, float crossfade = 0)
    {
        isDefending = true;
        if (defenceType == ActionType.block)
        {
            currentAction = block;
        }
        else
        {
            currentAction = parry;
            Invoke(nameof(ActionDone), currentAction.duration);
            Debug.Log("Defence");
        }

        archetypeAnim.CrossFade(currentAction.state, crossfade);
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return from2 + (value - from1) * (to2 - from2) / (to1 - from1);
    }
    private void UpdateCurrentAttack()
    {
        if(currentCombo < 2)
        {
            currentCombo++;
        }
        else
        {
            currentCombo = 0;
        }
    }

    //See if it should chain into a new attack
    public void CheckQueue()
    {
        // Attack if queue is not empty
        if (attackQueue.Count > 0)
        {
            Debug.Log("CheckQueue");
            CancelInvoke(nameof(ActionDone));
            Attack(attackQueue[0], 0.25f);
            attackQueue.RemoveAt(0);
            OnActionDone?.Invoke();
        }
    }
    //Used at end of attacks
    public void ActionDone()
    {
        Debug.Log(3);
        OnActionDone?.Invoke();
        attackQueue.Clear();
        isAttacking = false;
        isDefending = false;
        currentCombo = 0;
        currentAction = null;
        archetypeAnim.CrossFade(idleAnim.state, 0.25f);
    }

    //This is called from the animation, to see when an attack is lethal
    public void Lethal()
    {
        OnLethal?.Invoke();
    }
    public void Lethal2()
    {
        OnLethal2?.Invoke();
    }
    public void Both()
    {
        Lethal();
        Lethal2();
    }

    public void NotLethal()
    {
        OnNotLethal?.Invoke();
    }
}
