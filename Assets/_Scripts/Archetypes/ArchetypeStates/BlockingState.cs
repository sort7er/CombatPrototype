using UnityEngine;

namespace ArchetypeStates
{
    public class BlockingState : ArchetypeState
    {
        private ArchetypeAnimator archetypeAnimator;

        private float parryWindow = 0.17f;

        public override void EnterState(ArchetypeAnimator archetype)
        {
            archetypeAnimator = archetype;
            archetypeAnimator.IsBlocking(false);
            archetype.CrossFade(archetype.block);
            archetype.InvokeFunction(Blocking, parryWindow);
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
        public override void Staggered(ArchetypeAnimator archetype)
        {

        }
        #endregion
        public override void Parry(ArchetypeAnimator archetype)
        {
            if (!archetypeAnimator.isBlocking)
            {
                archetypeAnimator.StopFunction();
                archetypeAnimator.IsBlocking(false);
                archetypeAnimator.SwitchState(archetypeAnimator.parryState);
            }
            else
            {
                archetypeAnimator.IsBlocking(false);
                archetype.SwitchState(archetype.idleState);
            }
        }

        private void Blocking()
        {
            archetypeAnimator.IsBlocking(true);
        }


    }
}
