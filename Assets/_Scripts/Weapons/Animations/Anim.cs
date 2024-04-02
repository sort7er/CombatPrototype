using UnityEngine;

public class Anim
{
    public AnimationClip animationClip;
    public float duration { get; private set; }
    public int state { get; private set; }

    public float exitTime { get; private set; }
    public float exitTimeSeconds { get; private set; }
    public float transitionDuration { get; private set; }

    public Anim(string stateName)
    {
        state = Animator.StringToHash(stateName);
    }

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
    public Anim(AnimationClip clip, float exitTime, float transitionDuration)
    {
        if (clip != null)
        {
            animationClip = clip;
            duration = animationClip.length;
            state = Animator.StringToHash(animationClip.name);
            this.exitTime = exitTime;
            this.transitionDuration = transitionDuration;
            exitTimeSeconds = Tools.Remap(exitTime, 0, 1, 0, duration);
        }
        else
        {
            Debug.Log("Missing clip");
        }

    }

}
