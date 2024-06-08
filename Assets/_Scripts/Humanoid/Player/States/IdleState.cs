using UnityEngine;
namespace PlayerSM
{
    public class IdleState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);
                
            if (player.isFalling)
            {
                LeaveState(fallState);
            }
            else if (player.isMoving)
            {
                player.SetAnimation(archetype.walk);
            }
            else
            {
                player.SetAnimation(archetype.idle);
            }
        }

        public override void Moving()
        {
            player.SetAnimation(archetype.walk);
        }
        public override void StoppedMoving()
        {
            player.SetAnimation(archetype.idle);
        }
        public override void Jump()
        {
            LeaveState(jumpState);
        }
        public override void Fall()
        {
            LeaveState(fallState);
        }
        public override void Attack()
        {
            LeaveState(attackState);
        }
        public override void Block()
        {
            LeaveState(blockState);
        }
        public override void Parry()
        {
            LeaveState(parryState);
        }
        public override void PerfectParry()
        {
            LeaveState(perfectParryState);
        }
        public override void Unique()
        {
            LeaveState(uniqueState);
        }
        public override void Hit()
        {
            LeaveState(hitState);
        }

    }

}
