using System;
using UnityEngine;
using ArchetypeStates;
using System.Collections;
using Attacks;
using UnityEngine.Windows;

[RequireComponent(typeof(Animator))]
public class ArchetypeAnimator : MonoBehaviour
{
    public event Action<Attack> OnAttack;
    public event Action OnSwingDone;
    public event Action OnAttackDone;

    [Header("Idle")]
    [SerializeField] private AnimationInput idleInput;
    [Header("Light attacks")]
    [SerializeField] private AttackInput[] lightInputs;
    [Header("Heavy attacks")]
    [SerializeField] private AttackInput[] heavyInputs;
    [Header("Unique attack")]
    [SerializeField] private AttackInput uniqueInput;
    [Header("Block")]
    [SerializeField] private AttackInput blockInput;
    [Header("Parry")]
    [SerializeField] private AttackInput []parryInputs;
    [Header("Staggered")]
    [SerializeField] private AnimationInput staggeredInput;


    public Archetype archetype { get; private set; }
    public Animator archetypeAnim { get; private set; }
    public Attack currentAttack { get; private set; }
    public Attack entryAttack { get; private set; }
    public bool isAttacking { get; private set; }
    public bool isBlocking { get; private set; }
    public bool canBeParried { get; private set; }

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
        archetype = GetComponent<Archetype>();
        archetypeAnim = GetComponent<Animator>();
        SetUpAnimations();
        SwitchState(idleState);

        archetype.hitBox.OnCanBeParried += CanBeParried;
    }
    private void OnDestroy()
    {
        archetype.hitBox.OnCanBeParried -= CanBeParried;
    }

    #region Animation set up
    private void SetUpAnimations()
    {
        idle = new Anim(idleInput.animationClip);
        staggered = new Anim(staggeredInput.animationClip);

        SetUpAttacks(ref light, lightInputs);
        SetUpAttacks(ref heavy, heavyInputs);
        SetUpAttacks(ref parry, parryInputs);
        SetUpAttack(ref unique, uniqueInput);
        SetUpAttack(ref block, blockInput);
    }
    private void Update()
    {
        Debug.Log(archetype.owner.name + ": " + canBeParried);
    }
    public void SetUpAttack(ref Attack attacksToSetUp, AttackInput inputs)
    {
        attacksToSetUp = new Attack(inputs.animationClip, inputs.damage, inputs.postureDamage, inputs.queuePoint, inputs.damageType, inputs.activeWeapon, inputs.attributeAffected);
    }

    public void SetUpAttacks(ref Attack[] attacksToSetUp, AttackInput[] inputs)
    {
        attacksToSetUp = new Attack[inputs.Length];

        for (int i = 0; i < inputs.Length; i++)
        {
            SetUpAttack(ref attacksToSetUp[i], inputs[i]);
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
    public void SetEntryAttack(Attack newAttack)
    {
        entryAttack = newAttack;
    }
    public void IsAttacking(Attack newAttack, float crossfade = 0)
    {
        isAttacking = true;
        currentAttack = newAttack;
        CrossFade(currentAttack, crossfade);
        OnAttack?.Invoke(currentAttack);
    }
    public void SwingDone()
    {
        OnSwingDone?.Invoke();
    }
    public void AttackingDone()
    {
        OnAttackDone?.Invoke();
        SwingDone();
        NotAttacking();
    }
    public void NotAttacking()
    {
        isAttacking = false;
    }
    public void IsBlocking(bool state)
    {
        isBlocking = state;
    }
    public void CanBeParried(bool state)
    {
        canBeParried = state;
    }
    public void CrossFade(Anim anim, float crossfade = 0)
    {
        archetypeAnim.CrossFade(anim.state, crossfade);
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
}
