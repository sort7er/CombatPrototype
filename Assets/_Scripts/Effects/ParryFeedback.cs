using TMPro;
using UnityEngine;

public class ParryFeedback : Billboard
{
    public TextMeshProUGUI feedbackText;
    public Animator feedbackAnim;
    public AnimationClip feedbackAnimation;

    public void StartFeedback(string message)
    {
        feedbackText.text = message;
        feedbackAnim.SetTrigger("SendFeedback");
    }

    public float Duration()
    {
        return feedbackAnimation.length;
    }
}
