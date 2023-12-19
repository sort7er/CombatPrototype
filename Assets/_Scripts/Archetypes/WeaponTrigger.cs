using System;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    public event Action<Health, WeaponTrigger> OnHit;

    public Vector3 contactPoint { get; private set; }
    public Vector3 swingDir { get; private set; }
    public Vector3 upDir { get; private set; }

    private Collider weaponCollider;
    private Vector3 currentPos;
    private Vector3 previousPos;


    private void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        DisableCollider();
        
        currentPos = Vector3.zero;
        previousPos = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Health>(out Health health))
        {
            upDir = -transform.forward;
            contactPoint = other.ClosestPointOnBounds(transform.position);
            OnHit?.Invoke(health, this);
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

    private void Update()
    {
        currentPos = transform.position;
        swingDir = currentPos - previousPos;
        previousPos = transform.position;
    }
}
