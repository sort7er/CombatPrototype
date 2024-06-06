using Stats;
using UnityEngine;

namespace EnemyAI
{
    public class PerfectParryState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            enemyBehaviour.PerfectParryEnter();
            DoPerfectParry();
        }

        private void DoPerfectParry()
        {
            Anim perfectParryAnim = currentWeapon.archetype.enemyPerfectParry;

            StartRotate();
            enemy.InvokeMethod(StopRotate, 0.25f);


            ReturnPostureDamage(ParryType.PerfectParry);


            EffectManager.instance.PerfectParry(enemy.hitPoint);
            enemy.SetAnimation(perfectParryAnim);
            enemy.InvokeMethod(EndPerfectParry, perfectParryAnim.duration);
        }

        public void DoPefectParryAttack()
        {
            LeaveState(parryAttackState);
        }

        private void EndPerfectParry()
        {
            enemy.SetAnimationWithInt(enemy.attackDoneState, 0.25f);
            LeaveState(standbyState);
        }
    }
}