using UnityEngine;
namespace PlayerSM
{
    public class JumpState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);
            player.SetAnimation(archetype.jump, 0.1f);
            player.InvokeMethod(StartFall, player.currentAnimation.duration);
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
