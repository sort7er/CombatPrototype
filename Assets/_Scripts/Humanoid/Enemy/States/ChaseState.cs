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

            float dist = Vector3.Distance(player.Position(), enemy.Position());

            if (dist < enemy.playerDistance)
            {
                LeaveStateAndDo(standbyState, ChaseDone);
            }
            else
            {
                enemy.SpeedByDist(dist);
                enemy.MoveToTarget(player.Position(), player.Position());
            }
        }
        public override void Hit()
        {
            LeaveStateAndDo(hitState, ChaseDone);  
        }
        public override void Stunned()
        {
            LeaveStateAndDo(stunnedState, ChaseDone);
        }
        private void ChaseDone()
        {
            enemy.DisableMovement();
            enemyAnimator.SetWalking(false);
        }
    }
}

