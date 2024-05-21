using UnityEngine;

namespace EnemyAI
{
    public class ParryState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

        }

        public override void Update()
        {
            base.Update();
        }

        public override void Stunned()
        {
            enemy.SwitchState(enemy.stunnedState);
        }
    }
}

