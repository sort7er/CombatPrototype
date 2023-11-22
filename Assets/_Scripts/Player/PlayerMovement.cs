using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Values")]
    public float playerSpeed;
    public float jumpForce;
    public float airMultiplier;
    public float groundDistance;
    public float groundDrag;

    [Header("References")]
    public LayerMask groundLayer;
    public TextMeshProUGUI speed;

    public bool canMove { get; private set; }

    private Rigidbody rb;
    private Vector2 input;
    private Vector3 movementDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        canMove = true;
    }


    //Input actions
    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if(GroundCheck() && context.started)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }


    private void Update()
    {
        if(GroundCheck() )
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        SpeedControl();

        if (speed!= null)
        {
            speed.text = "Speed: " + rb.velocity.magnitude.ToString("F2");
        }

    }

    void FixedUpdate()
    {
        if (canMove)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        movementDirection = transform.forward * input.y + transform.right * input.x;
        if (GroundCheck())
        {
            rb.AddForce(movementDirection.normalized * playerSpeed * 10, ForceMode.Force);
        }
        else
        {
            rb.AddForce(movementDirection.normalized * airMultiplier * playerSpeed * 10, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVel.magnitude > playerSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * playerSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }



    private bool GroundCheck()
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

    public void DisableMovement()
    {
        canMove = false;
    }
    public void EnableMovement()
    {
        canMove = true;
    }

}
