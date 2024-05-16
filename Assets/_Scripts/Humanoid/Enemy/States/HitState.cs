namespace EnemyAI
{
    public class HitState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            enemy.enemyAnimator.SetWalking(false);

            Anim hitAnim = currentWeapon.archetype.enemyHit;

            enemy.SetAnimation(hitAnim, 0.25f);
            enemy.InvokeFunction(HitDone, hitAnim.duration);
        }


        private void HitDone()
        {
            enemy.SwitchState(enemy.chaseState);
        }

        public override void Stunned()
        {
            enemy.StopFunction();
            enemy.SwitchState(enemy.stunnedState);
        }
    }
}
