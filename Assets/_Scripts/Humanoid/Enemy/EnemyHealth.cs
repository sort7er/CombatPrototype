using UnityEngine;
using Attacks;
using UnityEngine.UI;
using DG.Tweening;
using Stats;
public class EnemyHealth : Health
{
    [Header("For slice death")]
    [SerializeField] private SlicingController slicingController;
    [Header("For crumble death")]
    [SerializeField] private ShardContainer[] prefabs;

    [Header("When stagered")]
    [SerializeField] private Image skull;

    protected override void Awake()
    {
        base.Awake();
        skull.gameObject.SetActive(false);
    }

    public override void TakeDamage(Humanoid attacker, Vector3 hitPoint)
    {
        base.TakeDamage(attacker, hitPoint);
    }

    protected override void Dead(Weapon attackingWeapon)
    {
        if (attackingWeapon.currentAttack.hitType == HitType.slice)
        {
            slicingController.Slice(attackingWeapon);
        }
        else if (attackingWeapon.currentAttack.hitType == HitType.crumble)
        {
            int rnd = Random.Range(0, prefabs.Length);
            ShardContainer container = Instantiate(prefabs[rnd], transform.position, transform.rotation);

            Vector3 direction = transform.position - attackingWeapon.owner.Position();
            container.Blast(direction * 2);
        }
        base.Dead(attackingWeapon);
        owner.Dead();
    }

    protected override void JustCutDead(Weapon attackingWeapon, Vector3 worldPos, Vector3 planeNormal)
    {
        if(slicingController != null)
        {
            slicingController.JustCut(attackingWeapon, worldPos, planeNormal);
        }

        base.JustCutDead(attackingWeapon, worldPos, planeNormal);
        owner.Dead();
    }

    protected override void DrainedPosture()
    {
        base.DrainedPosture();
        skull.gameObject.SetActive(true);
        skull.transform.DOPunchScale(Vector3.one * 0.1f, 3f).SetLoops(-1);
        healthSlider.gameObject.SetActive(false);
        canvasGroupPosture.gameObject.SetActive(false);
        SetHealth(1);
    }
    protected override void StunnedDone()
    {
        base.StunnedDone();
        skull.transform.DOKill();
        skull.gameObject.SetActive(false);
        healthSlider.gameObject.SetActive(true);
        canvasGroupPosture.gameObject.SetActive(true);
        SetHealth(storedHealth);
        SetPosture(startPosture * 0.5f);
    }
}
