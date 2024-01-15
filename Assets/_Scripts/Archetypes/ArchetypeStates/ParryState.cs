using UnityEngine;

namespace ArchetypeStates
{
    public class ParryState : ArchetypeState
    {
        private ArchetypeAnimator archetypeAnimator;
        private int currentParry = 0;
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
        #endregion
        public override void Parry(ArchetypeAnimator archetype)
        {
            if (!archetypeAnimator.isParrying)
            {
                archetype.StopFunction();
                DoParry(archetype);
            }
        }

        private void DoParry(ArchetypeAnimator archetype)
        {
            archetype.IsParrying(true);
            archetype.CrossFade(archetype.parry[currentParry], 0.25f);
            archetype.InvokeFunction(EndParry, archetype.parry[currentParry].duration);
            archetype.InvokeFunction(CanParry, archetype.Remap(15, 0, 60, 0, 1));
            UpdateParry();
        }

        private void CanParry()
        {
            archetypeAnimator.IsParrying(false);
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