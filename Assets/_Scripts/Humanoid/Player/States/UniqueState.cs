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

        public override void Attack()
        {
            if (actionDone)
            {
                LeaveState(actions.attackState);
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
                LeaveState(actions.blockState);
            }
            else if (CheckUpcommingAction())
            {
                SetUpcommingAction(QueuedAction.Block);
            }
        }

        public override void ActionDone()
        {
            actions.StartUniqueCooldown();

            if (upcommingAction == QueuedAction.Attack)
            {
                LeaveState(actions.attackState);
            }
            else if (upcommingAction == QueuedAction.Block)
            {
                LeaveState(actions.blockState);
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

