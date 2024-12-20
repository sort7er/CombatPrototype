using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI
{
    public class Enemy : Humanoid
    {
        [Header("Navigation")]
        [SerializeField] private float navMeshRefreshRate = 0.2f;
        [SerializeField] private float waypointDistance = 1f;
        [SerializeField] private float rotationSlerp = 10;


        public float DistanceBeforeChase = 5f;
        public float minTargetDistance = 2f;
        public float playerDistanceThreshold = 1f;
        public float runDistance = 8f;

        [Header("Attacking")]
        public Humanoid target;
        [SerializeField] private float attackCooldown;
        [SerializeField] private float attackFOV = 90;
        
        public float attackParryPeriod = 0.25f;

        [Header("Parrying")]
        [SerializeField] private float parryFOV = 160;

        [Header("Weapons")]
        [SerializeField] private Weapon startWeapon;

        [Header("References")]
        public EnemyBehaviour enemyBehaviour;
        public EnemyAnimator enemyAnimator;
        public EnemyDissolve enemyDissolve;

        public EnemyState currentState { get; private set; }
        public EnemyState nextParryState { get; private set; }
        public NavMeshAgent agent { get; private set; }
        public NavMeshPath currentPath { get; private set; }
        public Vector3 currentMoveToTarget { get; private set; }
        public Vector3 lookAtTarget { get; private set; }
        public Vector3 forwardTarget { get; private set; }

        //State machine
        public IdleState idleState = new IdleState();
        public ChaseState chaseState = new ChaseState();
        public AttackState attackState = new AttackState();
        public ParryState parryState = new ParryState();
        public StaggeredState staggeredState = new StaggeredState();
        public StunnedState stunnedState = new StunnedState();
        public HitState hitState = new HitState();
        public StandbyState standbyState = new StandbyState();
        public BlockState blockState = new BlockState();
        public PerfectParryState perfectParryState = new PerfectParryState();
        public ParryAttackState parryAttackState = new ParryAttackState();
        public TakedownState takedownState = new TakedownState();
        public DeadState deadState = new DeadState();



        //For attack done when hit
        public int attackDoneState { get; private set; }
        public int stunnedDoneState { get; private set; }

        private int currentCorner;
        private float refreshRateTimer;
        private float animatorRunSpeed;
        private bool isRunning;

        public bool justCut { get; private set; }
        public Vector3 planeNormal { get; private set; }

        public int currentPerfectParry { get; private set; }

        private Quaternion startRotation;
        private float timeElapsed;


        #region Setup
        protected override void Awake()
        {
            base.Awake();
            FindReferences();
            SetUpValues();


            SwitchState(idleState);
        }
        private void Start()
        {
            SetNewWeapon(Instantiate(startWeapon));

            if (CheckForWeapon())
            {
                currentWeapon.SetOwner(this, transform, weaponTransform);
            }
        }
        private void FindReferences()
        {
            currentPath = new NavMeshPath();
            agent = GetComponent<NavMeshAgent>();
            if(target == null)
            {
                SetTarget(FindObjectOfType<Player>());
            }
        }
        private void SetUpValues()
        {
            SetSpeed(3);
            SetNextParryState(parryState);
            agent.enabled = false;
            attackDoneState = Animator.StringToHash("AttackDone");
            stunnedDoneState = Animator.StringToHash("StunnedDone");
            target.OnAttack += TargetAttacking;

        }
        protected override void Update()
        {   
            if(currentState != deadState)
            {
                base.Update();
                currentState.Update();
                AnimationSpeed();
            }
        }
        #endregion

        #region Signals to state machine

        public void Takedown(Humanoid attacker)
        {
            SetTarget(attacker);
            currentState.Takedown();
        }
        public void TargetAttacking(Attack attack)
        {
            currentState.TargetAttacking();
        }
        public void TargetQueingAttack()
        {
            currentState.TargetQueingAttack();
        }
        // From humanoid
        public override void Staggered()
        {
            currentState.Staggered();
        }

        public override void Stunned()
        {
            currentState.Stunned();
        }
        public override void Hit(Attack attack, Humanoid attacker, Vector3 hitPoint)
        {
            base.Hit(attack, attacker, hitPoint);
            currentState.Hit();
            SetTarget(attacker);
        }
        public void HitJustCut(Attack attack, Humanoid attacker, Vector3 hitPoint, Vector3 planeNormal)
        {
            justCut = true;
            this.planeNormal = planeNormal;
            Hit(attack, attacker, hitPoint);
        }
        public override void OverlapCollider()
        {
            currentState.OverlapCollider();
        }
        public override void Dead()
        {
            SwitchState(deadState);

            //gameObject.SetActive(false);
        }

        #endregion

        #region Called from the state machine

        public void SetCurrentPerfectParry()
        {
            if (currentPerfectParry == 0)
            {
                currentPerfectParry = currentWeapon.archetype.enemyPerfectParrys.Length - 1;
            }
            else
            {
                currentPerfectParry = 0;
            }
        }
        public void SetNextParryState(EnemyState enemyState)
        {
            nextParryState = enemyState;
        }
        public void SetTarget(Humanoid target)
        {
            this.target = target;
        }
        public bool CheckForWeapon()
        {
            if (currentWeapon != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Target
        public bool InsideAttackFOV()
        {
            return InsideFOV(attackFOV);
        }
        public bool InsideParryFOV()
        {
            return InsideFOV(parryFOV);
        }
        private bool InsideFOV(float FOV)
        {
            if (Vector3.Angle(target.Position() - transform.position, InFront() - transform.position) < FOV * 0.5f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public float DistanceToTarget()
        {
            return Vector3.Distance(target.Position(), Position());
        }
        public Vector3 DirectionToTarget()
        {
            return (target.Position() - transform.position).normalized;
        }
        public void SwitchState(EnemyState state)
        {
            currentState = state;
            currentState.Enter(this);
        }
        public void MoveToTarget(Vector3 walkToTarget, Vector3 lookAtTarget)
        {
            if (refreshRateTimer < navMeshRefreshRate)
            {
                refreshRateTimer += Time.deltaTime;
            }
            else
            {
                MoveTo(walkToTarget);
                refreshRateTimer = 0;
            }
            RotateToTarget(lookAtTarget, currentMoveToTarget);
        }

        public void SetAnimationWithInt(int state, float transition = 0)
        {
            enemyAnimator.animator.CrossFadeInFixedTime(state, transition);
        }
        public void RotateToTarget(Vector3 lookAtTarget, Vector3 forwardTarget)
        {
            // This is here because to set the targets
            SetLookAtAndForward(lookAtTarget, forwardTarget);
            Vector3 alteredPlayerPos = new Vector3(lookAtTarget.x, transform.position.y, lookAtTarget.z);
            Vector3 targetDirection = alteredPlayerPos - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion slerpedRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSlerp);
            SetRotation(slerpedRotation);
        }

        public void SetStartRotation(Quaternion startRot)
        {
            startRotation = startRot;
            timeElapsed = 0;
        }

        public void RotateToTarget(Vector3 lookAtTarget, Vector3 forwardTarget, float duration)
        {
            // This is here because to set the targets
            SetLookAtAndForward(lookAtTarget, forwardTarget);
            Vector3 alteredPlayerPos = new Vector3(lookAtTarget.x, transform.position.y, lookAtTarget.z);
            Vector3 targetDirection = alteredPlayerPos - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            if (timeElapsed < duration)
            {

                float t = timeElapsed / duration;
                SetRotation(Quaternion.Slerp(startRotation, targetRotation, t));
                timeElapsed += Time.deltaTime;
            }
        }
        public void SetLookAtAndForward(Vector3 lookAtTarget, Vector3 forwardTarget)
        {
            this.lookAtTarget = lookAtTarget;
            this.forwardTarget = forwardTarget;
        }
        public void SpeedByDist(float dist)
        {
            if (dist > runDistance)
            {
                if (!isRunning)
                {
                    isRunning = true;
                    SetSpeed(6);
                }
            }
            else
            {
                if (isRunning && dist < minTargetDistance + playerDistanceThreshold)
                {
                    isRunning = false;
                    SetSpeed(3);

                }
            }
        }

        #endregion

        #region Called from this class
        private void AnimationSpeed()
        {
            if (isRunning)
            {
                animatorRunSpeed = Mathf.Lerp(animatorRunSpeed, 1, Time.deltaTime * 5);

            }
            else
            {
                animatorRunSpeed = Mathf.Lerp(animatorRunSpeed, 0, Time.deltaTime * 5);
            }
            enemyAnimator.animator.SetFloat("MovementZ", animatorRunSpeed);
        }
        protected override void Move()
        {

            if (currentCorner < currentPath.corners.Length)
            {
                currentMoveToTarget = currentPath.corners[currentCorner];

                if (Vector3.Distance(currentMoveToTarget, transform.position) < waypointDistance)
                {
                    currentCorner++;
                }
            }

            movementDirection = currentMoveToTarget - transform.position;

            base.Move();
        }
        public void MoveTo(Vector3 target)
        {
            agent.enabled = true;
            canMove = FindPath(target);
            agent.enabled = false;
            currentCorner = 1;
        }

        public bool FindPath(Vector3 target)
        {
            return agent.CalculatePath(target, currentPath);
        }
        #endregion

        #region Called from other classes
        public float CalculateDotProduct()
        {
            Vector3 directionToLookAt = lookAtTarget - transform.position;
            Vector3 directionToTarget = forwardTarget - transform.position;

            return Tools.Dot(directionToLookAt, directionToTarget);
        }
        #endregion

    }
}



