using UnityEngine;

namespace PlayerSM
{
    public class BlockState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);
            ResetValues();
            player.IsBlocking();
            player.InvokeMethod(CanRelease, archetype.block.duration * 1.5f);
            player.SetAnimation(archetype.block);
            player.StartParryTimer();

        }

        #region Queuing methods
        public override void Attack()
        {
            DoOrQueueAction(() => LeaveStateAndDo(attackState, NotBlocking));
        }
        public override void BlockRelease()
        {
            DoOrQueueAction(EndBlocking);
        }
        #endregion
        private void CanRelease()
        {
            if (player.blockReleased)
            {
                EndBlocking();
            }
            else
            {
                CheckQueueOrActionDone();
            }
        }

        public override void Parry()
        {
            LeaveStateAndDo(parryState, NotBlocking);
        }

        public override void PerfectParry()
        {
            LeaveStateAndDo(perfectParryState, NotBlocking);
        }

        private void EndBlocking()
        {
            LeaveStateAndDo(idleState, NotBlocking);
        }
        private void NotBlocking()
        {
            player.IsNotBlocking();
        }
    }

}
