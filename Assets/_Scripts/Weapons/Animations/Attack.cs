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
    public int damage;
    public int postureDamage;
    public Wield currentWield;
    public HitType hitType;
    public AnimationCurve animationCurve;
    public AttackCoord[] attackCoordsMain;
    public AttackCoord[] attackCoordsSecondary;

    public Attack(AnimationClip clip, int damage, int postureDamage, Wield wield, HitType hitType, AnimationCurve animationCurve, AttackCoord[] attackCoordsMain, AttackCoord[] attackCoordsSecondary) : base(clip)
    {
        this.damage = damage;
        this.postureDamage = postureDamage;
        currentWield = wield;
        this.animationCurve = animationCurve;
        this.hitType = hitType;
        this.attackCoordsMain = attackCoordsMain;
        this.attackCoordsSecondary = attackCoordsSecondary;
    }
}
