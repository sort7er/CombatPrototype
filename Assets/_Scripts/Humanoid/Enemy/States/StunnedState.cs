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
            enemy.InvokeMethod(StunnedDone, enemy.stunnedDuration);

        }
        public override void Hit()
        {
            TakeDamage();
        }
        private void StunnedDone()
        {
            LeaveStateAndDo(chaseState, () => enemy.SetAnimationWithInt(enemy.stunnedDoneState,0.25f));
        }
    }
}

