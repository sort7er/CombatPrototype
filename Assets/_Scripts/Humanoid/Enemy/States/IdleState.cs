using UnityEngine;

namespace EnemyAI
{
    public class IdleState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            enemy.enemyAnimator.SetWalking(false);
        }

        public override void Update()
        {
            base.Update();

            float dist = Vector3.Distance(player.Position(), enemy.Position());

            if (dist < enemy.DistanceBeforeChase)
            {
                enemy.SwitchState(enemy.chaseState);
            }
        }
    }
}
