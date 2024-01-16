using System;
using UnityEngine;

public class WeaponContainer : MonoBehaviour
{
    public event Action<Humanoid> OnOwnerFound;
    public Humanoid owner { get; private set; }
    public Archetype archetype { get; private set; }

    public void FindOwner()
    {
        if (transform.parent.TryGetComponent(out Enemy enemy))
        {
            owner = enemy;
            OnOwnerFound?.Invoke(owner);
        }
        else if (transform.parent.parent.TryGetComponent(out PlayerMovement player))
        {
            owner = player;
            OnOwnerFound?.Invoke(owner);
        }

        archetype = transform.GetComponentInChildren<Archetype>(true);
    }
}
