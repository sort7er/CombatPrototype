using UnityEngine;

namespace Actions
{
    public class ParryAttackState : ActionState
    {
        public override void Enter(PlayerActions actions)
        {
            base.Enter(actions);

            ResetValuesAttack();
            actions.SetAnimation(archetype.parryfollowUpAttack[actions.currentPerfectParry], 0.05f);
            actions.InvokeMethod(EndAttack, actions.currentAnimation.duration);
        }


        #region Queuing methods 
        public override void Attack()
        {
            QueueAttack(() => LeaveState(actions.attackState));
        }
        public override void Block()
        {
            QueueBlock(() => LeaveState(actions.blockState));
        }
        public override void ActionDone()
        {
            QueueActionDone(() => LeaveState(actions.attackState), () => LeaveState(actions.blockState));
        }
        #endregion
        public override void OverlapCollider()
        {
            base.OverlapCollider();
            actions.player.AddForce(actions.transform.forward * 20);
        }

        private void EndAttack()
        {
            LeaveState(idleState);
        }
    }
}
