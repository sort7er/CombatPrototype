using UnityEngine;

namespace ArchetypeStates
{
    public class ParryState : ArchetypeState
    {
        private ArchetypeAnimator archetypeAnimator;
        private int currentParry = 0;
        private bool cannotParry;
        public override void EnterState(ArchetypeAnimator archetype)
        {
            archetypeAnimator = archetype;
            DoParry(archetype);
            
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
            if (cannotParry)
            {
                archetype.StopFunction();
                DoParry(archetype);
            }
        }

        private void DoParry(ArchetypeAnimator archetype)
        {
            archetype.IsParrying(true);
            cannotParry = true;
            archetype.CrossFade(archetype.parry[currentParry], 0.1f);
            archetype.InvokeFunction(ParryWindowDone, archetype.Remap(archetype.parry[currentParry].queuePoint));
            archetype.InvokeFunction(CanParry, archetype.Remap(10));
            archetype.InvokeFunction(EndParry, archetype.parry[currentParry].duration);
            UpdateParry();
        }
        private void ParryWindowDone()
        {
            archetypeAnimator.IsParrying(false);
        }
        private void CanParry()
        {
            cannotParry = false;
        }
        private void EndParry()
        {
            archetypeAnimator.IsParrying(false);
            archetypeAnimator.SwitchState(archetypeAnimator.idleState);
        }

        private void UpdateParry()
        {
            if (currentParry == 0)
            {
                currentParry = 1;
            }
            else
            {
                currentParry = 0;
            }
        }
    }
}