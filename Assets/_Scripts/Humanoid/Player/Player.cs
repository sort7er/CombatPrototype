using UnityEngine;
using PlayerSM;
using System;
using System.Collections;
using Stats;
using static UnityEngine.UI.GridLayoutGroup;

public class Player : Humanoid
{

    [Header("Values")]
    public float blockWaitTime = 0;

    [Header("References")]
    public Animator armAnimator;
    public Unique unique;
    public WeaponSwitcher weaponSwitcher;
    public InputReader inputReader;
    public CameraController cameraController;
    public HitBox hitBox;
    public Health Health;
    public TargetAssistance targetAssistance;
    public ParryCheck parryCheck;

    public Transform[] weaponPos;
    public Weapon currentWeapon { get; private set; }
    public PlayerState currentState { get; private set; }
    public Anim currentAnimation { get; private set; }
    public bool isMoving { get; private set; }
    public bool isFalling { get; private set; }

    public IdleState idleState = new IdleState();
    public JumpState jumpState = new JumpState();
    public FallState fallState = new FallState();
    public AttackState attackState = new AttackState();
    public UniqueState uniqueState = new UniqueState();
    public BlockState blockState = new BlockState();
    public ParryState parryState = new ParryState();
    public PerfectParryState perfectParryState = new PerfectParryState();
    public ParryAttackState parryAttackState = new ParryAttackState();



    public int currentParry { get; private set; }
    public int currentPerfectParry { get; private set; }
    public float uniqueCoolDown { get; private set; } = 8f;
    
    [HideInInspector] public bool canUseUnique = true;



    private Vector2 input;
    private Vector2 movement;

    #region Set up
    protected override void Awake()
    {
        base.Awake();
        EnableMovement();
        SetUpInput();
        weaponSwitcher.OnNewWeapon += SetNewWeapon;

    }

    private void OnDestroy()
    {
        EndInput();
    }

    protected override void Update()
    {
        base.Update();
        //Debug.Log("Current state: " + currentState);
        currentState.Update();

        movement = Vector2.Lerp(movement, input, Time.deltaTime * 5);
        SetMovement(movement);
    }
    #endregion

    #region Signal states

    public override void Stunned()
    {

    }
    public override void Staggered()
    {

    }
    public void Moving()
    {
        isMoving = true;
        currentState.Moving();
    }
    public void StoppedMoving()
    {
        isMoving = false;
        currentState.StoppedMoving();
    }
    public void Attack()
    {
        currentState.Attack();
    }
    public void Unique()
    {
        if (canUseUnique)
        {
            currentState.Unique();
        }
    }
    public void Block()
    {
        currentState.Block();
    }
    public void BlockRelease()
    {
        currentState.BlockRelease();
    }
    public void ActionStart()
    {
        currentWeapon.Effect();
    }
    public void OverlapCollider()
    {
        currentState.OverlapCollider();
    }
    public void ActionDone()
    {
        currentState.ActionDone();
        currentWeapon.AttackDone();
    }

    //From humanoid
    public override void Parry()
    {
        currentState.Parry();
    }
    public override void PerfectParry()
    {
        currentState.PerfectParry();
    }
    public override void Jumping()
    {
        isFalling = true;
        currentState.Jump();
    }
    public override void Fall()
    {
        isFalling = true;
        currentState.Fall();
    }
    public override void Landing()
    {
        isFalling = false;
        currentState.Landing();
    }
    public override void TakeDamage(Weapon attackingWeapon, Vector3 hitPoint)
    {
        currentState.TakeDamage(attackingWeapon, hitPoint);
        
        ParryType parryType = parryCheck.CheckForParry(this, attackingWeapon.owner);

        if (parryType == ParryType.None)
        {
            health.TakeDamage(attackingWeapon, hitPoint);
        }
        else
        {
            //parryCheck.IsDefending(parryData);
        }
    }

    #endregion

    #region Called from state machine
    //public void StartUniqueCooldown()
    //{
    //    unique.Loading();
    //    Invoke(nameof(UniqueCooldownDone), uniqueCoolDown);
    //}
    //private void UniqueCooldownDone()
    //{
    //    unique.Active();
    //    canUseUnique = true;
    //}

    public void SwitchState(PlayerState state)
    {
        currentState = state;
        currentState.Enter(this);
    }

    public void SetAnimation(Anim newAnim, float transition = 0.25f)
    {
        if (newAnim is Attack attack)
        {
            currentWeapon.Attack(attack);
        }
        else
        {
            currentWeapon.NoAttack();
        }


        currentAnimation = newAnim;
        armAnimator.CrossFadeInFixedTime(currentAnimation.state, transition);
    }

    public int GetCurrentParry()
    {
        if (currentParry == 0)
        {
            currentParry = 1;
        }
        else
        {
            currentParry = 0;
        }
        return currentParry;
    }
    public void SetCurrentPerfectParry()
    {
        if (currentPerfectParry == 0)
        {
            currentPerfectParry = 1;
        }
        else
        {
            currentPerfectParry = 0;
        }
    }

    #endregion

    #region Called from this class
    public void SetMovement(Vector2 movement)
    {
        armAnimator.SetFloat("MovementX", movement.x);
        armAnimator.SetFloat("MovementZ", movement.y);
    }
    //private void SetUpParryData(Weapon attackingWeapon, Vector3 hitPoint)
    //{
    //    parryData.parryType = parryCheck.CheckForParry(this, attackingWeapon.owner);
    //    parryData.hitPoint = hitPoint;
    //    parryData.direction = transform.position - attackingWeapon.owner.Position();
    //    parryData.attackingWeapon = attackingWeapon;
    //    parryData.postureDamage = attackingWeapon.currentAttack.postureDamage;
    //    parryData.defendingWeapon = GetOwnersWeapon();
    //}

    #endregion

    #region Called from other classes
    public void SetNewWeapon(Weapon weapon)
    {
        if (currentWeapon != null)
        {
            currentWeapon.Hidden();
        }

        currentWeapon = weapon;
        currentWeapon.SetOwner(this, cameraController.camTrans, weaponPos);
        
        hitBox.SetCurrentWeapon();
        currentWeapon.Vissible();

        if (currentState != null)
        {
            currentState.UpdateWeapon(weapon);
        }

        SwitchState(idleState);
    }

    public bool CanSwitchWeapon()
    {
        if (currentState == attackState || currentState == parryState || currentState == blockState || currentState == uniqueState)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion

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

    #region Inputs

    public void OnMove(Vector2 movement)
    {
        input = movement;
    }
    protected override void Move()
    {
        movementDirection = transform.forward * input.y + transform.right * input.x;
        base.Move();
    }

    private void SetUpInput()
    {
        inputReader.OnAttack += Attack;
        inputReader.OnUnique += Unique;
        inputReader.OnBlock += Block;
        inputReader.OnMoveStarted += Moving;
        inputReader.OnMoveStopped += StoppedMoving;
        inputReader.OnBlockRelease += BlockRelease;

        input = Vector2.zero;
        inputReader.OnMove += OnMove;
        inputReader.OnJump += Jump;
    }
    private void EndInput()
    {
        inputReader.OnAttack -= Attack;
        inputReader.OnUnique -= Unique;
        inputReader.OnBlock -= Block;
        inputReader.OnMoveStarted -= Moving;
        inputReader.OnMoveStopped -= StoppedMoving;
        inputReader.OnBlockRelease -= BlockRelease;

        inputReader.OnMove -= OnMove;
        inputReader.OnJump -= Jump;
    }

    #endregion
}
