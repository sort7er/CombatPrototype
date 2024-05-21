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
            enemy.SwitchState(enemy.chaseState);
        }
        public override void Hit()
        {
            enemy.StopFunction();
            HitAnimation();
        }

        public override void Stunned()
        {
            enemy.StopFunction();
            enemy.SwitchState(enemy.stunnedState);
        }
        private void HitAnimation()
        {
            Anim hitAnim = currentWeapon.archetype.enemyHit;

            enemy.SetAnimation(hitAnim, 0f);
            enemy.InvokeFunction(HitDone, hitAnim.duration);
        }
    }
}
