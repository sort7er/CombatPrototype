using PlayerSM;
using UnityEngine;

namespace PlayerSM
{
    public class AttackState : PlayerState
    {
        private int currentAttack;

        public override void Enter(Player player)
        {
            base.Enter(player);
            currentAttack = 0;
            StartAttack();
        }
        #region Queuing methods 
        public override void Attack()
        {
            DoOrQueueAction(StartAttack);
        }
        public override void Block()
        {
            DoOrQueueAction(() => LeaveState(blockState));     
        }
        #endregion

        private void StartAttack()
        {
            ResetValuesAttack();
            player.AttackEvent();
            player.SetAnimation(archetype.attacks[currentAttack], 0.05f);
            player.StopMethod();
            player.InvokeMethod(EndAttack, player.currentAnimation.duration);
            UpdateCurrentAttack();
        }


        private void EndAttack()
        {
            LeaveState(idleState);
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
        public override void Hit()
        {
            LeaveState(hitState);
        }
        public override void Stunned()
        {
            LeaveState(stunnedState);
        }
        public override void Staggered()
        {
            LeaveState(staggeredState);
        }

    }
}

