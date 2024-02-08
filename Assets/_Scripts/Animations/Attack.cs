using UnityEngine;
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
    public ActiveWeapon activeWeapon;
    public AttributeAffected attributeAffected;


    public Attack(AnimationClip clip, int dmg, int postureDmg, float queuePoint, ActiveWeapon activeWeapon, AttributeAffected attributeAffected) : base(clip)
    {
        damage = dmg;
        postureDamage = postureDmg;
        this.queuePoint = queuePoint;
        this.activeWeapon = activeWeapon;
        this.attributeAffected = attributeAffected;
    }
}
