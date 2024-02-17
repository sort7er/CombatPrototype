using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Humanoid
{
    [Header("Enemy values")]
    [SerializeField] private float rotationSlerp = 10;
    [SerializeField] private float waypointDistance = 1f;
    [SerializeField] private float playerDistance = 1f;

    [Header("Attacking")]
    [SerializeField] private float attackCooldown;


    public Player player { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public NavMeshPath currentPath { get; private set; }
    public Animator enemyAnim { get; private set; }

    public Vector3 currentTarget { get; private set; }
    private int currentCorner;

    protected override void Awake()
    {
        base.Awake();
        FindReferences();

        //For now, no navmeshagent
        agent.enabled = false;

    }

    public void Takedown()
    {

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
        movementDirection = currentTarget - transform.position;

        base.Move();

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
