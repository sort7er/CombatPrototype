using UnityEngine;

namespace PlayerSM
{
    public class FallState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);
            player.SetAnimation(archetype.fall);
        }

        public override void Update()
        {
            base.Update();
            if (player.GroundCheck())
            {
                player.Landing();
            }
        }
        public override void Landing()
        {
            LeaveState(idleState);
        }
    }
}

