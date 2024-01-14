using System;
using UnityEngine;

namespace ArchetypeStates
{
    [Serializable]
    public abstract class ArchetypeState
    {

        public abstract void EnterState(ArchetypeAnimator archetype, Animator anim);
        public abstract void Fire(ArchetypeAnimator archetype);
        public abstract void HeavyFire(ArchetypeAnimator archetype);
        public abstract void UniqueFire(ArchetypeAnimator archetype);
        public abstract void Block(ArchetypeAnimator archetype);
        public abstract void Parry(ArchetypeAnimator archetype);
    }

}

