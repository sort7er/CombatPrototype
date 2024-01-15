using System;
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
    [Header("Light attacks")]
    [SerializeField] private AttackInput[] lightAttackInputs;
    [Header("Heavy attacks")]
    [SerializeField] private AttackInput[] heavyAttackInputs;
    [Header("Unique attack")]
    [SerializeField] private AttackInput uniqueAttackInput;
    [Header("Block")]
    [SerializeField] private AttackInput blockInput;
    [Header("Parry")]
    [SerializeField] private AttackInput []parryInputs;
    [Header("Staggered")]
    [SerializeField] private AnimationInput staggeredInput;


    public Animator archetypeAnim { get; private set; }
    public Attack currentAttack { get; private set; }
    public Attack entryAttack { get; private set; }
    public bool isAttacking { get; private set; }
    public bool isBlocking { get; private set; }
    public bool isParrying { get; private set; }

    public Anim idle;
    public Attack[] light;
    public Attack[] heavy;
    public Attack unique;
    public Attack block;
    public Attack[] parry;
    public Anim staggered;


    public ArchetypeState currentState;
    public IdleState idleState = new IdleState();
    public AttackState attackState = new AttackState();
    public BlockingState blockState = new BlockingState();
    public ParryState parryState = new ParryState();
    public StaggeredState staggeredState = new StaggeredState();




    private void Awake()
    {
        archetypeAnim = GetComponent<Animator>();

        SetUpAnimations();
        SwitchState(idleState);
    }

    #region Animation set up
    private void SetUpAnimations()
    {
        idle = new Anim(idleInput.animationClip);
        light = new Attack[lightAttackInputs.Length];
        heavy = new Attack[heavyAttackInputs.Length];
        block = new Attack(blockInput.animationClip, blockInput.damage, blockInput.queuePoint, blockInput.damageType);
        parry = new Attack[parryInputs.Length];
        staggered = new Anim(staggeredInput.animationClip);

        SetUpAttacks(light, lightAttackInputs);
        SetUpAttacks(heavy, heavyAttackInputs);
        SetUpAttacks(parry, parryInputs);
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
    public void Staggered()
    {
        currentState.Staggered(this);
    }
    #endregion

    //Temporary fuction until weapon switching is proparly made
    public void Abort()
    {
        SwitchState(idleState);
    }

    //Called from other parts of the FSM
    public void SwitchState(ArchetypeState state)
    {
        currentState = state;
        state.EnterState(this);
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
    public void IsBlocking(bool state)
    {
        isBlocking = state;
    }
    public void IsParrying(bool state)
    {
        isParrying = state;
    }
    public void CrossFade(Anim anim, float crossfade = 0)
    {
        archetypeAnim.CrossFade(anim.state, crossfade);
    }
    public void SetAnimation(Anim anim)
    {
        archetypeAnim.Play(anim.state);
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
    //Tool to mach invoke time with keyframes
    public float Remap(float value, float from1 = 0, float to1 = 60, float from2  = 0, float to2 = 1)
    {
        return from2 + (value - from1) * (to2 - from2) / (to1 - from1);
    }

    //This is called from the animations, to see when an attack is lethal

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
