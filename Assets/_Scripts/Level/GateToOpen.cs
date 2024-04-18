using UnityEngine;

public class Gate : MonoBehaviour
{
    public InputReader reader;

    public Animator gateAnim;
    public AudioSource gateAS;
    public AudioClip[] openAC;
    public AudioClip[] closeAC;

    private void Awake()
    {
        reader.OnNextWeapon += Open;
        reader.OnPreviousWeapon += Close;
    }
    private void OnDestroy()
    {
        reader.OnNextWeapon -= Open;
        reader.OnPreviousWeapon -= Close;
    }



    public virtual void Open()
    {
        gateAnim.SetBool("Open",true);
    }
    public virtual void Close()
    {
        gateAnim.SetBool("Open", false);
    }

    public void PlayAudio()
    {
        if (gateAnim.GetBool("Open"))
        {
            gateAS.clip = openAC[Random.Range(0, openAC.Length)];
            gateAS.Play();
        }
        else
        {
            gateAS.clip = closeAC[Random.Range(0, closeAC.Length)];
            gateAS.Play();
        }
    }
}
