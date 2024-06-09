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
            DoOrQueueAction(() => LeaveState(attackState));
        }
        public override void Block()
        {

            DoOrQueueAction(() => LeaveState(blockState));
        }

        private void EndStunned()
        {
            LeaveState(idleState);
        }

    }
}
