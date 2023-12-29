using System;
using HealthRelated;

[Serializable]
public class AttackInput : AnimationInput
{
    public DamageType damageType;

    public int damage;

    public float queuePoint;
}
