using UnityEngine;

public class ShardContainer : MonoBehaviour
{
    [SerializeField] private Rigidbody[] shards;

    private void Awake()
    {
        for(int i = 0; i < shards.Length; i++)
        {
            shards[i].gameObject.SetActive(false);
        }
    }

    public void Blast(Vector3 direction)
    {
        for (int i = 0; i < shards.Length; i++)
        {
            shards[i].gameObject.SetActive(true);
            shards[i].gameObject.layer = 7;
            shards[i].transform.parent = ParentManager.instance.meshes;
            shards[i].AddExplosionForce(200, transform.position - direction, 5);
            Destroy(shards[i].gameObject, 4);
        }
    }
}
