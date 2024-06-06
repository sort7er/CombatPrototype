using UnityEngine;

namespace EnemyAI
{
    public class BlockState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

            Anim blockAnim = currentWeapon.archetype.enemyBlock;
            enemy.SetAnimation(blockAnim);

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
        }

        public override void Hit(Weapon attackingWeapon, Vector3 hitPoint)
        {
            enemy.SetHitPoint(hitPoint);
            enemyBehaviour.BlockHit();
        }

        public void BlockingDone()
        {
            LeaveStateAndDo(standbyState, LeaveBlocking);
        }
        public void LeaveBlocking()
        {
            enemy.SetAnimationWithInt(enemy.blockDoneState, 0.25f);
        }
    }
}
