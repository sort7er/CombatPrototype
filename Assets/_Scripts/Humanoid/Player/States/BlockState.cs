using UnityEngine;

namespace Actions
{
    public class BlockState : ActionState
    {
        public override void Enter(PlayerActions actions)
        {
            base.Enter(actions);
            ResetValues();

            actions.InvokeMethod(CanRelease, archetype.block.duration);
            actions.SetAnimation(archetype.block, 0.1f);
            actions.player.StartParryTimer();

        }

        #region Queuing methods
        public override void Attack()
        {
            QueueAttack(() => LeaveStateAndDo(actions.attackState, NotBlocking));
        }
        public override void BlockRelease()
        {
            QueueIdle(EndBlocking);
        }
        private void CanRelease()
        {
            actions.player.IsBlocking();
            QueueActionDone(() => LeaveStateAndDo(actions.attackState, NotBlocking), null, EndBlocking);
        }
        #endregion

        public override void Parry()
        {
            LeaveStateAndDo(actions.parryState, NotBlocking);
        }

        public override void PerfectParry()
        {
            LeaveStateAndDo(actions.perfectParryState, NotBlocking);
        }

        private void EndBlocking()
        {
            LeaveStateAndDo(actions.idleState, NotBlocking);
        }
        private void NotBlocking()
        {
            actions.player.IsNotBlocking();
        }
    }

}
