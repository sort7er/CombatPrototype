using UnityEngine;

namespace EnemyAI
{
    public class TakedownState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            enemyBehaviour.TakedownEnter();
        }

        public override void Hit()
        {
            TakeDamage();
        }

    }
}