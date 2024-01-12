using UnityEngine;

namespace EnemyStates
{
    public class ChaseState : EnemyState
    {
        public override void EnterState(Enemy enemy)
        {

        }

        public override void UpdateState(Enemy enemy)
        {
            enemy.LookAtTarget(enemy.currentTarget);
            if (Vector3.Distance(enemy.player.Position(), enemy.Position()) > enemy.playerDistance)
            {
                enemy.MoveTo(enemy.player.Position());
            }
            else
            {
                enemy.SwitchState(enemy.attackState);
            }
        }
    }
}

