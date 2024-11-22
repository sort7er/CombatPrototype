using UnityEngine;

public class HandEffects : MonoBehaviour
{
    public ParticleSystem leftParticleSystem;
    public ParticleSystem rightParticleSystem;

    private void Awake()
    {
        DisableBoth();
    }

    public void EnableLeft()
    {
        leftParticleSystem.Play();
    }
    public void EnableRight()
    {
        rightParticleSystem.Play();
    }
    public void DisableBoth()
    {
        DisableLeft();
        DisableRight();
    }
    public void DisableLeft()
    {
        leftParticleSystem.Stop();
    }
    public void DisableRight()
    {
        rightParticleSystem.Stop();
    }
}
