using UnityEngine;

namespace Actions
{

    public class PerfectParryState : ActionState
    {
        public override void Enter(PlayerActions actions)
        {
            base.Enter(actions);
            canChain = true;
            actionDone = false;
            SetUpcommingAction(QueuedAction.None);

            actions.SetAnimation(archetype.parry[2], 0.05f);
            actions.InvokeMethod(EndParry, actions.currentAnimation.duration);
        }



        private void EndParry()
        {
            actions.StopMethod();
            actions.SwitchState(actions.idleState);
        }

        public override void Attack()
        {
            if (actionDone)
            {
                DoFollowUpAttack();
            }
            else if (CheckUpcommingAction())
            {
                SetUpcommingAction(QueuedAction.Attack);
            }
        }
        public override void Block()
        {
            if (actionDone)
            {
                actions.StopMethod();
                actions.SwitchState(actions.blockState);
            }
            else
            {
                actions.player.StartParryTimer();
            }
        }

        public override void ActionDone()
        {
            if (upcommingAction == QueuedAction.Attack)
            {
                DoFollowUpAttack();
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

        private void DoFollowUpAttack()
        {
            actions.StopMethod();
            actions.player.ResetParryTimer();
            actions.SwitchState(actions.parryAttackState);
        }
    }

}
