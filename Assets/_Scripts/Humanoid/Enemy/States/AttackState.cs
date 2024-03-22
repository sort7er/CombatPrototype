using UnityEngine;

namespace EnemyAI
{
    public class AttackState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
        }

        public override void Update()
        {
            base.Update();

            if (Vector3.Distance(player.Position(), enemy.Position()) > enemy.playerDistance)
            {
                enemy.SwitchState(enemy.chaseState);
            }
            else
            {
                Debug.Log("Arrg!");
            }
        }
    }
}