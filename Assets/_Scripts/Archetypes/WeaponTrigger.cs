using System;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    public event Action<Health, Vector3, WeaponTrigger> OnHit;

    private Collider weaponCollider;

    private void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        DisableCollider();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Health>(out Health health))
        {
            Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);
            OnHit?.Invoke(health, contactPoint, this);
        }
    }

    public void EnableCollider()
    {
        weaponCollider.enabled = true;
    }
    public void DisableCollider()
    {
        weaponCollider.enabled = false;
    }
}
