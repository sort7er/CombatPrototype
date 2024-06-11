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

            player.SetAttack(parry);
            player.InvokeMethod(EndParry, parry.duration);
        }

        private void EndParry()
        {
            LeaveState(idleState);
        }

        #region Queuing methods 
        public override void Attack()
        {
            DoOrQueueAction(() => LeaveStateAndDo(parryAttackState, () => player.ResetParryTimer()));
        }
        public override void Block()
        {
            DoOrQueueAction(() => LeaveState(blockState));
        }
        #endregion
        public override void ActionDone()
        {
            base.ActionDone();
            Debug.Log("Yeah");
        }

        public override void Hit()
        {
            LeaveState(hitState);
        }
    }

}
