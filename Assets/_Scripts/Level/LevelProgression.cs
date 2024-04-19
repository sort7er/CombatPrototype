using EnemyAI;
using System;
using UnityEngine;

public class LevelProgression : MonoBehaviour
{


    [Header("Spawning")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Enemies")]
    [SerializeField] private Enemy[] villageEnemies;
    [SerializeField] private Enemy[] courtyardEnemies;
    [SerializeField] private Enemy[] bridgeEnemies;
    [SerializeField] private Enemy[] churchEnemies;
    [SerializeField] private Enemy[] watchtowerEnemies;

    [Header("Gates")]
    public Drawbridge drawbridge;
    public Gate[] gates;

    public Transform currentSpawn { get; private set; }

    private int villageEnemiesKilled;
    private int courtyardEnemiesKilled;
    private int bridgeEnemiesKilled;
    private int churchEnemiesKilled;
    private int watchtowerEnemiesKilled;

    private void Start()
    {
        EnterVillage();

        Subscribe(VillageEnemyKilled, villageEnemies);
        Subscribe(CourtyardEnemyKilled, courtyardEnemies);
        Subscribe(BridgeEnemyKilled, bridgeEnemies);
        Subscribe(ChurchEnemyKilled, churchEnemies);
        Subscribe(WatchtowerEnemyKilled, watchtowerEnemies);

    }
    private void OnDestroy()
    {
        Unsubscribe(VillageEnemyKilled, villageEnemies);
        Unsubscribe(CourtyardEnemyKilled, courtyardEnemies);
        Unsubscribe(BridgeEnemyKilled, bridgeEnemies);
        Unsubscribe(ChurchEnemyKilled, churchEnemies);
        Unsubscribe(WatchtowerEnemyKilled, watchtowerEnemies);
    }


    //NewEnemyDead

    private void VillageEnemyKilled()
    {
        AddKill(ref villageEnemiesKilled, villageEnemies, VillageCompleted);
    }
    private void CourtyardEnemyKilled()
    {
        AddKill(ref courtyardEnemiesKilled, courtyardEnemies, CourtyardCompleted);
    }
    private void BridgeEnemyKilled()
    {
        AddKill(ref bridgeEnemiesKilled, bridgeEnemies, BridgeCompleted);
    }
    private void ChurchEnemyKilled()
    {
        AddKill(ref churchEnemiesKilled, churchEnemies, ChurchCompleted);
    }
    private void WatchtowerEnemyKilled()
    {
        AddKill(ref watchtowerEnemiesKilled, watchtowerEnemies, WatchtowerCompleted);
    }


    //Gates
    private void VillageCompleted()
    {
        drawbridge.Open();
        gates[0].Open();
    }
    private void CourtyardCompleted()
    {
        gates[1].Open();
    }
    private void BridgeCompleted()
    {
        gates[2].Open();
    }
    private void ChurchCompleted()
    {
        gates[3].Open();
    }
    private void WatchtowerCompleted()
    {
        Debug.Log("Well done");
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

    //Speed up functions
    private void Subscribe(Action method, Enemy[] enemyArray)
    {
        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyArray[i].health.OnDeath += method;
        }
    }
    private void Unsubscribe(Action method, Enemy[] enemyArray)
    {
        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyArray[i].health.OnDeath -= method;
        }
    }

    private void AddKill(ref int enemiesKilled, Enemy[] enemyArray, Action completed)
    {
        enemiesKilled++;
        if (enemiesKilled >= enemyArray.Length)
        {
            completed();
        }
    }

    private void SetSpawn(int index)
    {
        currentSpawn = spawnPoints[index];
    }

}
