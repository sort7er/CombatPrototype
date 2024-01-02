using UnityEngine;
using UnityEngine.AI;

public class Enemy : Humanoid
{
    [Header("Values")]
    [SerializeField] private float rotationSlerp = 10;

    public float waypointDistance = 1f;
    public float playerDistance = 1f;

    public Player player { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public NavMeshPath currentPath { get; private set; }
    private Animator enemyAnim;

    //State machine
    public EnemyState currentState;
    public ChaseState chaseState = new ChaseState();
    public AttackState attackState = new AttackState();
    public TakedownState takedownState = new TakedownState();

    public Vector3 currentTarget { get; private set; }
    private int currentCorner;

    protected override void Awake()
    {
        base.Awake();
        FindReferences();
        SwitchState(chaseState);
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


    //Called from other scripts
    public void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    public void Hit()
    {
        enemyAnim.SetTrigger("Hit");
    }





}
