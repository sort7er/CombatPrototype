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
    public override void TakeDamage(int damage, DamageType incomingDamage)
    {
        base.TakeDamage(damage, incomingDamage);
        enemy.Hit();
    }
    protected override void Dead(DamageType incomingDamage)
    {
        base.Dead(incomingDamage);

        if(incomingDamage == DamageType.Slice)
        {
            for (int i = 0; i < meshes.Length; i++) 
            {
                enemy.player.weaponSelector.currentArchetype.currentSlicingObject.Slice(meshes[i]);
            }
        }
        if (incomingDamage == DamageType.Crubmle)
        {
            int rnd = Random.Range(0, prefabs.Length);
            ShardContainer container = Instantiate(prefabs[rnd], transform.position, transform.rotation);

            Vector3 direction = transform.position - enemy.player.Position();
            container.Blast(direction * 2);
        }
        Destroy(gameObject);

    }
}
