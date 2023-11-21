using System.Collections.Generic;
using UnityEngine;

public class TargetAssistance : MonoBehaviour
{

    [Header("Values")]
    [SerializeField] private float overlapRange = 10f;
    [SerializeField] private int maxColliders = 10;
    [Range(-1, 1)]
    [SerializeField] private float acceptedDotProduct = 0.75f;
    [Range(-1, 1)]
    [SerializeField] private float idealDotProduct = 0.9f;


    [Header("References")]
    [SerializeField] private LayerMask enemyLayer;

    private Transform target;
    private Enemy targetEnemy;
    private Collider[] hitColliders;
    private InputReader inputReader; //Temp

    private List<Target> targetsToCheck = new();

    private float highest;
    private bool insideTargetDot;

    private void Awake()
    {
        inputReader = GetComponent<InputReader>(); //Temp 
        inputReader.OnFire += Temp; //Temp


        hitColliders = new Collider[maxColliders];
    }

    //Temp
    private void OnDestroy()
    {
        inputReader.OnFire -= Temp;
    }

    //Temp
    private void Temp()
    {
        CheckForEnemies(overlapRange, acceptedDotProduct);
    }

    public void CheckForEnemies(float range, float dotProduct)
    {
        insideTargetDot = false;
        int numColliders;
        numColliders = Physics.OverlapSphereNonAlloc(transform.position, range, hitColliders, enemyLayer);

        for (int i = 0; i < numColliders; i++)
        {
            // Find distance and dotproduct of everything inside the layer
            Target newTarget = CreateTarget(hitColliders[i].transform);

            if (newTarget.dotProduct >= acceptedDotProduct)
            {
                targetsToCheck.Add(newTarget);
            }


            //Check if targets are inside the accepted dotproduct
            

        }

        for (int i = 0; i < numColliders; i++)
        {
            // Find distance and dotproduct of everything inside the layer
            Target newTarget = CreateTarget(hitColliders[i].transform);

            if (newTarget.dotProduct >= acceptedDotProduct)
            {
                targetsToCheck.Add(newTarget);
            }


            //Check if targets are inside the accepted dotproduct


        }



        if (target != null)
        {
            if (target.TryGetComponent<Enemy>(out Enemy enemy))
            {
                targetEnemy = enemy;
                targetEnemy.SetAsTarget();
            }
        }



        //if (enemiesToCheck.Count > 0)
        //{
        //    // Afterwards return the closest of the remaining enemies

        //    //FindClosestEnemy();
        //    targetEnemy.SetAsTarget();
        //}
    }

    private Target CreateTarget(Transform colliderTransform)
    {
        Vector3 dirToTarget = Vector3.Normalize(colliderTransform.position - transform.position);
        float dotProduct = Vector3.Dot(transform.forward, dirToTarget);

        bool insideTarget = false;

        if(dotProduct >= idealDotProduct)
        {
            insideTarget = true;
        }

        float distance = Vector3.Distance(transform.position, colliderTransform.position);

        return new Target(colliderTransform, dotProduct, distance, ref insideTarget);
    }

    private void InsideDotProduct(Transform targetTrans, float dotProduct)
    {
        Vector3 dirToTarget = Vector3.Normalize(targetTrans.position - transform.position);
        float dot = Vector3.Dot(transform.forward, dirToTarget);

        if (dot >= dotProduct)
        {
            if(dot > highest)
            {
                highest = dot;
                target = targetTrans; 
            }
        }
    }

    //private void FindClosestEnemy()
    //{
    //    targetEnemy = enemiesToCheck[0];

    //    float closest = 20;
    //    for (int i = 0; i < enemiesToCheck.Count; i++)
    //    {
    //        float distance = Vector3.Distance(transform.position, enemiesToCheck[i].transform.position);

    //        if (distance < closest)
    //        {
    //            targetEnemy = enemiesToCheck[i];
    //            closest = distance;
    //        }
    //    }
    //}

}
