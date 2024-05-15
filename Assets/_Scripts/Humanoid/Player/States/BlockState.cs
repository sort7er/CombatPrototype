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
            QueueAttack(() => LeaveState(actions.attackState));
        }
        public override void BlockRelease()
        {
            QueueIdle(EndBlocking);
        }
        private void CanRelease()
        {
            QueueActionDone(() => LeaveState(actions.attackState), null, EndBlocking);
        }
        #endregion

        public override void Parry()
        {
            LeaveState(actions.parryState);
        }

        public override void PerfectParry()
        {
            LeaveState(actions.perfectParryState);
        }

        private void EndBlocking()
        {
            LeaveState(actions.idleState);
        }
    }

}
