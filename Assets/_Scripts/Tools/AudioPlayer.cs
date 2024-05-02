using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] clips;

    public bool randomPitch = false;
    public bool playOnEnable = false;

    private int lastRnd;

    private float startPitch;
    private float pitchRange;

    private void Awake()
    {
        startPitch = audioSource.pitch;
        pitchRange = 0.1f;
    }

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

        audioSource.pitch = startPitch;

        if (randomPitch)
        {
            audioSource.pitch += Random.Range(-pitchRange, pitchRange);
        }




        audioSource.clip = clips[rnd];
        audioSource.Play();
    }
}
