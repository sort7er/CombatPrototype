using UnityEngine;

namespace PlayerSM
{
    public class ParryAttackState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);

            ResetValuesAttack();
            player.SetAnimation(archetype.parryAttack[player.currentPerfectParry], 0.05f);
            player.InvokeMethod(EndAttack, player.currentAnimation.duration);
        }


        #region Queuing methods 
        public override void Attack()
        {
            QueueAttack(() => LeaveState(attackState));
        }
        public override void Block()
        {
            QueueBlock(() => LeaveState(blockState));
        }
        public override void ActionDone()
        {
            QueueActionDone(() => LeaveState(attackState), () => LeaveState(blockState));
        }
        #endregion
        public override void OverlapCollider()
        {
            base.OverlapCollider();
            player.AddForce(player.Forward() * 20);
        }

        private void EndAttack()
        {
            LeaveState(idleState);
        }
    }
}
