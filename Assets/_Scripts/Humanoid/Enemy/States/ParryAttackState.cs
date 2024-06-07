using UnityEngine;

namespace EnemyAI
{
    public class ParryAttackState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

            Anim parryAttack = currentWeapon.archetype.enemyParryAttack;

            enemy.SetAnimation(parryAttack);
            StartRotate();
            enemy.InvokeMethod(StopRotate, 0.25f);
            enemy.InvokeMethod(EndAttack, parryAttack.duration);

        }
        private void EndAttack()
        {
            LeaveState(standbyState);
        }
        public override void Hit()
        {
            LeaveStateAndDo(hitState, () => LeaveParryAttack());
        }
        public override void Staggered()
        {
            LeaveStateAndDo(staggeredState, () => LeaveParryAttack());
        }
        public override void Stunned()
        {
            LeaveStateAndDo(stunnedState, () => LeaveParryAttack());
        }
        private void LeaveParryAttack(float transition = 0)
        {
            enemy.SetAnimationWithInt(enemy.attackDoneState, transition);
        }
    }


}