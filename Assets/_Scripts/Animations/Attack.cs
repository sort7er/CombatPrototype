using UnityEngine;
using HealthRelated;
using Attacks;

namespace Attacks
{
    public enum ActiveWeapon
    {
        right,
        left,
        both
    }
    public enum AttributeAffected
    {
        normal,
        onlyPosture
    }
}

public class Attack : Anim
{


    public int damage;
    public int postureDamage;
    public float queuePoint;
    public DamageType damageType;
    public ActiveWeapon activeWeapon;
    public AttributeAffected attributeAffected;


    public Attack(AnimationClip clip, int dmg, int postureDmg, float queuePoint, DamageType damageType, ActiveWeapon activeWeapon, AttributeAffected attributeAffected) : base(clip)
    {
        damage = dmg;
        postureDamage = postureDmg;
        this.queuePoint = queuePoint;
        this.damageType = damageType;
        this.activeWeapon = activeWeapon;
        this.attributeAffected = attributeAffected;
    }
}
