using System;
using System.Collections.Generic;
using UnityEngine;
using ArchetypeStates;
using System.Collections;

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


    public Animator archetypeAnim { get; private set; }
    public Attack currentAttack { get; private set; }
    public Attack entryAttack { get; private set; }
    public bool isAttacking { get; private set; }

    public Anim idleAnim;
    public Attack block;
    public Attack parry;
    public Attack[] light;
    public Attack[] heavy;
    public Attack unique;


    public ArchetypeState currentState;
    public IdleState idleState = new IdleState();
    public AttackState attackState = new AttackState();
    public BlockingState blockState = new BlockingState();
    public ParryState parryState = new ParryState();




    private void Awake()
    {
        archetypeAnim = GetComponent<Animator>();

        SetUpAnimations();
        SwitchState(idleState);
    }
    private void Update()
    {
        //Debug.Log(attackState.attackQueue.Count);
    }

    #region Animation set up
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
    #endregion

    #region Inputs
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
    #endregion

    //Temporary fuction until weapon switching is proparly made
    public void Abort()
    {
        SwitchState(idleState);
    }

    //Used at end of attacks

    //Called from other parts of the FSM
    public void SwitchState(ArchetypeState state)
    {
        currentState = state;
        state.EnterState(this, archetypeAnim);
    }
    public void SetCurrentAttack(Attack newAttack)
    {
        currentAttack = newAttack;
    }
    public void SetEntryAttack(Attack newAttack)
    {
        entryAttack = newAttack;
    }
    public void IsAttacking(bool state)
    {
        isAttacking = state;
    }
    public void SetAnimation(Anim anim, float crossfade = 0)
    {
        archetypeAnim.CrossFade(anim.state, crossfade);
    }
    public void InvokeAttackDoneEvent()
    {
        OnActionDone?.Invoke();
    }
    public void InvokeFunction(Action function, float waitTime)
    {
        StartCoroutine(DoFunction(function, waitTime));
    }

    private IEnumerator DoFunction(Action function, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        function.Invoke();
    }
    public void StopFunction()
    {
        StopAllCoroutines();
    }

    //This is called from the animation, to see when an attack is lethal

    #region When weapon is leathal
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
    #endregion
}
