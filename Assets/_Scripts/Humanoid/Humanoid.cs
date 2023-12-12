using UnityEngine;

public class Humanoid : MonoBehaviour
{
    [Header("Humanoid")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float groundDistance;
    [SerializeField] protected float groundDrag;
    [SerializeField] protected float fallingForce;


    protected Rigidbody rb;


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
}
