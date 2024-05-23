using UnityEngine;

namespace Actions
{
    public class FallState : ActionState
    {
        public override void Enter(PlayerActions actions)
        {
            base.Enter(actions);
            actions.SetAnimation(archetype.fall);
        }

        public override void Update()
        {
            base.Update();
            if (actions.player.GroundCheck())
            {
                actions.Landing();
            }
        }
        public override void Landing()
        {
            LeaveState(idleState);
        }
    }
}

