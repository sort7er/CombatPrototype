using System;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    public event Action<Health, WeaponTrigger> OnHit;

    public Vector3 contactPoint { get; private set; }
    public Vector3 swingDir { get; private set; }
    public Vector3 upDir { get; private set; }

    private Archetype archetype;
    private Collider weaponCollider;
    private Vector3 currentPos;
    private Vector3 previousPos;

    private ArchetypeAnimator hittingArchetype;
    private SlicingObject slicingObject;

    private void Awake()
    {
        archetype = GetComponentInParent<Archetype>();
        weaponCollider = GetComponent<Collider>();
        slicingObject = GetComponent<SlicingObject>();
        DisableCollider();
        
        currentPos = Vector3.zero;
        previousPos = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Health health))
        {
            //Check if is blocking or parrying
            hittingArchetype = CheckIfAnimator(health);

            if(hittingArchetype != null)
            {
                CheckIfBlock(other, health);
            }
            else
            {
                DoDamage(other, health);
            }
            
        }
        else if(other.TryGetComponent(out SlicableObject sliceble) && slicingObject != null)
        {
            //Check that object don't get sliced several times in the same blow
            if (!slicingObject.cannotSlice.Contains(sliceble))
            {
                //Slice object if not to small
                if (VolumeOfMesh(sliceble.mesh) > 0.15f)
                {
                    archetype.currentSlicingObject.Slice(sliceble);
                }
            }
        }
    }

    public ArchetypeAnimator CheckIfAnimator(Health health)
    {
        if (health.TryGetComponent(out Enemy enemy))
        {
            return enemy.currentArchetype.archetypeAnimator;
        }
        else if (health.TryGetComponent(out WeaponSelector weaponSelector))
        {
            return weaponSelector.currentArchetype.archetypeAnimator;
        }
        else
        {
            return null;
        }
    }

    private void CheckIfBlock(Collider other, Health health)
    {
        if (hittingArchetype.isParrying)
        {
            archetype.owner.Staggered();

            Vector3 direction = hittingArchetype.transform.position - transform.position;

            EffectManager.instance.Parry(transform.position + direction * 0.5f + Vector3.up * 0.3f);
        }
        else if (hittingArchetype.isBlocking)
        {
            Debug.Log("Block");
        }
        else
        {
            Debug.Log("No");
            DoDamage(other, health);
        }
    }

    private void DoDamage(Collider other, Health health)
    {
        upDir = transform.forward;
        contactPoint = other.ClosestPointOnBounds(transform.position);
        OnHit?.Invoke(health, this);
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

    public float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }
    private float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;

        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

}
