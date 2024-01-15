using UnityEngine;

namespace EnemyStates
{
    public class StaggeredState : EnemyState
    {
        private Enemy enemy;
        private ArchetypeAnimator archetypeAnimator;
        public override void EnterState(Enemy enemy)
        {
            this.enemy = enemy;
            enemy.enemyAnim.SetTrigger("Staggered");
            enemy.DisableMovement();
            
            archetypeAnimator = enemy.currentArchetype.archetypeAnimator;
            archetypeAnimator.Staggered();
            enemy.InvokeFunction(StaggerDone, archetypeAnimator.staggered.duration);


        }

        public override void Staggered(Enemy enemy)
        {

        }

        public override void UpdateState(Enemy enemy)
        {

        }
        private void StaggerDone()
        {
            enemy.SwitchState(enemy.chaseState);
        }
    }

}
