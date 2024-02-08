using UnityEngine;

public class IdleState : ActionState
{
    public override void Enter(PlayerActions actions)
    {
        base.Enter(actions);
        Debug.Log("Idle");
        actions.SetAnimation(archetype.idle);
    }

    public override void Attack()
    {
        actions.SwitchState(actions.attackState);
    }

}
