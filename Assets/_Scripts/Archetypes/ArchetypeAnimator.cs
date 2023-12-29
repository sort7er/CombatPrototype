using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ArchetypeAnimator : MonoBehaviour
{
    public event Action OnLethal;
    public event Action OnLethal2;
    public event Action OnNotLethal;

    [Header("Idle")]
    [SerializeField] private AnimationInput idleInput;
    [Header("Light attacks")]
    [SerializeField] private AttackInput[] lightAttackInputs;
    [Header("Heavy attacks")]
    [SerializeField] private AttackInput[] heavyAttackInputs;
    [Header("Unique attack")]
    [SerializeField] private AttackInput uniqueAttackInput;

    private Anim idleAnim;
    private Attack[] lightAttacks;
    private Attack[] heavyAttacks;
    private Attack uniqueAttack;

    
    private Animator archetypeAnim;

    private enum AttackType
    {
        light,
        heavy,
        unique
    }

    private List<AttackType> attackQueue = new();

    private int queueCapasity = 2;
    public bool isAttacking { get; private set; }

    private int currentCombo;

    private void Awake()
    {
        archetypeAnim = GetComponent<Animator>();
        idleAnim = new Anim(idleInput.animationClip);
        lightAttacks = new Attack[lightAttackInputs.Length];
        heavyAttacks = new Attack[heavyAttackInputs.Length];

        SetUpAttacks(lightAttacks, lightAttackInputs);
        SetUpAttacks(heavyAttacks, heavyAttackInputs);
        uniqueAttack = new Attack(uniqueAttackInput.animationClip, uniqueAttackInput.damage, uniqueAttackInput.queuePoint);

    }
    
    public void SetUpAttacks(Attack[] attacksToSetUp, AttackInput[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            attacksToSetUp[i] = new Attack(inputs[i].animationClip, inputs[i].damage, inputs[i].queuePoint);
        }
    }


    public void Fire()
    {
        CheckAttack(AttackType.light);
    }
    public void HeavyFire()
    {
        CheckAttack(AttackType.heavy);
    }
    public void UniqueFire()
    {
        CheckAttack(AttackType.unique);
    }

    //Temporary fuction until weapon switching is proparly made
    public void Abort()
    {
        attackQueue.Clear();
        isAttacking = false;
    }

    //Checking if there is a queue
    private void CheckAttack(AttackType attackType)
    {
        if (isAttacking)
        {
            //If is currently attacking, try and add this attack to queue. Cannot queue unique attack
            if(attackType != AttackType.unique)
            {
                TryAddToQueue(attackType);
            }

        }
        else
        {
            //If not currently attacking
            Attack(attackType);
        }
    }

    private void TryAddToQueue(AttackType attackType)
    {
        // if there is capasity in the queue, add the new attack
        if (attackQueue.Count < queueCapasity)
        {
            attackQueue.Add(attackType);
        }
    }

    private void Attack(AttackType attackType, float crossfade = 0)
    {
        isAttacking = true;
        Attack currenAttack;
        if (attackType == AttackType.light)
        {
            currenAttack = lightAttacks[currentCombo];
        }
        else if(attackType == AttackType.heavy)
        {
            currenAttack = heavyAttacks[currentCombo];
        }
        else
        {
            currenAttack = uniqueAttack;
        }

        archetypeAnim.CrossFade(currenAttack.state, crossfade);

        //If not the third attack, check for more attacks in the queue after queuepoint in the attack
        if(currentCombo < 2)
        {
            float remapedValue = Remap(currenAttack.queuePoint, 0, 60, 0, 1);
            Invoke(nameof(CheckQueue), remapedValue);
        }
        Invoke(nameof(AttackDone), currenAttack.duration);
        UpdateCurrentAttack();

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
            CancelInvoke(nameof(AttackDone));
            Attack(attackQueue[0], 0.25f);
            attackQueue.RemoveAt(0);
        }
    }
    //Used at end of attacks
    public void AttackDone()
    {
        attackQueue.Clear();
        isAttacking = false;
        currentCombo = 0;
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
