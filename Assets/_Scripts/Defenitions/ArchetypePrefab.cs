using UnityEngine;

public class ArchetypePrefab : MonoBehaviour
{
    public Animator archetypeAnim { get; private set; }

    private void Awake()
    {
        archetypeAnim = GetComponent<Animator>();
    }

    public void Fire()
    {
        archetypeAnim.SetTrigger("Fire");
        Debug.Log("Huh");
    }
    public void HeavyFire()
    {
        archetypeAnim.SetTrigger("HeavyFire");
    }
    public void StopFire()
    {
        archetypeAnim.ResetTrigger("Fire");
        archetypeAnim.ResetTrigger("HeavyFire");
    }

}
