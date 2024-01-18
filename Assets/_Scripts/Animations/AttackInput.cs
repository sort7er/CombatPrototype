using System;
using Attacks;
using HealthRelated;

[Serializable]
public class AttackInput : AnimationInput
{
    public DamageType damageType;

    public ActiveWeapon activeWeapon;

    public AttributeAffected attributeAffected;

    public int damage;

    public int postureDamage;

    public float queuePoint;
}
