using UnityEngine;

namespace ArchetypeStates
{
    public class IdleState : ArchetypeState
    {
        public override void EnterState(ArchetypeAnimator archetype, Animator anim)
        {
            archetype.SetAnimation(archetype.idleAnim, 0.25f);
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
            archetype.SwitchState(archetype.parryState);
        }
    }
}

