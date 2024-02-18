using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] clips;

    private int lastRnd;

    public bool playOnEnable = false;

    private void OnEnable()
    {
        if(playOnEnable)
        {
            lastRnd = clips.Length;
            Play();
        }
    }
    private void OnDisable()
    {
        audioSource.Stop();
    }

    public void Play()
    {
        int rnd = Random.Range(0, clips.Length);
        
        if (clips.Length > 1)
        {
            while (rnd == lastRnd)
            {
                rnd = Random.Range(0, clips.Length);
            }
        }


        audioSource.clip = clips[rnd];
        audioSource.Play();
    }
}
