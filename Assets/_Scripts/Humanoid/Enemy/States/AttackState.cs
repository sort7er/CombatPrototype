using UnityEngine;

namespace EnemyAI
{
    public class AttackState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            enemy.DisableMovement();
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
                Debug.Log(enemy.currentWeapon.archetype.enemyAttacks[0].animationClip.name);
            }
        }
    }
}