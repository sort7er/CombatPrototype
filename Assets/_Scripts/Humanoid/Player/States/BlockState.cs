using UnityEngine;

namespace PlayerSM
{
    public class BlockState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);
            ResetValues();

            player.InvokeMethod(CanRelease, archetype.block.duration);
            player.SetAnimation(archetype.block, 0.1f);
            player.StartParryTimer();

        }

        #region Queuing methods
        public override void Attack()
        {
            QueueAttack(() => LeaveStateAndDo(attackState, NotBlocking));
        }
        public override void BlockRelease()
        {
            QueueIdle(EndBlocking);
        }
        private void CanRelease()
        {
            player.IsBlocking();
            QueueActionDone(() => LeaveStateAndDo(attackState, NotBlocking), null, EndBlocking);
        }
        #endregion

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
