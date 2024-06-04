using UnityEngine;

namespace EnemyAI
{
    public class ParryState : EnemyState
    {
        private int currentParry;
        private bool rotateTowardsPlayer;

        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            DoParry();
        }

        public override void Update()
        {
            if (rotateTowardsPlayer)
            {
                enemy.RotateToTarget(player.Position(), player.Position());
            }
        }

        private void DoParry()
        {
            Anim parryAnim = currentWeapon.archetype.enemyParrys[GetCurrentParry()];
            rotateTowardsPlayer = true;

            enemy.SetAnimation(parryAnim);
            enemy.InvokeFunction(StopRotate, 0.25f);
            enemy.InvokeFunction(EndParry, parryAnim.duration);
        }
        private void StopRotate()
        {
            rotateTowardsPlayer = false;
        }

        private void EndParry()
        {
            LeaveState(standbyState);
        }

        public override void Hit(Weapon attackingWeapon, Vector3 hitPoint)
        {
            if (enemy.InsideParryFOV())
            {
                enemy.StopFunction();
                DoParry();
            }
            else
            {
                LeaveState(hitState);
            }
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

