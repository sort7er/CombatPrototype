using System;
using UnityEngine;

public class Humanoid : MonoBehaviour
{

    public event Action OnJump;
    public event Action OnFalling;
    public event Action OnLanding;

    [Header("Humanoid")]
    [SerializeField] protected float movementSpeed = 6;
    [SerializeField] protected float jumpForce = 8;
    [Range(0, 1)]
    [SerializeField] protected float airMultiplier = 0.4f;
    [SerializeField] protected float groundDistance = 0.3f;
    [SerializeField] protected float groundDrag = 8;
    [SerializeField] protected float fallingForce = 6;
    [SerializeField] protected LayerMask groundLayer = 3;
    public Rigidbody rb { get; private set; }
    public Health health { get; private set; }

    protected bool canMove;
    protected Vector3 movementDirection;


    private bool isJumping;
    private bool isFalling;

    protected virtual void Awake()
    {
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

    public virtual void Staggered()
    {

    }

    protected bool GroundCheck()
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

    //Called from other classes
    public void DisableMovement()
    {
        canMove = false;
    }
    public void EnableMovement()
    {
        canMove = true;
    }
    public void ImmediateStop()
    {
        DisableMovement();
        rb.velocity = Vector3.zero;
    }
    public void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
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
}
