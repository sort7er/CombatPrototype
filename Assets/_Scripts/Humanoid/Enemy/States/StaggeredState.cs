using UnityEngine;

namespace EnemyAI
{
    public class StaggeredState : EnemyState
    {
        private Vector3 stepbackTarget;
        private Vector3 storedPlayerPos;

        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

            stepbackTarget = enemy.Position() - enemy.transform.forward * 1f;
            storedPlayerPos = player.Position();

            Anim staggeredAnim = currentWeapon.archetype.enemyStaggered;

            enemy.SetAnimation(staggeredAnim, 0.25f);
            enemy.InvokeMethod(StaggerDone, staggeredAnim.duration);

        }

        public override void Update()
        {
            enemy.MoveToTarget(stepbackTarget, storedPlayerPos);
        }

        private void StaggerDone()
        {
            LeaveState(chaseState);
        }
        public override void Hit()
        {
            TakeDamage();
        }
        public override void Stunned()
        {
            LeaveState(stunnedState);
        }
    }
}

