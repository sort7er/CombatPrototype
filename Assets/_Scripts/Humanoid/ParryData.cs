using Stats;
using UnityEngine;

public class ParryData
{
    public int postureDamage;
    public ParryType parryType;
    public Vector3 hitPoint;
    public Vector3 direction;
    public Humanoid defender;
    public Weapon defendingWeapon;
    public Weapon attackingWeapon;

    public ParryData(Humanoid defender)
    {
        this.defender = defender;
    }
}
