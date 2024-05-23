using UnityEngine;
namespace Actions
{
    public class IdleState : ActionState
    {
        public override void Enter(PlayerActions actions)
        {
            base.Enter(actions);
            if (actions.isFalling)
            {
                LeaveState(fallState);
            }
            else if (actions.isMoving)
            {
                actions.SetAnimation(archetype.walk);
            }
            else
            {
                actions.SetAnimation(archetype.idle);
            }
        }

        public override void Moving()
        {
            actions.SetAnimation(archetype.walk);
        }
        public override void StoppedMoving()
        {
            actions.SetAnimation(archetype.idle);
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

    }

}
