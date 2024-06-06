using UnityEngine;

namespace EnemyAI
{
    public class IdleState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            enemyAnimator.SetWalking(false);
        }

        public override void Update()
        {
            base.Update();

            float dist = Vector3.Distance(player.Position(), enemy.Position());

            if (dist < enemy.DistanceBeforeChase)
            {
                LeaveState(chaseState);
            }
        }
        public override void Hit()
        {
            base.Hit();
            LeaveState(hitState);
        }
        public override void Stunned()
        {
            LeaveState(stunnedState);
        }
    }
}
