using EnemyAI;
using UnityEngine;

public class Water : MonoBehaviour
{
    public LevelProgression levelProgression;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
        {
            enemy.Dead();
        }
        if(other.TryGetComponent<Player>(out Player player))
        {
            player.ResetForce();
            player.SetTransform(levelProgression.currentSpawn);
        }
    }
}
