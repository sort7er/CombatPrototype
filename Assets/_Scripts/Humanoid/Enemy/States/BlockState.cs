using Stats;
using UnityEngine;

namespace EnemyAI
{
    public class BlockState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

            Attack blockAnim = currentWeapon.archetype.enemyBlock;
            enemy.SetAttack(blockAnim);

            StartRotate();
            enemy.InvokeMethod(StopRotate, 0.25f);
            enemy.InvokeMethod(BlockingDone, 3f);
        }

        public override void Update()
        {
            base.Update();
            
            if (!enemy.InsideAttackFOV())
            {
                BlockingDone();
            }

            //This updates the rotation of the animation
            enemy.SetLookAtAndForward(enemy.target.Position(), enemy.InFront());
        }

        public override void Hit()
        {
            enemyBehaviour.BlockHit();
        }
        public override void Stunned()
        {
            LeaveStateAndDo(stunnedState, LeaveBlocking);
        }
        public override void Staggered()
        {
            LeaveStateAndDo(staggeredState, LeaveBlocking);
        }
        public override void Takedown()
        {
            LeaveStateAndDo(takedownState, LeaveBlocking);
        }
        public void BlockingDone()
        {
            LeaveStateAndDo(standbyState, LeaveBlocking);
        }
        public void LeaveBlocking()
        {
            enemy.SetAnimationWithInt(enemy.attackDoneState, 0.25f);
        }

    }
}
