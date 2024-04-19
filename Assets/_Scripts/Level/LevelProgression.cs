using UnityEngine;

public class LevelProgression : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    public Transform currentSpawn { get; private set; }


    private void Awake()
    {
        EnterVillage();
    }

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
