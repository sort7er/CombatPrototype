using UnityEngine;

namespace EnemyAI
{
    public class ChaseState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            enemy.EnableMovement();
            enemy.enemyAnimator.SetWalking(true);
        }

        public override void Update()
        {
            base.Update();

            if(Vector3.Distance(player.Position(), enemy.Position()) < enemy.playerDistance)
            {
                enemy.enemyAnimator.SetWalking(false);
                enemy.SwitchState(enemy.attackState);
            }
            else
            {
                enemy.SetTarget(player.Position());
            }
        }
    }
}

