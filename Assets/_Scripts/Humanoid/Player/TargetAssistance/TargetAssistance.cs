using System.Collections.Generic;
using UnityEngine;
using EnemyAI;

public class TargetAssistance : MonoBehaviour
{

    [Header("Values")]
    [SerializeField] private int maxColliders = 10;

    [Header("References")]
    public LayerMask enemyLayer;

    private Collider[] hitColliders;

    private List<Target> idealTargets = new();
    private List<Target> otherTargets = new();
    private List<Enemy> finalTargets = new();


    //Gridsorting
    private List<Vector3> keys = new();
    private Dictionary <Vector3, List<Enemy>> enemyGroups = new();

    private void Awake()
    {
        hitColliders = new Collider[maxColliders];
    }

    public List<Enemy> CheckForEnemies(Ability uniqueAbility)
    {
        CleanUpPreviousData();

        //Find the targets within the area and put them in the correct list
        AddTargetsToLists(uniqueAbility.range, uniqueAbility.idealDotProduct, uniqueAbility.acceptedDotProduct);

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

            if (newTarget == null)
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
        if (colliderTransform.TryGetComponent<Enemy>(out Enemy enemy))
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
    public List<List<Enemy>> GroupedEnemies(Vector3 centerPos, Vector3 halfExtends, Quaternion rotation, Vector3Int divisions)
    {
        List<Enemy> rawList = CastBox(centerPos, halfExtends, rotation);

        keys.Clear();
        enemyGroups.Clear();



        Vector3 gridSize = halfExtends * 2;
        Vector3 cellSize = new Vector3(gridSize.x / divisions.x, gridSize.y / divisions.y, gridSize.z / divisions.z);

        for (int i = 0; i < rawList.Count;i++)
        {
            Vector3 gridPos = transform.InverseTransformPoint(rawList[i].Position());
            gridPos.x += halfExtends.x;

            Vector3 key = GetKey(gridPos, cellSize, divisions);

            if(enemyGroups.ContainsKey(key))
            {
                enemyGroups[key].Add(rawList[i]);
            }
            else
            {
                List<Enemy> newList = new List<Enemy>() { rawList[i] };
                
                keys.Add(key);
                enemyGroups.Add(key, newList);

            }
        }

        List<List<Enemy>> sortedList = new();

        keys.Sort(SortByDistance);

        for(int i = 0; i < keys.Count;i++)
        {
            if (enemyGroups.ContainsKey(keys[i]))
            {
                sortedList.Add(enemyGroups[keys[i]]);
            }
        }

        return sortedList;  
    }

    private int SortByDistance(Vector3 g1, Vector3 g2)
    {
        float g1Distance = Vector3.Distance(g1, Vector3.zero);
        float g2Distance = Vector3.Distance(g2, Vector3.zero);

        return g1Distance.CompareTo(g2Distance);
    }
    private int SortByDotProduct(Vector3 g1, Vector3 g2)
    {
        Vector3 g1DirToTarget = Vector3.Normalize(g1 - transform.position);
        float g1DotProduct = Vector3.Dot(transform.forward, g1DirToTarget);

        Vector3 g2DirToTarget = Vector3.Normalize(g2 - transform.position);
        float g2DotProduct = Vector3.Dot(transform.forward, g2DirToTarget);

        return -g1DotProduct.CompareTo(g2DotProduct);
    }

    private Vector3 GetKey(Vector3 gridPos, Vector3 cellSize, Vector3Int divisions)
    {
        float x = CastToDivision(gridPos.x, cellSize.x, divisions.x);
        float y = CastToDivision(gridPos.y, cellSize.y, divisions.y);
        float z = CastToDivision(gridPos.z, cellSize.z, divisions.z);

        return new Vector3(x, y, z);
    }

    private float CastToDivision(float value, float size, int divisions)
    {
        float currentDiff;
        float smallestDiff = Mathf.Infinity;
        float closest = 0;

        for (int i = 0; i < divisions; i++)
        {
            currentDiff = Mathf.Abs(value - size * i);
            if (currentDiff < smallestDiff)
            {
                smallestDiff = currentDiff;
                closest = size * i;
            }
        }

        return closest;
    }

    private List<Enemy> CastBox(Vector3 centerPos, Vector3 halfExtends, Quaternion rotation)
    {

        Collider[] hits;
        hits = Physics.OverlapBox(centerPos, halfExtends, rotation, enemyLayer);

        List<Enemy> rawList = new();

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].TryGetComponent<Enemy>(out Enemy enemy))
            {
                Vector3 localPos = transform.InverseTransformPoint(enemy.Position());
     
                if (IsWithinBounds(localPos, halfExtends))
                {
                    rawList.Add(enemy);
                }
            }
        }

        return rawList;
    }
    private bool IsWithinBounds(Vector3 localPos, Vector3 halfExtends)
    {
        if(localPos.x < -halfExtends.x || localPos.x > halfExtends.x)
        {
            return false;
        }
        else if(localPos.y < -halfExtends.y || localPos.y > halfExtends.y)
        {
            return false;
        }
        else if (localPos.z < 0 || localPos.z > halfExtends.z * 2)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
