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
            QueueAttack(() => LeaveState(attackState));
        }
        public override void Block()
        {
            QueueBlock(() => LeaveState(blockState));
        }
        public override void ActionDone()
        {
            QueueActionDone(() => LeaveState(attackState), () => LeaveState(blockState));
        }
        #endregion

        public void StaggeredDone()
        {
            LeaveState(idleState);
        }
    }
}
