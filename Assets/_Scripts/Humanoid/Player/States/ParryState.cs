using UnityEngine;

namespace PlayerSM
{
    public class ParryState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);

            ResetValues();

            player.SetAnimation(archetype.parry[player.GetCurrentParry()], 0.05f);
            player.StopMethod();
            player.InvokeMethod(EndParry, player.currentAnimation.duration);
        }
        #region Queuing methods 
        public override void Attack()
        {
            QueueAttack(DoAttack);
        }
        public override void Block()
        {
            QueueBlock(() => LeaveState(blockState));
        }
        public override void ActionDone()
        {
            QueueActionDone(DoAttack, () => LeaveState(blockState));
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
    }

}
