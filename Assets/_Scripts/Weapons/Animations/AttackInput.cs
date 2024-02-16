using System;
using Attacks;

[Serializable]
public class AttackInput : AnimationInput
{
    public Wield activeWield;
    public HitType hitType;
    public AttackCoord[] attackCoords;
}
