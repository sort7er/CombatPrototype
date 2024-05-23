using UnityEngine;

namespace EnemyAI
{
    public class StunnedState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            Anim stunnedAnim = currentWeapon.archetype.enemyStunned;

            enemy.SetAnimation(stunnedAnim);
            enemy.InvokeFunction(StunnedDone, enemy.stunnedDuration);

            enemyAnimator.SetAnimatorBool("Stunned", true);

        }
        private void StunnedDone()
        {
            LeaveStateAndDo(chaseState, () => enemyAnimator.SetAnimatorBool("Stunned", false));
        }
    }
}

