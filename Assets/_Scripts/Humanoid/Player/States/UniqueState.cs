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

            Attack attack = archetype.unique;

            player.SetAttack(attack);
            player.CannotUseUnique();

            //This is for the UI
            player.unique.Using();

            //List of enemies
            GetEnemies();

            player.InvokeMethod(EndAttack, attack.duration);
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
            DoOrQueueAttack(() => LeaveStateAndDo(attackState, LeaveUnique));
        }
        public override void Block()
        {
            DoOrQueueBlock(() => LeaveStateAndDo(blockState, LeaveUnique));
        }
        #endregion

        private void EndAttack()
        {
            LeaveStateAndDo(idleState, LeaveUnique);
        }
        public override void Stunned()
        {
            LeaveStateAndDo(stunnedState, LeaveUnique);
        }
        public override void Staggered()
        {
            LeaveStateAndDo(staggeredState, LeaveUnique);
        }

        private void LeaveUnique()
        {
            player.unique.Loading();
        }
    }
}

