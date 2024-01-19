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
        }

        public override void Staggered(Enemy enemy)
        {

        }

        public override void UpdateState(Enemy enemy)
        {

        }
        public void StaggerDone()
        {
            enemy.SwitchState(enemy.chaseState);
        }
    }

}
