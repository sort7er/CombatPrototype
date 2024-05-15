using UnityEngine;

namespace Actions
{
    public class ParryState : ActionState
    {
        public override void Enter(PlayerActions actions)
        {
            base.Enter(actions);

            ResetValues();

            actions.SetAnimation(archetype.parry[actions.GetCurrentParry()], 0.05f);
            actions.StopMethod();
            actions.InvokeMethod(EndParry, actions.currentAnimation.duration);
        }
        #region Queuing methods 
        public override void Attack()
        {
            QueueAttack(DoAttack);
        }
        public override void Block()
        {
            QueueBlock(() => LeaveState(actions.blockState));
        }
        public override void ActionDone()
        {
            QueueActionDone(DoAttack, () => LeaveState(actions.blockState));
        }
        #endregion

        private void DoAttack()
        {
            actions.player.ResetParryTimer();
            LeaveState(actions.attackState);
        }

        private void EndParry()
        {
            LeaveState(actions.idleState);
        }
    }

}
