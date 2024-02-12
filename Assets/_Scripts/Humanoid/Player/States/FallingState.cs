using UnityEngine;

public class FallState : ActionState
{
    public override void Enter(PlayerActions actions)
    {
        base.Enter(actions);
        actions.SetAnimation(archetype.fall);
    }

    public override void Landing()
    {
        actions.SwitchState(actions.idleState);
    }
}
