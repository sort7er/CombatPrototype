using UnityEngine;

namespace EnemyAI
{
    public class HitState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

            HitAnimation();
        }
        private void HitDone()
        {
            LeaveState(chaseState);
        }
        public override void Hit(Weapon attackingWeapon, Vector3 hitPoint)
        {
            base.Hit(attackingWeapon, hitPoint);
            enemy.StopFunction();
            HitAnimation();
        }

        public override void Staggered()
        {
            LeaveState(staggeredState);
        }

        public override void Stunned()
        {
            LeaveState(stunnedState);
        }
        private void HitAnimation()
        {
            Anim hitAnim = currentWeapon.archetype.enemyHit;

            enemy.SetAnimation(hitAnim, 0f);
            enemy.InvokeFunction(HitDone, hitAnim.duration);
        }
    }
}
