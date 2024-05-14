using UnityEngine;

namespace Actions
{
    public class ParryState : ActionState
    {
        public override void Enter(PlayerActions actions)
        {
            base.Enter(actions);

            canChain = true;
            actionDone = false;
            SetUpcommingAction(QueuedAction.None);

            actions.SetAnimation(archetype.parry[actions.GetCurrentParry()], 0.05f);
            actions.StopMethod();
            actions.InvokeMethod(EndParry, actions.currentAnimation.duration);
        }

        public override void Attack()
        {
            if (actionDone)
            {
                actions.StopMethod();
                actions.player.ResetParryTimer();
                actions.SwitchState(actions.attackState);
            }
            else if (CheckUpcommingAction())
            {
                SetUpcommingAction(QueuedAction.Attack);
            }
        }

        public override void Block()
        {
            if(actionDone)
            {
                actions.StopMethod();
                actions.SwitchState(actions.blockState);
            }
            else if (CheckUpcommingAction())
            {
                SetUpcommingAction(QueuedAction.Block);
            }
        }

        public override void ActionDone()
        {
            if (upcommingAction == QueuedAction.Attack)
            {
                actions.StopMethod();
                actions.player.ResetParryTimer();
                actions.SwitchState(actions.attackState);
            }
            else if (upcommingAction == QueuedAction.Block)
            {
                actions.StopMethod();
                actions.SwitchState(actions.blockState);
            }
            else
            {
                actionDone = true;
            }
        }

        private void EndParry()
        {
            actions.StopMethod();
            actions.player.ResetParryTimer();
            actions.SwitchState(actions.idleState);
        }
    }

}
