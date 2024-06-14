using UnityEngine;
namespace PlayerSM
{
    public class StunnedState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);

            Anim stunned = weapon.archetype.stunned;

            player.SetAnimation(stunned, 0);
            player.InvokeMethod(EndStunned, stunned.duration);
        }

        public override void Attack()
        {
            DoOrQueueAttack(() => LeaveState(attackState));
        }
        public override void Block()
        {

            DoOrQueueBlock(() => LeaveState(blockState));
        }

        private void EndStunned()
        {
            LeaveState(idleState);
        }

    }
}
