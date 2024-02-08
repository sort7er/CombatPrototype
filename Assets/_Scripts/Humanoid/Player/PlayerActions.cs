using System;
using System.Collections;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [Header("Values")]
    public float parryWindow = 0.17f;

    [Header("References")]
    public Animator armAnimator;
    public Player player;
    public Weapon startWeapon;
    public Transform[] weaponPos;
    public Weapon currentWeapon { get; private set; }

    public ActionState currentState;
    public Anim currentAnimation;

    public IdleState idleState = new IdleState();
    public AttackState attackState = new AttackState();
    public UniqueState uniqueState = new UniqueState();
    public BlockState blockState = new BlockState();
    public ParryState parryState = new ParryState();

    private InputReader inputReader;

    private void Awake()
    {
        SetUpInput();
        SetNewWeapon(startWeapon);
        SwitchState(idleState);
    }

    private void OnDestroy()
    {
        EndInput();
    }

    #region Signal states
    private void Update()
    {
        currentState.Update();
    }
    public void Attack()
    {
        currentState.Attack();
    }
    public void Unique()
    {
        currentState.Unique();
    }
    public void Block()
    {
        currentState.Block();
    }
    public void Parry()
    {
        currentState.Parry();
    }
    public void CheckChain()
    {
        currentState.CheckChain();
    }

    #endregion


    //Called from state machine
    public void SwitchState(ActionState state)
    {
        currentState = state;
        currentState.Enter(this);
    }
    public void SetAnimation(Anim newAnim, float transition = 0.25f)
    {
        currentAnimation = newAnim;
        armAnimator.CrossFade(currentAnimation.state, transition);
    }

    #region Invoking
    public void InvokeMethod(Action function, float waitTime)
    {
        StartCoroutine(DoFunction(function, waitTime));
    }
    private IEnumerator DoFunction(Action function, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        function.Invoke();
    }
    public void StopMethod()
    {
        StopAllCoroutines();
    }
    #endregion

    //Called from other classes
    public void SetNewWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        currentWeapon.SetParent(weaponPos);
    }

    #region Redirects
    private void SetUpInput()
    {
        inputReader = player.inputReader;

        inputReader.OnAttack += Attack;
        inputReader.OnUnique += Unique;
        inputReader.OnBlock += Block;
        inputReader.OnParry += Parry;
    }
    private void EndInput()
    {
        inputReader.OnAttack -= Attack;
        inputReader.OnUnique -= Unique;
        inputReader.OnBlock -= Block;
        inputReader.OnParry -= Parry;
    }

    #endregion
}
