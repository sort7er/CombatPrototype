using EnemyAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSM
{
    public class RangedState : PlayerState
    {
        private List<Enemy> enemyList;
        public override void Enter(Player player)
        {
            base.Enter(player);
            Attack ranged = weapon.abilitySet.ranged;
            player.SetAttack(ranged);

            GetEnemies();
            player.InvokeMethod(EndRanged, ranged.duration);
        }
        private void GetEnemies()
        {
            ClearList();
            enemyList = player.targetAssistance.CheckForEnemies(weapon.abilitySet.rangedAbilty);


            if (enemyList.Count > 0)
            {
                weapon.abilitySet.rangedAbilty.ExecuteAbility(player, enemyList);
            }
            else
            {
                weapon.abilitySet.rangedAbilty.ExecuteAbilityNoTarget(player);
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

        public override void LateUpdate()
        {
            weapon.abilitySet.rangedAbilty.LateUpdateAbility();
        }

        private void EndRanged()
        {
            Debug.Log("Ranged left");
            LeaveState(idleState);
        }

    }
}
