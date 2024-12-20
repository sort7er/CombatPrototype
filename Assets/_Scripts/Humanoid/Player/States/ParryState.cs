using UnityEngine;

namespace PlayerSM
{
    public class ParryState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);

            ResetValues();

            Attack parry = archetype.parry[player.GetCurrentParry()];

            player.SetBlock(parry, 0f);
            player.StopMethod();
            player.InvokeMethod(EndParry, parry.duration);
        }
        #region Queuing methods 
        public override void Attack()
        {
            DoOrQueueAttack(DoAttack);
        }
        public override void Block()
        {
            DoOrQueueBlock(() => LeaveState(blockState));
        }
        #endregion

        private void DoAttack()
        {
            LeaveStateAndDo(attackState, () => player.ResetParryTimer());
        }

        private void EndParry()
        {
            LeaveState(idleState);
        }
        public override void Hit()
        {
            LeaveState(hitState);
        }

    }

}
