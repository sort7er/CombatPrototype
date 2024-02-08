using System;
using Attacks;
[Serializable]
public class AttackInput : AnimationInput
{
    public ActiveWeapon activeWeapon;

    public AttributeAffected attributeAffected;

    public int damage;

    public int postureDamage;

    public float queuePoint;
}
