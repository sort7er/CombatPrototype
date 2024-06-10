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

            float dist = Vector3.Distance(enemy.target.Position(), enemy.Position());

            if (dist < enemy.minTargetDistance)
            {
                LeaveStateAndDo(standbyState, ChaseDone);
            }
            else
            {
                enemy.SpeedByDist(dist);
                enemy.MoveToTarget(enemy.target.Position(), enemy.target.Position());
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
        public override void Takedown()
        {
            LeaveStateAndDo(takedownState, ChaseDone);
        }
        private void ChaseDone()
        {
            enemy.DisableMovement();
            enemyAnimator.SetWalking(false);
        }
    }
}

