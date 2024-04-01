using UnityEngine;

public class Anim
{
    public AnimationClip animationClip;
    public float duration { get; private set; }
    public int state { get; private set; }


    public Anim(AnimationClip clip)
    {
        if(clip != null)
        {
            animationClip = clip;
            duration = animationClip.length;
            state = Animator.StringToHash(animationClip.name);
        }
        else
        {
            Debug.Log("Missing clip");
        }

    }
    public Anim(string stateName)
    {
        state = Animator.StringToHash(stateName);
    }
}
