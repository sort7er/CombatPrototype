using UnityEngine;
using PlayerSM;
using Stats;
using System;

public class Player : Humanoid
{
    [Header("Values")]
    public float blockWaitTime = 0;
    public float uniqueCoolDown = 8f;

    [Header("References")]
    public Unique unique;
    public WeaponSwitcher weaponSwitcher;
    public InputReader inputReader;
    public CameraController cameraController;
    public TargetAssistance targetAssistance;
    public Transform[] abilityTransforms;
    public HandEffects handEffects;
    public PlayerState currentState { get; private set; }
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
    public StunnedState stunnedState = new StunnedState();
    public MeleeState meleeState = new MeleeState();
    public RangedState rangedState = new RangedState();

    public int currentParry { get; private set; }
    public int currentPerfectParry { get; private set; }
    
    public bool canUseUnique { get; private set; }



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
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        currentState.FixedUpdate();
    }
    protected override void LateUpdate()
    {
        currentState.LateUpdate();
    }
    #endregion

    #region Signal states

    public override void Stunned()
    {
        currentState.Stunned();
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
    public void Melee()
    {
        currentState.Melee();
    }
    public void Ranged()
    {
        currentState.Ranged();
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
    public void AbilityPing()
    {
        currentState.AbilityPing();
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
    public override void Hit(Attack attack, Humanoid attacker, Vector3 hitPoint)
    {
        base.Hit(attack, attacker, hitPoint);

        ParryType parryType = parryCheck.CheckForParry(this, attacker);
        Vector3 direction = (transform.position - attacker.Position()).normalized;

        if (parryType == ParryType.None)
        {
            AddForce(direction * attacker.currentWeapon.pushbackForce);
            health.TakeDamage(attacker, hitPoint);
            currentState.Hit();
        }
        else
        {
            parryCheck.ReturnPostureDamage(attack, attacker, hitPoint, parryType, direction);
        }
    }

    #endregion

    #region Called from state machine
    public void SwitchState(PlayerState state)
    {
        currentState = state;
        currentState.Enter(this);
    }

    public int GetCurrentParry()
    {
        if (currentParry == 0)
        {
            currentParry = currentWeapon.archetype.parry.Length - 1;
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
            currentPerfectParry = currentWeapon.archetype.perfectParry.Length - 1;
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
        animator.SetFloat("MovementX", movement.x);
        animator.SetFloat("MovementZ", movement.y);
    }
    #endregion

    #region Called from other classes
    public override void SetNewWeapon(Weapon weapon)
    {
        base.SetNewWeapon(weapon);

        currentWeapon.SetOwner(this, cameraController.camTrans, weaponTransform);

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
    public void CanUseUnique()
    {
        canUseUnique = true;
    }
    public void CannotUseUnique()
    {
        canUseUnique = false;
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
        inputReader.OnMelee += Melee;
        inputReader.OnRanged += Ranged;

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
        inputReader.OnMelee -= Melee;
        inputReader.OnRanged -= Ranged;

        inputReader.OnMove -= OnMove;
        inputReader.OnJump -= Jump;
    }

    #endregion
}
