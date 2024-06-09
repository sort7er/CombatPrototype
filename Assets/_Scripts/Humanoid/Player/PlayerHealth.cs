using UnityEngine;
using Stats;
using DG.Tweening;

public class PlayerHealth : Health
{
    [Header("Vignette")]
    [SerializeField] private float vignetteDuration;
    [SerializeField] private Material vignetteMaterial;


    protected override void Awake()
    {
        base.Awake();
        SetSize(0.4f);
    }
    private void SetSize(float size)
    {
        vignetteMaterial.SetFloat("_VignetteSize", size);
    }

    public override void MinusHealth(int damage)
    {
        if (damage <= 0)
        {
            return;
        }

        base.MinusHealth(damage);
        Hit(health);
    }
    private void Hit(int health)
    {
        float amount = Tools.Remap(health, 0, 100, 1f, 0.6f);
        vignetteMaterial.DOKill();

        vignetteMaterial.DOFloat(amount, "_VignetteSize", vignetteDuration * 0.1f).OnComplete(() =>
        {
            vignetteMaterial.DOFloat(0.4f, "_VignetteSize", vignetteDuration * 0.9f);
        });
    }
    protected override void StunnedDone()
    {
        base.StunnedDone();
        SetPosture(startPosture);
    }
}
