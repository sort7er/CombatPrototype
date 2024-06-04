using UnityEngine;

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

            enemy.SetAnimation(parryAnim);
            enemy.InvokeFunction(EndParry, parryAnim.duration);
        }

        private void EndParry()
        {
            LeaveState(standbyState);
        }

        public override void Hit()
        {
            enemy.StopFunction();
            DoParry();
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

