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
                actions.SwitchState(actions.fallState);
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
            actions.SwitchState(actions.jumpState);
        }
        public override void Fall()
        {
            actions.SwitchState(actions.fallState);
        }
        public override void Attack()
        {
            actions.SwitchState(actions.attackState);
        }

        public override void Block()
        {
            actions.SwitchState(actions.blockState);
        }
        public override void Unique()
        {
            actions.SwitchState(actions.uniqueState);
        }

    }

}
