using UnityEngine;
using HealthRelated;

public class EnemyHealth : Health
{
    [Header("For slice death")]
    [SerializeField] private SlicableObject[] meshes;
    [Header("For crumble death")]
    [SerializeField] private ShardContainer[] prefabs;


    private Enemy enemy;
    

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
    }
    public override void TakeDamage(int damage, Archetype killingArchetype, DamageType incomingDamage)
    {
        base.TakeDamage(damage, killingArchetype, incomingDamage);
        enemy.Hit();

        Vector3 pushback = transform.position - enemy.player.Position();
        enemy.AddForce(pushback.normalized * 2);
        
    }
    protected override void Dead(Archetype killingArchetype, DamageType incomingDamage)
    {
        base.Dead(killingArchetype, incomingDamage);

        if(incomingDamage == DamageType.Slice)
        {
            for (int i = 0; i < meshes.Length; i++) 
            {
                killingArchetype.hitBox.Slice(meshes[i]);
            }
        }
        if (incomingDamage == DamageType.Crumble)
        {
            int rnd = Random.Range(0, prefabs.Length);
            ShardContainer container = Instantiate(prefabs[rnd], transform.position, transform.rotation);

            Vector3 direction = transform.position - killingArchetype.owner.Position();
            container.Blast(direction * 2);
        }
        Destroy(gameObject);

    }
}
