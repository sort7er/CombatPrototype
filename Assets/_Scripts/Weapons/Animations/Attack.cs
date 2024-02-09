using UnityEngine;
using Attacks;

namespace Attacks
{
    public enum Wield
    {
        right,
        left,
        both
    }
    public enum HitType
    {
        normal,
        slice,
        crumble
    }
}

public class Attack : Anim
{
    public Wield currentWield;
    public HitType hitType;

    public Attack(AnimationClip clip, Wield wield, HitType hitType) : base(clip)
    {
        this.currentWield = wield;
        this.hitType = hitType;
    }
}
