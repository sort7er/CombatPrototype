using UnityEngine;

namespace PlayerSM
{
    public class ParryAttackState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);

            ResetValuesAttack();

            Attack attack = archetype.parryAttack[player.currentPerfectParry];

            player.SetAttack(attack);
            player.InvokeMethod(EndAttack, attack.duration);
        }


        #region Queuing methods 
        public override void Attack()
        {
            DoOrQueueAction(() => LeaveState(attackState));
        }
        public override void Block()
        {
            DoOrQueueAction(() => LeaveState(blockState));
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
        public override void Stunned()
        {
            LeaveState(stunnedState);
        }

        public override void Staggered()
        {
            LeaveState(staggeredState);
        }

    }
}
