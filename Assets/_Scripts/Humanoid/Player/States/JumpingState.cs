using UnityEngine;
namespace Actions
{
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
            LeaveState(idleState);
        }

        private void StartFall()
        {
            LeaveState(fallState);
        }
    }

}
