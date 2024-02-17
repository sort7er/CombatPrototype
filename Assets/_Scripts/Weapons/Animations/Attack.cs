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
    public AttackCoord[] attackCoords;

    public Attack(AnimationClip clip, Wield wield, HitType hitType, AttackCoord[] attackCoords) : base(clip)
    {
        currentWield = wield;
        this.hitType = hitType;
        this.attackCoords = attackCoords;
    }
}
