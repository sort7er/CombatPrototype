using System;
using Attacks;
using UnityEngine;

[Serializable]
public class AttackInput : AnimationInput
{
    public int damage;
    public int postureDamage;
    public Wield activeWield;
    public HitType hitType;
    public AnimationCurve animationCurve;
    public AttackCoord[] attackCoordsMain;
    public AttackCoord[] attackCoordsSecondary;
}
