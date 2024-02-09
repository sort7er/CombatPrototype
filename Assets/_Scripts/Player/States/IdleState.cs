using UnityEngine;

public class IdleState : ActionState
{
    public override void Enter(PlayerActions actions)
    {
        base.Enter(actions);

        if(archetype.idle == null)
        {
            Debug.Log("Bruh");
        }

        actions.SetAnimation(archetype.idle);
    }

    public override void Attack()
    {
        actions.SwitchState(actions.attackState);
    }

    public override void Block()
    {
        actions.SwitchState(actions.blockState);
    }

}
