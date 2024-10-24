using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using EnemyStates;

public class Enemy : Humanoid
{
    [Header("Movement Values")]
    [SerializeField] private float rotationSlerp = 10;
    public float waypointDistance = 1f;
    public float playerDistance = 1f;

    [Header("Attacking")]
    public float attackCooldown;


    public Player player { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public NavMeshPath currentPath { get; private set; }
    public Archetype currentArchetype { get; private set; }
    public Animator enemyAnim { get; private set; }

    private WeaponContainer weaponContainer;

    //State machine
    public EnemyState currentState;
    public ChaseState chaseState = new ChaseState();
    public AttackState attackState = new AttackState();
    public StaggeredState staggeredState = new StaggeredState();
    public TakedownState takedownState = new TakedownState();

    public Vector3 currentTarget { get; private set; }
    private int currentCorner;

    protected override void Awake()
    {
        base.Awake();
        FindReferences();
        SwitchState(chaseState);
    }
    private void Start()
    {
        currentArchetype = weaponContainer.archetype;
        currentArchetype.archetypeAnimator.OnAttackDone += attackState.AttackDone;
        health.OnPostureDrained += Staggered;
        health.OnStaggerDone += StaggerDone;
    }

    private void OnDestroy()
    {
        if(currentArchetype != null)
        {
            currentArchetype.archetypeAnimator.OnAttackDone -= attackState.AttackDone;
            health.OnPostureDrained -= Staggered;
            health.OnStaggerDone -= StaggerDone;
        }
    }

    protected override void Update()
    {
        base.Update();
        SpeedControl();
        currentState.UpdateState(this);

    }

    public void SwitchState(EnemyState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    //Called from this class
    private void FindReferences()
    {
        currentPath = new NavMeshPath();
        enemyAnim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();
        weaponContainer = GetComponentInChildren<WeaponContainer>();
    }
    protected override void Move()
    {
        currentTarget = currentPath.corners[currentCorner];

        Vector3 movementDirection = currentTarget - transform.position;

        rb.AddForce(movementDirection.normalized * movementSpeed * 10, ForceMode.Force);

        if (Vector3.Distance(currentTarget, transform.position) < waypointDistance)
        {
            if (currentCorner < currentPath.corners.Length)
            {
                currentCorner++;
            }
        }
    }


    //Called from the state machine
    public void LookAtTarget(Vector3 target)
    {
        Vector3 alteredPlayerPos = new Vector3(target.x, transform.position.y, target.z);
        Vector3 playerDirection = alteredPlayerPos - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSlerp);
    }

    public void MoveTo(Vector3 target)
    {
        canMove = FindPath(target);
        currentCorner= 1;
    }

    public bool FindPath(Vector3 target)
    {
        return agent.CalculatePath(target, currentPath);
    }

    //State switching from other scripts

    public void Takedown()
    {
        SwitchState(takedownState);
    }
    public override void Staggered()
    {
        currentState.Staggered(this);
    }
    private void StaggerDone()
    {
        staggeredState.StaggerDone();
    }


    //Called from other scripts
    public void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    public void Hit()
    {
        enemyAnim.SetTrigger("Hit");
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
    public void InvokeCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
    public void StopFunction()
    {
        StopAllCoroutines();
    }
}
