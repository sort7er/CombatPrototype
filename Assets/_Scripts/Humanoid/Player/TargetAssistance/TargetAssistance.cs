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
    private Dictionary <Vector3, TargetGroup> enemyGroups = new();

    //public Transform cube;

    private void Awake()
    {
        hitColliders = new Collider[maxColliders];
    }

    #region Sphere cast
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
    #endregion

    #region Putting groups of enemies into a grid
    public List<TargetGroup> GroupedEnemies(Vector3 centerPos, Vector3 halfExtends, Quaternion rotation, Vector3Int divisions)
    {
        keys.Clear();
        enemyGroups.Clear();

        List<Enemy> rawList = CastBox(centerPos, halfExtends, rotation);
        ConvertRawListToDictionary(rawList, halfExtends, divisions);



        List<TargetGroup> sortedList = new();
        for (int i = 0; i < keys.Count; i++)
        {
            if (enemyGroups.ContainsKey(keys[i]))
            {
                CalculateDotAndDistance(enemyGroups[keys[i]]);
                sortedList.Add(enemyGroups[keys[i]]);
            }
        }

        sortedList.Sort(SortByDotProductAndDistance);

        return sortedList;
    }
    public List<Enemy> CastBox(Vector3 centerPos, Vector3 halfExtends, Quaternion rotation, bool includeBounds = true)
    {

        Collider[] hits;
        hits = Physics.OverlapBox(centerPos, halfExtends, rotation, enemyLayer);

        //cube.position = centerPos;
        //cube.rotation = rotation;
        //cube.localScale = halfExtends * 2;

        List<Enemy> rawList = new();

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].TryGetComponent<Enemy>(out Enemy enemy))
            {
                Vector3 localPos = transform.InverseTransformPoint(enemy.Position());

                if (!includeBounds)
                {
                    rawList.Add(enemy);
                }
                else if(IsWithinBounds(localPos, halfExtends))
                {
                    rawList.Add(enemy);
                }
            }
        }

        return rawList;
    }
    #region Shortcuts
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
    #endregion

    private void ConvertRawListToDictionary(List<Enemy> rawList, Vector3 halfExtends, Vector3Int divisions)
    {
        Vector3 gridSize = halfExtends * 2;
        Vector3 cellSize = new Vector3(gridSize.x / divisions.x, gridSize.y / divisions.y, gridSize.z / divisions.z);

        for (int i = 0; i < rawList.Count; i++)
        {
            Vector3 gridPos = transform.InverseTransformPoint(rawList[i].Position());
            gridPos.x += halfExtends.x;

            Vector3 key = GetKey(gridPos, cellSize, divisions);

            if (enemyGroups.ContainsKey(key))
            {
                enemyGroups[key].AddEnemyToGroup(rawList[i]);
            }
            else
            {
                List<Enemy> newList = new List<Enemy>() { rawList[i] };

                TargetGroup newGroup = new TargetGroup(newList);

                keys.Add(key);
                enemyGroups.Add(key, newGroup);
            }
        }
    }

    private void CalculateDotAndDistance(TargetGroup targets)
    {
        Vector3 dirToTarget = Vector3.Normalize(targets.AveragePosOfGroup() - transform.position);
        float dotProduct = Vector3.Dot(dirToTarget, transform.forward);
        float distance = Vector3.Distance(transform.position, targets.AveragePosOfGroup());

        targets.SetDotProductAndDistance(dotProduct, distance);
    }


    #endregion

    #region Sorting
    private int SortByDotProduct(Target t1, Target t2)
    {
        return t1.dotProduct.CompareTo(t2.dotProduct);
    }
    private int SortByDistance(Target t1, Target t2)
    {
        return t1.distance.CompareTo(t2.distance);
    }
    private int SortByDotProductAndDistance(TargetGroup g1, TargetGroup g2)
    {

        float threshold = 0.2f;

        if (Mathf.Abs(g1.dotProduct) < threshold && Mathf.Abs(g1.dotProduct) < threshold)
        {
            return SortByDistance(g1, g2);
        }
        else
        {
            return -g1.dotProduct.CompareTo(g2.dotProduct);
        }
    }
    private int SortByDistance(TargetGroup g1, TargetGroup g2)
    {
        return g1.distance.CompareTo(g2.distance);
    }
    #endregion
}
