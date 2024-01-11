using UnityEngine;

public class WeaponContainer : MonoBehaviour
{
    public Humanoid owner { get; private set; }
    public Archetype archetype { get; private set; }

    private void Awake()
    {
        if(transform.parent.TryGetComponent(out Enemy enemy))
        {
            owner = enemy;
        }
        else if(transform.parent.parent.TryGetComponent(out PlayerMovement player))
        {
            owner = player;
        }

        archetype = transform.GetComponentInChildren<Archetype>(true);
    }
}
