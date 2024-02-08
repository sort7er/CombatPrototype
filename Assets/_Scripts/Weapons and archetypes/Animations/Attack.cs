using UnityEngine;
using Attacks;

namespace Attacks
{
    public enum Wield
    {
        right,
        left,
        both
    }
    //public enum AttributeAffected
    //{
    //    normal,
    //    onlyPosture
    //}
}

public class Attack : Anim
{
    public Wield currentWield;


    public Attack(AnimationClip clip, Wield wield) : base(clip)
    {
        this.currentWield = wield;
    }
}
