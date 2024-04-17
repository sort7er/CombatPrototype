using UnityEngine;

public class GateToOpen : MonoBehaviour
{
    public InputReader reader;

    public Animator gateAnim;
    public AudioSource gateAS;
    public AudioClip[] openAC;
    public AudioClip[] closeAC;

    private void Awake()
    {
        reader.OnNextWeapon += OpenGate;
        reader.OnPreviousWeapon += CloseGate;
    }
    private void OnDestroy()
    {
        reader.OnNextWeapon -= OpenGate;
        reader.OnPreviousWeapon -= CloseGate;
    }



    public void OpenGate()
    {
        gateAnim.SetBool("Open",true);
    }
    public void CloseGate()
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
