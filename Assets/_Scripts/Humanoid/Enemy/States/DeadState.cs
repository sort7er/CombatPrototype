using UnityEngine;

namespace EnemyAI
{
    public class DeadState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            enemy.StopMethod();
            enemy.enemyDissolve.Dissolve();
            enemy.currentWeapon.gameObject.SetActive(false);
        }
    }
}

