using EnemyAI;
using System.Collections.Generic;

namespace Actions
{
    public class UniqueState : ActionState
    {
        private List<Enemy> enemyList;
        public override void Enter(PlayerActions actions)
        {
            base.Enter(actions);
            ResetValuesAttack();
            actions.SetAnimation(archetype.unique);
            actions.canUseUnique = false;

            //This is for the UI
            actions.unique.Using();

            //List of enemies
            GetEnemies();

            actions.InvokeMethod(EndAttack, actions.currentAnimation.duration);
        }

        private void GetEnemies()
        {
            ClearList();
            enemyList = actions.player.targetAssistance.CheckForEnemies(actions.currentWeapon.uniqueAbility);


            if (enemyList.Count > 0)
            {
                actions.currentWeapon.uniqueAbility.ExecuteAbility(actions.player, enemyList);
            }
            else
            {
                actions.currentWeapon.uniqueAbility.ExecuteAbilityNoTarget(actions.player);
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
            QueueAttack(() => LeaveState(actions.attackState));
        }
        public override void Block()
        {
            QueueBlock(() => LeaveState(actions.blockState));
        }
        public override void ActionDone()
        {
            QueueActionDone(() => LeaveState(actions.attackState), () => LeaveState(actions.blockState));
        }
        #endregion

        private void EndAttack()
        {
            LeaveState(actions.idleState);
        }
    }
}

