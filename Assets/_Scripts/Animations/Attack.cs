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
}

public class Attack : Anim
{


    public int damage;
    public float queuePoint;
    public DamageType damageType;
    public ActiveWeapon activeWeapon;
    

    public Attack(AnimationClip clip, int dmg, float queuePoint, DamageType damageType, ActiveWeapon activeWeapon) : base(clip)
    {
        damage = dmg;
        this.queuePoint = queuePoint;
        this.damageType = damageType;
        this.activeWeapon = activeWeapon;
    }
}
