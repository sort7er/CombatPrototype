using UnityEngine;

namespace ArchetypeStates
{
    public class BlockingState : ArchetypeState
    {
        private ArchetypeAnimator archetypeAnimator;

        private float parryWindow = 0.17f;
        private bool blocking;

        public override void EnterState(ArchetypeAnimator archetype)
        {
            archetypeAnimator = archetype;
            blocking = false;
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
        #endregion
        public override void Parry(ArchetypeAnimator archetype)
        {
            if (!blocking)
            {
                archetypeAnimator.SwitchState(archetypeAnimator.parryState);
            }
            else
            {
                archetype.SwitchState(archetype.idleState);
            }
        }

        private void Blocking()
        {
            blocking = true;
        }
    }
}
