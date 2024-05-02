using DG.Tweening;
using UnityEngine;

public class DisableLight : MonoBehaviour
{
    public float duration;
    public Light lightToDisable;


    private float startIntensity;

    private void Awake()
    {
        startIntensity = lightToDisable.intensity;   
    }


    private void OnEnable()
    {
        lightToDisable.intensity = startIntensity;
        lightToDisable.DOIntensity(0, duration);
    }
}
