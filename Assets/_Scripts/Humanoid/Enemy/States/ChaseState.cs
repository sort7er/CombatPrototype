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

            float dist = Vector3.Distance(player.Position(), enemy.Position());

            if (dist < enemy.playerDistance)
            {
                enemy.enemyAnimator.SetWalking(false);
                enemy.SwitchState(enemy.attackState);
            }
            else
            {
                enemy.SpeedByDist(dist);
                enemy.SetTarget(player.Position(), player.Position());
            }
        }
        public override void Hit()
        {
            enemy.SwitchState(enemy.hitState);
        }
        public override void Stunned()
        {
            enemy.enemyAnimator.SetWalking(false);
            enemy.SwitchState(enemy.stunnedState);
        }
    }
}

