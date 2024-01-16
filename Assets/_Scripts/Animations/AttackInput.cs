using System;
using Attacks;
using HealthRelated;

[Serializable]
public class AttackInput : AnimationInput
{
    public DamageType damageType;

    public ActiveWeapon activeWeapon;

    public int damage;

    public float queuePoint;
}
