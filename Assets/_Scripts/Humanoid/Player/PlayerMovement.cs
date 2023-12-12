using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Humanoid
{
    [Header("Values")]
    public float playerSpeed;
    public float jumpForce;
    public float airMultiplier;

    [Header("References")]
    public TextMeshProUGUI speed;

    public bool canMove { get; private set; }

    private Vector2 input;
    private Vector3 movementDirection;

    protected override void Awake()
    {
        base.Awake();
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


    protected override void Update()
    {
        base.Update();
        SpeedControl();

        if (speed!= null)
        {
            speed.text = "Speed: " + rb.velocity.magnitude.ToString("F2");
        }

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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

    public void DisableMovement()
    {
        canMove = false;
    }
    public void EnableMovement()
    {
        canMove = true;
    }

}
