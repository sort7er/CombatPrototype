using EnemyAI;
using UnityEngine;

public class LevelProgression : MonoBehaviour
{


    [Header("Spawning")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Enemies")]
    [SerializeField] private Enemy[] villageEnemies;
    [SerializeField] private Enemy[] courtyardEnemies;
    [SerializeField] private Enemy[] churchEnemies;
    [SerializeField] private Enemy[] watchtowerEnemies;

    [Header("Gates")]
    public Drawbridge drawbridge;
    public Gate[] gates;

    public Transform currentSpawn { get; private set; }

    private int villageEnemiesKilled;

    private void Start()
    {
        EnterVillage();

        for(int i = 0; i < villageEnemies.Length; i++)
        {
            villageEnemies[i].health.OnDeath += VillageEnemy;
        }
    }
    private void OnDestroy()
    {
        for(int i = 0; i < villageEnemies.Length; i++)
        {
            villageEnemies[i].health.OnDeath -= VillageEnemy;
        }
    }

    //NewEnemyDead

    private void VillageEnemy()
    {
        villageEnemiesKilled++;
        if(villageEnemiesKilled >= villageEnemies.Length)
        {
            VillageCompleted();
        }
    }

    //Gates
    private void VillageCompleted()
    {
        drawbridge.Open();
        gates[0].Open();
    }

    //Triggers
    public void EnterVillage()
    {
        SetSpawn(0);
    }
    public void EnterCourtyard()
    {
        SetSpawn(1);
    }
    public void EnterChurch()
    {
        SetSpawn(2);
    }
    public void EnterWatchtower()
    {
        SetSpawn(3);
    }

    private void SetSpawn(int index)
    {
        currentSpawn = spawnPoints[index];
    }

}
