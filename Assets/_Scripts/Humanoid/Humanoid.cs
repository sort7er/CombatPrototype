using UnityEngine;

public class Humanoid : MonoBehaviour
{
    [Header("Humanoid")]
    [SerializeField] protected float movementSpeed = 10;
    [SerializeField] protected float groundDistance;
    [SerializeField] protected float groundDrag;
    [SerializeField] protected float fallingForce;
    [SerializeField] protected LayerMask groundLayer;


    public Rigidbody rb { get; private set; }
    protected bool canMove;


    protected virtual void Awake()
    {
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
    }
    protected virtual void FixedUpdate()
    {
        if (!GroundCheck())
        {
            //Falling speed
            rb.AddForce(Vector3.down * fallingForce, ForceMode.Force);
        }

        if (canMove)
        {
            Move();
        }
    }
    protected virtual void Move()
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



    public Vector3 Position()
    {
        return transform.position;
    }
    protected void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
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
}
