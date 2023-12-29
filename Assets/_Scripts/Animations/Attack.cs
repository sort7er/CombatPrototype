using UnityEngine;

public class Attack : Anim
{
    public int damage;
    public float queuePoint;


    public Attack(AnimationClip clip, int dmg, float queuePoint) : base(clip)
    {
        damage = dmg;
        this.queuePoint = queuePoint;
    }
}
