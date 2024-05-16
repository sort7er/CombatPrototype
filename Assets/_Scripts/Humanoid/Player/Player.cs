using UnityEngine;
using Actions;
using Stats;

public class Player : Humanoid
{
    private Vector2 input;
    private Vector2 movement;


    [Header("References")]
    public InputReader inputReader;
    public CameraController cameraController;
    public PlayerActions playerActions;
    public HitBox hitBox;
    public Health Health;
    public TargetAssistance targetAssistance;

    protected override void Awake()
    {
        base.Awake();
        EnableMovement();
        input = Vector2.zero;
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
    protected override void Update()
    {
        base.Update();

        movement = Vector2.Lerp(movement, input, Time.deltaTime * 5);

        playerActions.SetMovement(movement);
    }
    public override void Parry()
    {
        playerActions.Parry();
    }
    public override void PerfectParry()
    {
        playerActions.PerfectParry();
    }
    public override void Stunned()
    {

    }
    public override void Staggered()
    {

    }
}
