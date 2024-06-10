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

            Attack parry = archetype.perfectParry[player.currentPerfectParry];

            player.SetAttack(parry, 0.05f);
            player.InvokeMethod(EndParry, parry.duration);
        }

        private void EndParry()
        {
            LeaveState(idleState);
        }

        #region Queuing methods 
        public override void Attack()
        {
            DoOrQueueAction(DoFollowUpAttack);
        }
        public override void Block()
        {
            DoOrQueueAction(() => LeaveState(blockState));
        }
        #endregion

        private void DoFollowUpAttack()
        {
            LeaveStateAndDo(parryAttackState, LeavePerfectParry);
        }
        private void LeavePerfectParry()
        {
            player.ResetParryTimer();
            player.StopMethod();
        }
        public override void Hit()
        {
            LeaveState(hitState);
        }
    }

}
