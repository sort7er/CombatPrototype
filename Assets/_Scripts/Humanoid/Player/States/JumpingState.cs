using UnityEngine;

public class JumpState : ActionState
{
    public override void Enter(PlayerActions actions)
    {
        base.Enter(actions);
        actions.SetAnimation(archetype.jump, 0.1f);
        actions.InvokeMethod(StartFall, actions.currentAnimation.duration);
    }
    public override void Landing()
    {
        actions.StopMethod();
        actions.SwitchState(actions.idleState);
    }

    private void StartFall()
    {
        actions.SwitchState(actions.fallState);
    }
}
