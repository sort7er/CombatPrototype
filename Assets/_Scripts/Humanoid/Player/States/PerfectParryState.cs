using UnityEngine;

namespace Actions
{

    public class PerfectParryState : ActionState
    {
        public override void Enter(PlayerActions actions)
        {
            base.Enter(actions);
            ResetValues();


            //Makes the parry alternate from left to right each time
            actions.SetCurrentPerfectParry();
            actions.SetAnimation(archetype.perfectParry[actions.currentPerfectParry], 0.05f);
            actions.InvokeMethod(EndParry, actions.currentAnimation.duration);
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
            QueueBlock(() => LeaveState(actions.blockState));
        }
        public override void ActionDone()
        {
            QueueActionDone(DoFollowUpAttack, () => LeaveState(actions.blockState));
        }
        #endregion

        private void DoFollowUpAttack()
        {
            LeaveStateAndDo(parryAttackState, () => actions.player.ResetParryTimer());
        }
    }

}
