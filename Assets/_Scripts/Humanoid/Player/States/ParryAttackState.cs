using UnityEngine;

namespace PlayerSM
{
    public class ParryAttackState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);

            ResetValuesAttack();

            Attack attack = archetype.followUpAttacks[player.currentPerfectParry];

            player.SetAttack(attack);
            player.InvokeMethod(EndAttack, attack.duration);
        }


        #region Queuing methods 
        public override void Attack()
        {
            DoOrQueueAttack(() => LeaveState(attackState));
        }
        public override void Block()
        {
            DoOrQueueBlock(() => LeaveState(blockState));
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
