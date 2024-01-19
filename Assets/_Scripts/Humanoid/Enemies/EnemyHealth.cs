using UnityEngine;
using HealthRelated;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHealth : Health
{
    [Header("For slice death")]
    [SerializeField] private SlicableObject[] meshes;
    [Header("For crumble death")]
    [SerializeField] private ShardContainer[] prefabs;

    [SerializeField] private Image skull;

    private Enemy enemy;
    

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
        skull.gameObject.SetActive(false);
    }
    public override void TakeDamage(int damage, int postureDamage, Archetype killingArchetype, DamageType incomingDamage)
    {
        base.TakeDamage(damage, postureDamage, killingArchetype, incomingDamage);
        enemy.Hit();

        Vector3 pushback = transform.position - enemy.player.Position();
        //enemy.AddForce(pushback.normalized * 2);
        
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
    protected override void DrainedPosture()
    {
        base.DrainedPosture();
        skull.gameObject.SetActive(true);
        skull.transform.DOPunchScale(Vector3.one * 0.1f, 3f).SetLoops(-1);
        SetHealth(1);
    }
    protected override void StaggerDone()
    {
        base.StaggerDone();
        skull.transform.DOKill();
        skull.gameObject.SetActive(false);
    }
}
