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

    public List<SlicableObject> cannotSlice /*{ get; private set; }*/ = new();
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
}
