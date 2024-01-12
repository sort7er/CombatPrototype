using UnityEngine;

namespace ArchetypeStates
{
    public class IdleState : ArchetypeState
    {
        public override void EnterState(ArchetypeAnimator archetype, Animator anim)
        {
            anim.CrossFade(archetype.idleAnim.state, 0.25f);
        }
        public override void Fire(ArchetypeAnimator archetype)
        {
            archetype.SwitchState(archetype.attackState);
        }
        public override void HeavyFire(ArchetypeAnimator archetype)
        {
            archetype.SwitchState(archetype.attackState);
        }
        public override void UniqueFire(ArchetypeAnimator archetype)
        {
            archetype.SwitchState(archetype.uniqueState);
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

