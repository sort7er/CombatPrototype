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

    private Player player;
    private SlicingObject slicingObject;

    private void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        slicingObject = GetComponent<SlicingObject>();
        player = FindObjectOfType<Player>();
        DisableCollider();
        
        currentPos = Vector3.zero;
        previousPos = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Health>(out Health health))
        {
            upDir = transform.forward;
            contactPoint = other.ClosestPointOnBounds(transform.position);
            OnHit?.Invoke(health, this);
        }
        else if(other.TryGetComponent<SlicableObject>(out SlicableObject sliceble) && slicingObject != null)
        {
            if (!slicingObject.cannotSlice.Contains(sliceble))
            {
                if (VolumeOfMesh(sliceble.mesh) > 0.15f)
                {
                    player.weaponSelector.currentArchetype.currentSlicingObject.Slice(sliceble);
                }
            }
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
