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
        public override void Hit()
        {
            enemy.StopMethod();
            HitAnimation();
        }

        public override void Stunned()
        {
            LeaveStateAndDo(stunnedState, () => enemy.StopMethod());
        }
        private void HitAnimation()
        {
            Anim hitAnim = currentWeapon.archetype.enemyHit;

            enemy.SetAnimation(hitAnim, 0f);
            enemy.InvokeFunction(HitDone, hitAnim.duration);
        }
    }
}
