using UnityEngine;

namespace EnemyAI
{
    public class ParryAttackState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

            Anim parryAttack = currentWeapon.archetype.enemyParryAttack;

            enemy.SetAnimation(parryAttack);
            StartRotate();
            enemy.InvokeMethod(StopRotate, 0.25f);
            enemy.InvokeMethod(EndAttack, parryAttack.duration);

        }
        private void EndAttack()
        {
            LeaveState(standbyState);
        }
    }


}