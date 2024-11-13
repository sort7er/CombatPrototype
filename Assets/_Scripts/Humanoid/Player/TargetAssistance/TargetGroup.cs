using EnemyAI;
using System.Collections.Generic;
using UnityEngine;

public class TargetGroup : MonoBehaviour
{
    public List<Enemy> enemies;
    public float dotProduct;
    public float distance;

    public TargetGroup(List<Enemy> enemies)
    {
        this.enemies = enemies;
    }

    public void SetDotProductAndDistance(float dotProduct, float distance)
    {
        this.dotProduct = dotProduct;
        this.distance = distance;
    }

    public void AddEnemyToGroup(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public Vector3 AveragePosOfGroup()
    {
        Vector3 targetPos = Vector3.zero;
        for (int i = 0; i < enemies.Count; i++)
        {
            targetPos += enemies[i].Position();
        }

        return targetPos / enemies.Count;
    }
}
