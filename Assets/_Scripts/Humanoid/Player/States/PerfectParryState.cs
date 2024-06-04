using UnityEngine;

namespace PlayerSM
{

    public class PerfectParryState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);
            ResetValues();


            //Makes the parry alternate from left to right each time
            player.SetCurrentPerfectParry();
            player.SetAnimation(archetype.perfectParry[player.currentPerfectParry], 0.05f);
            player.InvokeMethod(EndParry, player.currentAnimation.duration);
        }

        private void EndParry()
        {
            LeaveState(idleState);
        }

        #region Queuing methods 
        public override void Attack()
        {
            QueueAttack(DoFollowUpAttack);
        }
        public override void Block()
        {
            QueueBlock(() => LeaveState(blockState));
        }
        public override void ActionDone()
        {
            QueueActionDone(DoFollowUpAttack, () => LeaveState(blockState));
        }
        #endregion

        private void DoFollowUpAttack()
        {
            LeaveStateAndDo(parryAttackState, () => player.ResetParryTimer());
        }
    }

}
