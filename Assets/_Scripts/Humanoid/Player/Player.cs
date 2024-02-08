using UnityEngine;

public class Player : Humanoid
{
    private Vector2 input;


    [Header("References")]
    public InputReader inputReader;
    public CameraController cameraController;
    public PlayerActions playerActions;


    protected override void Awake()
    {
        base.Awake();
        GetReferences();
        EnableMovement();
        inputReader.OnMove += OnMove;
        inputReader.OnJump += Jump;
    }
    private void OnDestroy()
    {
        inputReader.OnMove -= OnMove;
        inputReader.OnJump -= Jump;
    }

    //Input actions
    public void OnMove(Vector2 movement)
    {
        input = movement;
    }
    protected override void Move()
    {
        movementDirection = transform.forward * input.y + transform.right * input.x;
        base.Move();
    }

    private void GetReferences()
    {
        playerActions = GetComponent<PlayerActions>();
        cameraController = GetComponent<CameraController>();
        inputReader = GetComponent<InputReader>();
    }

}
