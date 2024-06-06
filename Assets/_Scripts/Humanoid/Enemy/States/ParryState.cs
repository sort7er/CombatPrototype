using UnityEngine;
using Stats;

namespace EnemyAI
{
    public class ParryState : EnemyState
    {
        private int currentParry;

        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            DoParry();
        }

        private void DoParry()
        {
            Anim parryAnim = currentWeapon.archetype.enemyParrys[GetCurrentParry()];

            StartRotate();
            enemy.InvokeMethod(StopRotate, 0.25f);


            EffectManager.instance.Parry(enemy.hitPoint);
            enemy.SetAnimation(parryAnim);
            enemy.InvokeMethod(EndParry, parryAnim.duration);

            ReturnPostureDamage(ParryType.PerfectParry);
        }

        private void EndParry()
        {
            LeaveState(standbyState);
        }

        public override void Hit()
        {
            enemy.enemyBehaviour.ParryHit();
        }
        public override void Stunned()
        {
            LeaveState(stunnedState);
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

