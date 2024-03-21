using System;
using UnityEngine;

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

        movement = Vector2.Lerp(movement, input, Time.deltaTime * 10);

        playerActions.SetMovement(movement);
    }
}
