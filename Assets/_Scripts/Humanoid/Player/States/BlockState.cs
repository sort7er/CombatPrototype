using UnityEngine;

public class BlockState : ActionState
{
    private bool parry;

    public override void Enter(PlayerActions actions)
    {
        base.Enter(actions);
        parry = true;
        actions.SetAnimation(archetype.block, 0.1f);
        actions.InvokeMethod(Parrying, actions.parryWindow);
    }

    public override void Parry()
    {
        if(parry)
        {
            actions.StopMethod();
            actions.SwitchState(actions.parryState);
        }
        else
        {
            actions.SwitchState(actions.idleState);
        }
    }

    private void Parrying()
    {
        parry = false;
    }

}
