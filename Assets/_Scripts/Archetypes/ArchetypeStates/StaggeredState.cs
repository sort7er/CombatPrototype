using UnityEngine;

namespace ArchetypeStates
{
    public class StaggeredState : ArchetypeState
    {
        private ArchetypeAnimator archetypeAnimator;
        public override void EnterState(ArchetypeAnimator archetype)
        {
            archetypeAnimator = archetype;
            archetype.CrossFade(archetype.staggered, 0f);
            archetype.InvokeFunction(StaggerDone, archetype.staggered.duration);
        }

        private void StaggerDone()
        {
            archetypeAnimator.SwitchState(archetypeAnimator.idleState);
        }
        #region Unused
        public override void Fire(ArchetypeAnimator archetype)
        {

        }
        public override void HeavyFire(ArchetypeAnimator archetype)
        {

        }
        public override void UniqueFire(ArchetypeAnimator archetype)
        {

        }
        public override void Block(ArchetypeAnimator archetype)
        {

        }

        public override void Parry(ArchetypeAnimator archetype)
        {

        }
        public override void Staggered(ArchetypeAnimator archetype)
        {

        }
        #endregion
    }
}
