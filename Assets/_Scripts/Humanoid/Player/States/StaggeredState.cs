using UnityEngine;

namespace PlayerSM
{
    public class StaggeredState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);

            Anim staggeredAnim = weapon.archetype.staggered;

            player.SetAnimation(staggeredAnim);
            player.InvokeMethod(StaggeredDone, staggeredAnim.duration);
        }


        public void StaggeredDone()
        {
            LeaveState(idleState);
        }
    }
}
