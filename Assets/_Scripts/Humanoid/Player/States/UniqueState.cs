using EnemyAI;
using System.Collections.Generic;

namespace PlayerSM
{
    public class UniqueState : PlayerState
    {
        private List<Enemy> enemyList;
        public override void Enter(Player player)
        {
            base.Enter(player);
            ResetValuesAttack();
            player.SetAnimation(archetype.unique);
            player.canUseUnique = false;

            //This is for the UI
            player.unique.Using();

            //List of enemies
            GetEnemies();

            player.InvokeMethod(EndAttack, player.currentAnimation.duration);
        }

        private void GetEnemies()
        {
            ClearList();
            enemyList = player.targetAssistance.CheckForEnemies(player.currentWeapon.uniqueAbility);


            if (enemyList.Count > 0)
            {
                player.currentWeapon.uniqueAbility.ExecuteAbility(player, enemyList);
            }
            else
            {
                player.currentWeapon.uniqueAbility.ExecuteAbilityNoTarget(player);
            }
        }
        private void ClearList()
        {
            if (enemyList == null)
            {
                enemyList = new();
            }
            enemyList.Clear();
        }

        #region Queuing methods 
        public override void Attack()
        {
            DoOrQueueAction(() => LeaveState(attackState));
        }
        public override void Block()
        {
            DoOrQueueAction(() => LeaveState(blockState));
        }
        #endregion

        private void EndAttack()
        {
            LeaveState(idleState);
        }
    }
}

