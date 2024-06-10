using UnityEngine;
namespace PlayerSM
{
    public class JumpState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);

            Anim anim = archetype.jump;

            player.SetAnimation(anim, 0.1f);
            player.InvokeMethod(StartFall, anim.duration);
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
