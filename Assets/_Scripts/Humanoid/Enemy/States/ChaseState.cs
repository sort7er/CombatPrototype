using UnityEngine;

namespace EnemyAI
{
    public class ChaseState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            enemy.EnableMovement();
            enemyAnimator.SetWalking(true);
        }

        public override void Update()
        {
            base.Update();

            float dist = Vector3.Distance(player.Position(), enemy.Position());

            if (dist < enemy.playerDistance)
            {
                LeaveStateAndDo(attackState,() => enemyAnimator.SetWalking(false));
            }
            else
            {
                enemy.SpeedByDist(dist);
                enemy.SetTarget(player.Position(), player.Position());
            }
        }
        public override void Hit()
        {
            LeaveState(hitState);  
        }
        public override void Stunned()
        {
            LeaveStateAndDo(stunnedState, () => enemyAnimator.SetWalking(false));
        }
    }
}

