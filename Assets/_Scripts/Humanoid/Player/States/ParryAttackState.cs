using UnityEngine;

namespace Actions
{
    public class ParryAttackState : ActionState
    {
        public override void Enter(PlayerActions actions)
        {
            base.Enter(actions);

            actionDone = false;
            canChain = false;

            SetUpcommingAction(QueuedAction.None);
            actions.SetAnimation(archetype.parryfollowUpAttack, 0.05f);
            actions.InvokeMethod(EndAttack, actions.currentAnimation.duration);
        }


        public override void Attack()
        {
            if (actionDone)
            {
                actions.StopMethod();
                actions.SwitchState(actions.attackState);
            }
            else if (CheckUpcommingAction())
            {
                SetUpcommingAction(QueuedAction.Attack);
            }

        }
        public override void OverlapCollider()
        {
            base.OverlapCollider();
            actions.player.AddForce(actions.transform.forward * 20);
        }
        public override void Block()
        {
            if (actionDone)
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

        private void EndAttack()
        {
            actions.SwitchState(actions.idleState);
        }
    }
}
