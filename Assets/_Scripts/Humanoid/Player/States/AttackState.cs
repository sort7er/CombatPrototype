using UnityEngine;

public class AttackState : ActionState
{
    public override void Enter(PlayerActions actions)
    {
        base.Enter(actions);
        Debug.Log("Attack");
        actions.SetAnimation(archetype.attacks[0], 0);

        actions.InvokeMethod(AttackDone, actions.currentAnimation.duration);
    }

    private void AttackDone()
    {
        actions.SwitchState(actions.idleState);
    }

}
