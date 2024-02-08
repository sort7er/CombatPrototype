using UnityEngine;
using HumanoidTypes;


namespace HumanoidTypes
{
    public enum OwnerType
    {
        Player,
        Enemy
    }
}

public class Humanoid : MonoBehaviour
{
    [Header("Humanoid")]
    public OwnerType ownerType;
    [SerializeField] protected float movementSpeed = 6;
    [SerializeField] protected float jumpForce = 8;
    [Range(0, 1)]
    [SerializeField] protected float airMultiplier = 0.4f;
    [SerializeField] protected float groundDistance = 0.3f;
    [SerializeField] protected float groundDrag = 8;
    [SerializeField] protected float fallingForce = 6;
    [SerializeField] protected LayerMask groundLayer = 3;

    public Health health { get; private set; }
    public Rigidbody rb { get; private set; }

    protected bool canMove;
    protected Vector3 movementDirection;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if(TryGetComponent(out Health health))
        {
            this.health = health;
        }
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
    public Vector3 Position()
    {
        return transform.position;
    }
    public Quaternion Rotation()
    {
        return transform.rotation;
    }
}
