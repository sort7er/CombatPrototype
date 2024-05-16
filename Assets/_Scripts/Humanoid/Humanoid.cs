using System;
using UnityEngine;
using Stats;

public class Humanoid : MonoBehaviour
{

    public event Action OnJump;
    public event Action OnFalling;
    public event Action OnLanding;

    [Header("Humanoid")]
    [SerializeField] protected float startSpeed = 6;
    [SerializeField] protected float jumpForce = 8;
    [Range(0, 1)]
    [SerializeField] protected float airMultiplier = 0.4f;
    [SerializeField] protected float groundDistance = 0.3f;
    [SerializeField] protected float groundDrag = 8;
    [SerializeField] protected float fallingForce = 6;
    [SerializeField] protected LayerMask groundLayer = 3;

    [Header("Stunned")]
    public float stunnedDuration;
    public Rigidbody rb { get; private set; }
    public Health health { get; private set; }

    //This is when the owner is parrying
    private bool startParryTimer;
    private float parryTimerLimit = 1;
    public float parryTimer { get; private set; }

    //This is when reciving an attack
    public float attackParryWindow { get; private set; }
    public float attackPerfectParryWindow { get; private set; }
    public float attackTooLateWindow { get; private set; }

    public bool isBlocking { get; private set; }

    protected bool canMove;

    
    protected Vector3 movementDirection;
    protected Vector3 rotationDirection;


    private float movementSpeed;
    private bool isJumping;
    private bool isFalling;



    protected virtual void Awake()
    {
        movementSpeed = startSpeed;
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        if (GroundCheck())
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        SpeedControl();



        //Event checks
        if (!GroundCheck())
        {
            if (!isJumping && !isFalling)
            {
                OnFalling?.Invoke();
            }
            isJumping = false;
            isFalling = true;
        }
        else if(isFalling && GroundCheck())
        {
            OnLanding?.Invoke();
            isFalling = false;
        }

        //Parry timer
        if (startParryTimer)
        {
            if(parryTimer < parryTimerLimit)
            {
                parryTimer += Time.deltaTime;
            }
            else
            {
                ResetParryTimer();
                startParryTimer = false;
            }
        }


    }
    protected virtual void FixedUpdate()
    {
        if (!GroundCheck())
        {
            rb.AddForce(Vector3.down * fallingForce, ForceMode.Force);
        }

        if (canMove)
        {
            Move();
            Rotate();
        }
    }
    protected virtual void Move()
    {
        if (GroundCheck())
        {
            rb.AddForce(movementDirection.normalized * movementSpeed * 10, ForceMode.Force);
        }
        else
        {
            rb.AddForce(movementDirection.normalized * airMultiplier * movementSpeed * 10, ForceMode.Force);
        }
    }

    protected virtual void Rotate()
    {
        rb.angularVelocity = rotationDirection;
    }

    protected virtual void Jump()
    {
        if (GroundCheck())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);


            isJumping = true;
            OnJump?.Invoke();
        }
    }
    protected virtual void SetSpeed(float newSpeed)
    {
        movementSpeed = newSpeed;
    }

    public bool GroundCheck()
    {
        if (Physics.CheckSphere(transform.position, groundDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Limits the velocity on the x, z plane
    protected void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    //Common methods that can be used to signal a state machine

    public virtual void Stunned()
    {

    }
    public virtual void Staggered()
    {

    }
    public virtual void Parry()
    {

    }
    public virtual void PerfectParry()
    {

    }


    //Called from other classes
    public void DisableMovement()
    {
        canMove = false;
    }
    public void EnableMovement()
    {
        canMove = true;
    }
    public void ResetForce()
    {
        rb.velocity = Vector3.zero;
    }
    public void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }
    public void SetRotation(Quaternion rotation)
    {
        rb.MoveRotation(rotation);
    }
    public Vector3 Position()
    {
        return transform.position;
    }
    public Quaternion Rotation()
    {
        return transform.rotation;
    }
    public Vector3 Movement()
    {
        return movementDirection;
    }
    public void IsBlocking()
    {
        isBlocking = true;
    }
    public void IsNotBlocking()
    {
        isBlocking = false;
    }

    public void SetTransform(Transform newTransform)
    {
        rb.position = newTransform.position;
        rb.rotation = newTransform.rotation;
        transform.position = newTransform.position;
        transform.rotation = newTransform.rotation;
    }
    public void StartParryTimer()
    {
        startParryTimer= true;
        ResetParryTimer();
    }
    public void ResetParryTimer()
    {
        parryTimer = 0;
    }
    //public void UpdateParryTimer(float time)
    //{
    //    parryTimer = time;
    //}
    public void SetAttackData(float parryTime, float perfectParryTime, float tooLateTime)
    {
        attackParryWindow = parryTime;
        attackPerfectParryWindow = perfectParryTime;
        attackTooLateWindow = tooLateTime;
    }
}
