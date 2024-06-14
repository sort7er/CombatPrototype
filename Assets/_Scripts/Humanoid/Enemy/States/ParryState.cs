using UnityEngine;
using Stats;

namespace EnemyAI
{
    public class ParryState : EnemyState
    {
        public int currentParry { get; private set; }

        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            enemyBehaviour.ParryEnter();
        }

        public void DoParry()
        {
            Attack parryAnim = currentWeapon.archetype.enemyParrys[GetCurrentParry()];

            StartRotate();
            enemy.InvokeMethod(StopRotate, 0.25f);

            enemy.SetAttack(parryAnim);
            ReturnPostureDamage(ParryType.Parry);
        }

        public void EndParry()
        {
            enemyBehaviour.ParryEnd();
        }
        public override void Hit()
        {
            enemyBehaviour.ParryHit();
        }
        public override void TargetAttacking()
        {
            enemyBehaviour.ParryTargetAttack();
        }
        public override void Stunned()
        {
            LeaveState(stunnedState);
        }
        public override void Takedown()
        {
            LeaveState(takedownState);
        }
        public int GetCurrentParry()
        {
            if (currentParry == 0)
            {
                currentParry++;
            }
            else
            {
                currentParry = 0;
            }
            return currentParry;
        }
    }


}

