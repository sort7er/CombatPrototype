using UnityEngine;
using Attacks;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHealth : Health
{
    [Header("For slice death")]
    [SerializeField] private SlicableMesh[] meshes;
    [Header("For crumble death")]
    [SerializeField] private ShardContainer[] prefabs;

    [Header("When stagered")]
    [SerializeField] private Image skull;

    protected override void Awake()
    {
        base.Awake();
        skull.gameObject.SetActive(false);
    }

    public override void TakeDamage(Weapon attackingWeapon)
    {
        base.TakeDamage(attackingWeapon);


        //enemy.Hit();

        //Vector3 pushback = transform.position - enemy.player.Position();
        //enemy.AddForce(pushback.normalized * 2);

    }

    protected override void Dead(Weapon attackingWeapon)
    {
        base.Dead(attackingWeapon);

        if (attackingWeapon.currentAttack.hitType == HitType.slice)
        {
            for (int i = 0; i < meshes.Length; i++)
            {
                attackingWeapon.Slice(meshes[i]);
            }
        }
        else if (attackingWeapon.currentAttack.hitType == HitType.crumble)
        {
            int rnd = Random.Range(0, prefabs.Length);
            ShardContainer container = Instantiate(prefabs[rnd], transform.position, transform.rotation);

            Vector3 direction = transform.position - attackingWeapon.owner.Position();
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
