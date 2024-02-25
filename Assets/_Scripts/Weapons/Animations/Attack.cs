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
    public AttackCoord[] attackCoordsMain;
    public AttackCoord[] attackCoordsSecondary;

    public Attack(AnimationClip clip, Wield wield, HitType hitType, AttackCoord[] attackCoordsMain, AttackCoord[] attackCoordsSecondary) : base(clip)
    {
        currentWield = wield;
        this.hitType = hitType;
        this.attackCoordsMain = attackCoordsMain;
        this.attackCoordsSecondary = attackCoordsSecondary;
    }
}
