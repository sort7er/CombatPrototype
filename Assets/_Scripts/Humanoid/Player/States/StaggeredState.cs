using UnityEngine;

namespace PlayerSM
{
    public class StaggeredState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);

            Anim staggeredAnim = weapon.archetype.staggered;

            player.SetAnimation(staggeredAnim, 0);
            player.InvokeMethod(StaggeredDone, staggeredAnim.duration);
            ResetValues();
        }
        #region Queuing methods 
        public override void Attack()
        {
            DoOrQueueAction(() => LeaveState(attackState));
        }
        public override void Block()
        {
            DoOrQueueAction(() => LeaveState(blockState));
        }
        #endregion

        public void StaggeredDone()
        {
            LeaveState(idleState);
        }
    }
}
