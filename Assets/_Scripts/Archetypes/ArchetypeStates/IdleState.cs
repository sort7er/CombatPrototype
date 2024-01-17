using UnityEngine;

namespace ArchetypeStates
{
    public class IdleState : ArchetypeState
    {
        public override void EnterState(ArchetypeAnimator archetype)
        {
            archetype.CrossFade(archetype.idle);
            archetype.SetEntryAttack(null);
        }
        public override void Fire(ArchetypeAnimator archetype)
        {
            archetype.SetEntryAttack(archetype.light[0]);

            archetype.SwitchState(archetype.attackState);
        }
        public override void HeavyFire(ArchetypeAnimator archetype)
        {
            archetype.SetEntryAttack(archetype.heavy[0]);

            archetype.SwitchState(archetype.attackState);
        }
        public override void UniqueFire(ArchetypeAnimator archetype)
        {
            archetype.SetEntryAttack(archetype.unique);
            archetype.SwitchState(archetype.attackState);
        }
        public override void Block(ArchetypeAnimator archetype)
        {
            archetype.SwitchState(archetype.blockState);
        }

        public override void Parry(ArchetypeAnimator archetype)
        {

        }
        public override void Staggered(ArchetypeAnimator archetype)
        {

        }
    }
}

