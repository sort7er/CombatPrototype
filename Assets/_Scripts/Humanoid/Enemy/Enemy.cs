using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

namespace EnemyAI
{
    public class Enemy : Humanoid
    {
        [Header("Navigation")]
        [SerializeField] private float navMeshRefreshRate = 0.2f;
        [SerializeField] private float waypointDistance = 1f;
        [SerializeField] private float rotationSlerp = 10;
        public float playerDistance = 2f;

        [Header("Attacking")]
        [SerializeField] private float attackCooldown;

        [Header("Weapons")]
        [SerializeField] private Transform[] weaponPos;
        [SerializeField] private Weapon startWeapon;


        public EnemyAnimator enemyAnimator;
        public Player player { get; private set; }
        public NavMeshAgent agent { get; private set; }
        public NavMeshPath currentPath { get; private set; }
        public Vector3 currentTarget { get; private set; }
        public Vector3 lookAtTarget { get; private set; }

        //State machine
        public EnemyState currentState;
        public ChaseState chaseState = new ChaseState();
        public AttackState attackState = new AttackState();
        //public StaggeredState staggeredState = new StaggeredState();
        //public TakedownState takedownState = new TakedownState();

        private int currentCorner;
        private float refreshRateTimer;
        
        protected override void Awake()
        {
            base.Awake();
            FindReferences();

            //For now, no navmeshagent
            agent.enabled = false;
            SwitchState(chaseState);

        }
        private void Start()
        {
            startWeapon.SetOwner(this, transform, weaponPos);
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
            currentState.Update();
        }

        public void Takedown()
        {

        }

        protected override void Move()
        {

            if(currentCorner < currentPath.corners.Length)
            {
                currentTarget = currentPath.corners[currentCorner];

                if (Vector3.Distance(currentTarget, transform.position) < waypointDistance)
            {
                currentCorner++;
            }
            }

            movementDirection = currentTarget - transform.position;

            base.Move();

            
        }

        //Called from the state machine
        public void SwitchState(EnemyState state)
        {
            currentState = state;
            currentState.Enter(this);
        }
        public void SetTarget(Vector3 target)
        {
            if (refreshRateTimer < navMeshRefreshRate)
            {
                refreshRateTimer += Time.deltaTime;
            }
            else
            {
                MoveTo(target);
                refreshRateTimer = 0;
            }
            LookAtTarget(target);
        }
        public float CalculateDotProduct()
        {
            Vector3 directionToLookAt = lookAtTarget - transform.position;
            Vector3 directionToTarget = currentTarget - transform.position;

            Vector3 perp = Vector3.Cross(directionToLookAt.normalized, directionToTarget.normalized);

            return Vector3.Dot(perp, Vector3.up);
        }

        //Called from this class
        private void LookAtTarget(Vector3 target)
        {
            lookAtTarget = target;
            Vector3 alteredPlayerPos = new Vector3(lookAtTarget.x, transform.position.y, lookAtTarget.z);
            Vector3 playerDirection = alteredPlayerPos - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSlerp);
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



