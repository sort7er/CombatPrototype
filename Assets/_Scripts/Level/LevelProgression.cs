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

    public GameTracking gameTracking;
    public ListElement objective;

    public Transform currentSpawn { get; private set; }

    private int villageEnemiesKilled;
    private int courtyardEnemiesKilled;
    private int bridgeEnemiesKilled;
    private int churchEnemiesKilled;
    private int watchtowerEnemiesKilled;


    private void Start()
    {
        EnterVillage();
        StartCoroutine(objective.SetListElement(VillageString(), 0));
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


    private string VillageString()
    {
        return "Kill all enemies in the village: " + villageEnemiesKilled.ToString() + "/" + villageEnemies.Length.ToString();
    }
    private string CourtyardString()
    {
        return "Kill all enemies in the courtyard: " + courtyardEnemiesKilled.ToString() + "/" + courtyardEnemies.Length.ToString();
    }
    private string BridgeString()
    {
        return "Kill the enemy on the bridge: " + bridgeEnemiesKilled.ToString() + "/" + bridgeEnemies.Length.ToString();
    }
    private string ChurchString()
    {
        return "Kill all enemies outside the church: " + churchEnemiesKilled.ToString() + "/" + churchEnemies.Length.ToString();
    }
    private string WatchtowerString()
    {
        return "Kill all enemies outside the watchtower: " + watchtowerEnemiesKilled.ToString() + "/" + watchtowerEnemies.Length.ToString();
    }



    //NewEnemyDead

    private void VillageEnemyKilled()
    {
        Action a = () => objective.UpdateListElement(VillageString());
        AddKill(ref villageEnemiesKilled, villageEnemies, VillageCompleted, a);
        
    }
    private void CourtyardEnemyKilled()
    {
        Action a = () => objective.UpdateListElement(CourtyardString());
        AddKill(ref courtyardEnemiesKilled, courtyardEnemies, CourtyardCompleted, a);
        
    }
    private void BridgeEnemyKilled()
    {
        Action a = () => objective.UpdateListElement(BridgeString());
        AddKill(ref bridgeEnemiesKilled, bridgeEnemies, BridgeCompleted, a);
        gameTracking.SwitchHUD();
    }
    private void ChurchEnemyKilled()
    {
        Action a = () => objective.UpdateListElement(ChurchString());
        AddKill(ref churchEnemiesKilled, churchEnemies, ChurchCompleted, a);
        
    }
    private void WatchtowerEnemyKilled()
    {

        Action a = () => objective.UpdateListElement(WatchtowerString());
        AddKill(ref watchtowerEnemiesKilled, watchtowerEnemies, WatchtowerCompleted, a);        
    }


    //Gates
    private void VillageCompleted()
    {
        objective.Completed(VillageString(), "Get to the courtyard");
        drawbridge.Open();
        gates[0].Open();
    }
    private void CourtyardCompleted()
    {
        objective.Completed(CourtyardString(), BridgeString());
        gates[1].Open();
    }
    private void BridgeCompleted()
    {
        objective.Completed(BridgeString(), "Get to the church");
        gates[2].Open();
    }
    private void ChurchCompleted()
    {
        objective.Completed(ChurchString(), "Get to the watchtower");
        gates[3].Open();
    }
    private void WatchtowerCompleted()
    {
        objective.Completed(WatchtowerString(), "");
        gameTracking.Finished();
    }

    //Triggers
    public void EnterVillage()
    {
        SetSpawn(0);
    }
    public void EnterCourtyard()
    {
        objective.Completed("Get to the courtyard", CourtyardString());
        SetSpawn(1);
    }
    public void EnterChurch()
    {
        objective.Completed("Get to the church", ChurchString());
        SetSpawn(2);
    }
    public void EnterWatchtower()
    {
        objective.Completed("Get to the watchtower", WatchtowerString());
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

    private void AddKill(ref int enemiesKilled, Enemy[] enemyArray, Action completed, Action updateObjective)
    {
        enemiesKilled++;
        if (enemiesKilled >= enemyArray.Length)
        {
            completed();
        }
        else
        {
            updateObjective();
        }
    }

    private void SetSpawn(int index)
    {
        currentSpawn = spawnPoints[index];
    }

}
