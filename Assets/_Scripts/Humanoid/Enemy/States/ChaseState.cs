using UnityEngine;

namespace EnemyAI
{
    public class ChaseState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            Debug.Log("Hola");
        }

        public override void Update()
        {
            base.Update();

            if(Vector3.Distance(player.Position(), enemy.Position()) < enemy.playerDistance)
            {
                enemy.SwitchState(enemy.attackState);
            }
            else
            {
                enemy.SetTarget(player.Position());
            }
        }
    }
}

