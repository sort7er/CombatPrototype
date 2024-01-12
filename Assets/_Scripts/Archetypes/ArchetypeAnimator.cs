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


    public Attack currentAttack { get; private set; }

    public Anim idleAnim;
    public Attack block;
    public Attack parry;
    public Attack[] light;
    public Attack[] heavy;
    public Attack unique;


    
    private Animator archetypeAnim;

    private List<Attack> attackQueue = new();

    private int queueCapasity = 2;
    public bool isAttacking { get; private set; }

    public bool isDefending { get; private set; }

    private int currentCombo;

    public ArchetypeState currentState;
    public IdleState idleState = new IdleState();
    public FireState fireState = new FireState();
    public HeavyFireState heavyFireState = new HeavyFireState();
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



    public void Fire()
    {
        currentState.Fire(this);
    }
    public void HeavyFire()
    {
        currentState.HeavyFire(this);
    }
    public void UniqueFire()
    {
        currentState.UniqueFire(this);
    }
    public void Block()
    {
        currentState.Block(this);
    }
    public void Parry()
    {
        currentState.Parry(this);
    }

    //Temporary fuction until weapon switching is proparly made
    public void Abort()
    {
        SwitchState(idleState);
    }


    //private void Defence(ActionType defenceType, float crossfade = 0)
    //{
    //    isDefending = true;
    //    if (defenceType == ActionType.block)
    //    {
    //        currentAttack = block;
    //    }
    //    else
    //    {
    //        currentAttack = parry;
    //        Invoke(nameof(ActionDone), currentAttack.duration);
    //        Debug.Log("Defence");
    //    }

    //    archetypeAnim.CrossFade(currentAttack.state, crossfade);
    //}

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
        // Attack if queue is not empty, there is an if statement here

            Debug.Log("CheckQueue");
            CancelInvoke(nameof(ActionDone));

    }
    //Used at end of attacks
    public void ActionDone()
    {
        isAttacking = false;
        OnActionDone?.Invoke();
        currentCombo = 0;
        currentAttack = null;
        archetypeAnim.CrossFade(idleAnim.state, 0.25f);
    }
    //Called from other parts of the FSM
    public void SwitchState(ArchetypeState state)
    {
        currentState = state;
        state.EnterState(this, archetypeAnim);
    }
    public void SetCurrentAction(Attack currentAttack)
    {
        this.currentAttack = currentAttack;
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
