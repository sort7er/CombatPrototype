using Actions;

namespace Actions
{
    public class AttackState : ActionState
    {
        private int currentAttack;

        public override void Enter(PlayerActions actions)
        {
            base.Enter(actions);
            currentAttack = 0;
            StartAttack();
        }
        #region Queuing methods 
        public override void Attack()
        {
            QueueAttack(StartAttack);
        }
        public override void Block()
        {
            QueueBlock(() => LeaveState(actions.blockState));
        }
        public override void ActionDone()
        {
            QueueActionDone(StartAttack, () => LeaveState(actions.blockState));
        }
        #endregion

        private void StartAttack()
        {
            ResetValuesAttack();
            actions.SetAnimation(archetype.attacks[currentAttack], 0.05f);
            actions.StopMethod();
            actions.InvokeMethod(EndAttack, actions.currentAnimation.duration);
            UpdateCurrentAttack();
        }


        private void EndAttack()
        {
            LeaveState(actions.idleState);
        }

        private void UpdateCurrentAttack()
        {
            if (currentAttack < archetype.attacks.Length - 1)
            {
                currentAttack++;
            }
            else
            {
                currentAttack = 0;
            }
        }

    }
}

