using Actions;
using UnityEngine;

namespace Actions
{
    public class ParryState : ActionState
    {
        private int currentParry;
        private float parryTimer;
        public override void Enter(PlayerActions actions)
        {
            base.Enter(actions);
            DoParry();
        }
        public override void Update()
        {
            base.Update();
            parryTimer += Time.deltaTime;
            actions.player.UpdateParryTimer(parryTimer);
        }
        public override void Parry()
        {
            if (actionDone)
            {
                DoParry();
            }
            else if (canChain && CheckUpcommingAction())
            {
                SetUpcommingAction(QueuedAction.Parry);
            }
        }

        public override void ActionDone()
        {
            if (upcommingAction == QueuedAction.Attack)
            {
                actions.StopMethod();
                actions.player.UpdateParryTimer(0);
                actions.SwitchState(actions.attackState);
            }
            else if (upcommingAction == QueuedAction.Parry)
            {
                DoParry();
            }
            else
            {
                actionDone = true;
            }
        }

        public override void Attack()
        {
            if (actionDone)
            {
                actions.StopMethod();
                actions.player.UpdateParryTimer(0);
                actions.SwitchState(actions.attackState);
            }
            else if (canChain && CheckUpcommingAction())
            {
                SetUpcommingAction(QueuedAction.Attack);
            }
        }

        private void DoParry()
        {
            actionDone = false;
            canChain = false;
            parryTimer = 0;
            SetUpcommingAction(QueuedAction.None);
            actions.SetAnimation(archetype.parry[currentParry], 0.05f);
            actions.StopMethod();
            actions.InvokeMethod(EndParry, actions.currentAnimation.duration);
            UpdateParry();
        }

        private void EndParry()
        {
            actions.SwitchState(actions.idleState);
            actions.player.UpdateParryTimer(0);
        }

        private void UpdateParry()
        {
            if (currentParry == 0)
            {
                currentParry = 1;
            }
            else
            {
                currentParry = 0;
            }
        }
    }

}
