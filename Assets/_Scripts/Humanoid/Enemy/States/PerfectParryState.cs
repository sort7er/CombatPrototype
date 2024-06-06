using UnityEngine;

namespace EnemyAI
{
    public class PerfectParryState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            DoPerfectParry();
        }

        private void DoPerfectParry()
        {
            Anim perfectParryAnim = currentWeapon.archetype.enemyPerfectParry;

            Debug.Log("Doing Perfect Parry");

            StartRotate();
            enemy.InvokeMethod(StopRotate, 0.25f);


            EffectManager.instance.PerfectParry(enemy.hitPoint);
            enemy.SetAnimation(perfectParryAnim);
            enemy.InvokeMethod(EndPerfectParry, perfectParryAnim.duration);
        }

        private void EndPerfectParry()
        {
            LeaveState(standbyState);
        }
    }
}