using UnityEngine;

namespace EnemyAI
{
    public class StaggeredState : EnemyState
    {
        private Vector3 stepbackTarget;

        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

            stepbackTarget = enemy.Position() - enemy.transform.forward * 1f;

            Anim staggeredAnim = currentWeapon.archetype.enemyStaggered;

            enemy.SetAnimation(staggeredAnim, 0.25f);
            enemy.InvokeFunction(StaggerDone, staggeredAnim.duration);

        }

        public override void Update()
        {
            enemy.SetTarget(stepbackTarget, player.Position());
        }

        private void StaggerDone()
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

