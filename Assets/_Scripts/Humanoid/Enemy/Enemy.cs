using System;
using System.Collections;
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


        public float turnThreshold = 0.8f;
        public float DistanceBeforeChase = 5f;
        public float playerDistance = 2f;
        public float playerDistanceThreshold = 1f;
        public float runDistance = 8f;

        [Header("Attacking")]
        [SerializeField] private float attackCooldown;
        [SerializeField] private float attackFOV = 90;

        [Header("Weapons")]
        [SerializeField] private Transform[] weaponPos;
        [SerializeField] private Weapon startWeapon;

        public EnemyAnimator enemyAnimator;
        public HitBox hitbox;


        public Weapon currentWeapon { get; private set; }
        public EnemyState currentState { get; private set; }
        public Anim currentAnimation { get; private set; }

        public Player player { get; private set; }
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
        //public TakedownState takedownState = new TakedownState();

        private int currentCorner;
        private float refreshRateTimer;
        private float animatorRunSpeed;
        private bool isRunning;

        //For attack done when hit
        public int attackDoneState { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            FindReferences();

            SetSpeed(3);

            currentWeapon = Instantiate(startWeapon);
            agent.enabled = false;

            //This is whatever the defaul state in the attack layer is in the animator
            attackDoneState = Animator.StringToHash("AttackDone");


            SwitchState(idleState);
        }
        private void Start()
        {
            if (CheckForWeapon())
            {
                currentWeapon.SetOwner(this, transform, weaponPos);
            }
        }
        private void FindReferences()
        {
            currentPath = new NavMeshPath();
            player = FindObjectOfType<Player>();
            agent = GetComponent<NavMeshAgent>();
        }

        protected override void Update()
        {
            base.Update();
            Debug.Log(currentState);
            currentState.Update();
            AnimationSpeed();

        }
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
        #region Signals to state machine
        public override void Staggered()
        {
            currentState.Staggered();
        }

        public override void Stunned()
        {
            currentState.Stunned();
        }
        public override void Hit()
        {
            currentState.Hit();

            // Add switch to idle instead of walk, and change attack animation to AttackDone


        }
        public void Takedown()
        {
            currentState.Takedown();
        }

        #endregion

        protected override void Move()
        {

            if(currentCorner < currentPath.corners.Length)
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

        //Called from the state machine

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
        public bool CheckView()
        {
            if(Vector3.Angle(player.Position() - transform.position, InFront() - transform.position) < attackFOV * 0.5f)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            RotateToTarget(lookAtTarget, walkToTarget);
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
            enemyAnimator.animator.CrossFadeInFixedTime(currentAnimation.state, transition);
        }

        //Needs to be in update

        public void SetLookAtAndForward(Vector3 lookAtTarget, Vector3 forwardTarget)
        {
            this.lookAtTarget = lookAtTarget;
            this.forwardTarget = forwardTarget;
        }
        public float CalculateDotProduct()
        {
            Vector3 directionToLookAt = lookAtTarget - transform.position;
            Vector3 directionToTarget = forwardTarget - transform.position;

            return Tools.Dot(directionToLookAt, directionToTarget);
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
        public void SpeedByDist(float dist)
        {
            if(dist > runDistance)
            {
                if (!isRunning)
                {
                    isRunning = true;
                    SetSpeed(6);
                }
            }
            else
            {
                if(isRunning && dist < playerDistance + playerDistanceThreshold)
                {
                    isRunning= false;
                    SetSpeed(3);

                }
            }
        }


        
        //Called from other scripts
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
}



