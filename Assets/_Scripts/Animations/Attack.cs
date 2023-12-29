using UnityEngine;
using HealthRelated;

public class Attack : Anim
{


    public int damage;
    public float queuePoint;
    public DamageType damageType;

    public Attack(AnimationClip clip, int dmg, float queuePoint, DamageType damageType) : base(clip)
    {
        damage = dmg;
        this.queuePoint = queuePoint;
        this.damageType = damageType;
    }
}
