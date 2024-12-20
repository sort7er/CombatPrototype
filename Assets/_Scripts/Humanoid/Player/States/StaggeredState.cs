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
            DoOrQueueAttack(() => LeaveState(attackState));
        }
        public override void Block()
        {
            DoOrQueueBlock(() => LeaveState(blockState));
        }
        #endregion

        public void StaggeredDone()
        {
            LeaveState(idleState);
        }
        public override void Hit()
        {
            LeaveState(hitState);
        }
        public override void Stunned()
        {
            LeaveState(stunnedState);
        }
    }
}
