using System.Collections.Generic;
using UnityEngine;

public class TargetAssistance : MonoBehaviour
{

    [Header("Values")]
    [SerializeField] private int maxColliders = 10;

    [Header("References")]
    [SerializeField] private LayerMask enemyLayer;

    private Collider[] hitColliders;

    private List<Target> idealTargets = new();
    private List<Target> otherTargets = new();
    private List<Enemy> finalTargets = new();

    private void Awake()
    {
        hitColliders = new Collider[maxColliders];
    }

    public List<Enemy> CheckForEnemies(TargetAssistanceParams targetAssistanceParams, out int numIdealTarget)
    {
        CleanUpPreviousData();

        //Find the targets within the area and put them in the correct list
        AddTargetsToLists(targetAssistanceParams.range, targetAssistanceParams.idealDotProduct, targetAssistanceParams.acceptedDotProduct);

        numIdealTarget = idealTargets.Count;

        //Sort lists based on the distance and dotproduct
        idealTargets.Sort(SortByDistance);
        otherTargets.Sort(SortByDotProduct);

        //Want the highest dot products first
        otherTargets.Reverse();

        //Add the elements to the final list so they are structured in the right order
        AddToFinalList();

        return finalTargets;
    }

    private void CleanUpPreviousData()
    {
        idealTargets.Clear();
        otherTargets.Clear();
        finalTargets.Clear();
    }

    private void AddTargetsToLists(float range, float idealDotProduct, float acceptedDotProduct)
    {
        int numColliders;
        numColliders = Physics.OverlapSphereNonAlloc(transform.position, range, hitColliders, enemyLayer);


        for (int i = 0; i < numColliders; i++)
        {
            Target newTarget = CreateTarget(hitColliders[i].transform);

            if(newTarget == null)
            {
                break;
            }


            if (newTarget.dotProduct >= idealDotProduct)
            {
                idealTargets.Add(newTarget);
            }
            else if (newTarget.dotProduct >= acceptedDotProduct)
            {
                otherTargets.Add(newTarget);
            }

        }
    }


    private Target CreateTarget(Transform colliderTransform)
    {
        if(colliderTransform.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Vector3 dirToTarget = Vector3.Normalize(colliderTransform.position - transform.position);
            float dotProduct = Vector3.Dot(transform.forward, dirToTarget);
            float distance = Vector3.Distance(transform.position, colliderTransform.position);


            return new Target(enemy, dotProduct, distance);
        }
        else
        {
            return null;
        }

        
    }

    private void AddToFinalList()
    {

        for (int i = 0; i < idealTargets.Count; i++)
        {
            finalTargets.Add(idealTargets[i].enemy);
        }
        for (int i = 0; i < otherTargets.Count; i++)
        {
            finalTargets.Add(otherTargets[i].enemy);
        }
    }


    private int SortByDistance(Target t1, Target t2)
    {
        return t1.distance.CompareTo(t2.distance);
    }

    private int SortByDotProduct(Target t1, Target t2)
    {
        return t1.dotProduct.CompareTo(t2.dotProduct);
    }
}
