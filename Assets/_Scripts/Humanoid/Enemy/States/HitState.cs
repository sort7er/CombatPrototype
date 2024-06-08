using UnityEngine;

namespace EnemyAI
{
    public class HitState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            GetHit();

        }
        private void HitDone()
        {
            LeaveState(chaseState);
        }
        public override void Hit()
        {
            enemyBehaviour.HitHit();
        }

        public override void Staggered()
        {
            LeaveState(staggeredState);
        }

        public override void Stunned()
        {
            LeaveState(stunnedState);
        }
        public void GetHit()
        {
            Anim hitAnim = currentWeapon.archetype.enemyHit;

            enemy.StopMethod();

            StartRotate();
            enemy.InvokeMethod(StopRotate, 0.25f);

            enemy.SetAnimation(hitAnim, 0f);
            enemy.InvokeMethod(HitDone, hitAnim.duration);
            TakeDamage();
        }
    }
}
