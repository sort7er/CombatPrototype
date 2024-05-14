using UnityEngine;

namespace Actions
{
    public class BlockState : ActionState
    {
        public override void Enter(PlayerActions actions)
        {
            base.Enter(actions);
            actionDone = false;
            SetUpcommingAction(QueuedAction.None);
            actions.InvokeMethod(CanRelease, 0.4f);
            actions.SetAnimation(archetype.block, 0.1f);
            actions.player.StartParryTimer();

        }


        public override void BlockRelease()
        {
            if (!actionDone)
            {
                SetUpcommingAction(QueuedAction.Block);
            }
            else
            {
                EndBlock();
            }
        }
        private void CanRelease()
        {
            if(upcommingAction == QueuedAction.Block)
            {
                EndBlock();
            }
            else
            {
                actionDone = true;
            }
        }

        public override void Parry()
        {
            actions.StopMethod();
            actions.SwitchState(actions.parryState);
        }

        public override void PerfectParry()
        {
            actions.StopMethod();
            actions.SwitchState(actions.perfectParryState);
        }

        private void EndBlock()
        {
            actions.StopMethod();
            actions.SwitchState(actions.idleState);
        }
    }

}
