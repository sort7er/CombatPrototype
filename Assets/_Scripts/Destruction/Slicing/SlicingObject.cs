using UnityEngine;
using EzySlice;
using System.Collections.Generic;

public class SlicingObject : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float cutForce = 2000f;

    [Header("References")]
    [SerializeField] private Transform endSlicePoint;
    [SerializeField] private Transform startSlicePoint;

    private Vector3 lastPos, newPos;
    private Vector3 direction;

    public List<SlicableObject> cannotSlice { get; private set; } = new();
    private WeaponSelector weaponSelector;
    private ArchetypeAnimator archetypeAnimator;
    

    private void Awake()
    {
        weaponSelector = FindObjectOfType<Player>().weaponSelector;
        weaponSelector.OnNewArchetype += NewArchetype;
    }
    private void OnDestroy()
    {
        weaponSelector.OnNewArchetype -= NewArchetype;
    }

    private void Update()
    {
        newPos = endSlicePoint.position;
        direction = newPos - lastPos;
        lastPos = endSlicePoint.position;
    }

    public void CheckSlice(SlicableObject sliceble)
    {
        if (!cannotSlice.Contains(sliceble))
        {
            //Slice object if not to small
            if (VolumeOfMesh(sliceble.mesh) > 0.15f)
            {
                Slice(sliceble);
            }
        }
    }

    public void Slice(SlicableObject target)
    {
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, direction);
        planeNormal.Normalize();




        SlicedHull hull = target.gameObject.Slice(endSlicePoint.position, planeNormal);

        if(hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target.gameObject, target.meshRenderer.material);
            upperHull.transform.position = target.transform.position;
            upperHull.transform.rotation = target.transform.rotation;
            SlicableObject upperSlice = upperHull.AddComponent<SlicableObject>();
            upperSlice.SetUpSlicableObject(ParentManager.instance.meshes, cutForce);
            cannotSlice.Add(upperSlice);

            GameObject lowerHull = hull.CreateLowerHull(target.gameObject, target.meshRenderer.material);
            lowerHull.transform.position = target.transform.position;
            lowerHull.transform.rotation = target.transform.rotation;
            SlicableObject lowerSlice = lowerHull.AddComponent<SlicableObject>();
            lowerSlice.SetUpSlicableObject(ParentManager.instance.meshes, cutForce);
            cannotSlice.Add(lowerSlice);

            Destroy(target.gameObject);
        }
    }
    public void NewArchetype(Archetype newArchetype)
    {
        if(archetypeAnimator != null)
        {
            archetypeAnimator.OnActionDone -= SwingDone;
        }
        archetypeAnimator = newArchetype.archetypeAnimator;
        archetypeAnimator.OnActionDone += SwingDone;
        
    }
    public void SwingDone()
    {
        cannotSlice.Clear();
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
