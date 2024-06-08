using UnityEngine;
using PlayerSM;
using System;
using Stats;

public class Player : Humanoid
{

    public event Action OnAttack;

    [Header("Values")]
    public float blockWaitTime = 0;

    [Header("References")]
    public Animator armAnimator;
    public Unique unique;
    public WeaponSwitcher weaponSwitcher;
    public InputReader inputReader;
    public CameraController cameraController;
    public TargetAssistance targetAssistance;

    public Transform[] weaponPos;
    public PlayerState currentState { get; private set; }
    public Anim currentAnimation { get; private set; }
    public bool isMoving { get; private set; }
    public bool isFalling { get; private set; }
    public bool blockReleased { get; private set; }

    public IdleState idleState = new IdleState();
    public JumpState jumpState = new JumpState();
    public FallState fallState = new FallState();
    public AttackState attackState = new AttackState();
    public UniqueState uniqueState = new UniqueState();
    public BlockState blockState = new BlockState();
    public ParryState parryState = new ParryState();
    public PerfectParryState perfectParryState = new PerfectParryState();
    public ParryAttackState parryAttackState = new ParryAttackState();
    public StaggeredState staggeredState = new StaggeredState();
    public HitState hitState = new HitState();

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
        currentState.Staggered();
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
        blockReleased = false;
        currentState.Block();
    }
    public void BlockRelease()
    {
        blockReleased = true;
        currentState.BlockRelease();
    }
    public void ActionStart()
    {
        currentWeapon.Effect();
    }
    public void ActionDone()
    {
        currentState.ActionDone();
        currentWeapon.AttackDone();
    }

    //From humanoid
    public override void OverlapCollider()
    {
        currentState.OverlapCollider();
    }
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
    public override void Hit(Humanoid attacker, Vector3 hitPoint)
    {

        ParryType parryType = parryCheck.CheckForParry(this, attacker);
        Vector3 direction = (transform.position - attacker.Position()).normalized;

        Weapon attackingWeapon = attacker.currentWeapon;

        if (parryType == ParryType.None)
        {
            //Can add hit functionnality here

            AddForce(direction * attackingWeapon.pushbackForce);
            health.TakeDamage(attacker, hitPoint);
        }
        else
        {
            parryCheck.ReturnPostureDamage(attacker, hitPoint, parryType, direction);
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

    public void AttackEvent()
    {
        OnAttack?.Invoke();
    }

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
    #endregion

    #region Called from other classes
    public override void SetNewWeapon(Weapon weapon)
    {
        base.SetNewWeapon(weapon);

        currentWeapon.SetOwner(this, cameraController.camTrans, weaponPos);

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
