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
            
            StartRotate();
            enemy.InvokeMethod(StopRotate, 0.25f);


            EffectManager.instance.Parry(enemy.hitPoint);
            enemy.SetAnimation(parryAnim);
            enemy.InvokeMethod(EndParry, parryAnim.duration);
        }

        private void EndParry()
        {
            LeaveState(standbyState);
        }

        public override void Hit()
        {
            enemy.enemyBehaviour.ParryHit();
        }

        //This is so it can be called from enemy behaviour
        public void SwitchToHit()
        {
            base.Hit();
            LeaveState(hitState);
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

