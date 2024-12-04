using Stats;
using UnityEngine;

namespace EnemyAI
{
    public class PerfectParryState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
      
            enemy.SetCurrentPerfectParry();
            enemyBehaviour.PerfectParryEnter();
            enemy.SetNextParryState(parryState);
            DoPerfectParry();
        }

        private void DoPerfectParry()
        {
            Attack perfectParryAnim = currentWeapon.archetype.enemyPerfectParrys[enemy.currentPerfectParry];

            StartRotate();
            enemy.InvokeMethod(StopRotate, 0.25f);


            enemy.SetBlock(perfectParryAnim);
            enemy.InvokeMethod(EndPerfectParry, perfectParryAnim.duration);


            ReturnPostureDamage(ParryType.PerfectParry);
        }

        public void DoPefectParryAttack()
        {
            LeaveState(parryAttackState);
        }
        public override void Takedown()
        {
            LeaveState(takedownState);
        }

        private void EndPerfectParry()
        {
            enemy.SetAnimationWithInt(enemy.attackDoneState, 0.25f);
            LeaveState(standbyState);
        }
    }
}