using Attacks;
using UnityEngine;

public class AttackEnemy : Attack
{
    public float exitTime { get; private set; }
    public float exitTimeSeconds { get; private set; }
    public float transitionDuration { get; private set; }

    public AttackEnemy(AnimationClip clip, int damage, int postureDamage, Wield wield, HitType hitType, AnimationCurve animationCurve, AttackCoord[] attackCoordsMain, AttackCoord[] attackCoordsSecondary, float exitTime, float transitionDuration) : base(clip, damage, postureDamage, wield, hitType, animationCurve, attackCoordsMain, attackCoordsSecondary)
    {
        this.damage = damage;
        this.postureDamage = postureDamage;
        currentWield = wield;
        this.hitType = hitType;
        this.animationCurve = animationCurve;

        this.exitTime = exitTime;
        this.transitionDuration = transitionDuration;
        exitTimeSeconds = Tools.Remap(exitTime, 0, 1, 0, duration);


        this.attackCoordsMain = attackCoordsMain;
        this.attackCoordsSecondary = attackCoordsSecondary;
    }
}
