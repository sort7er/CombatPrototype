using UnityEngine;
using UnityEngine.VFX;
public class EnemyDissolve : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material dissolveMaterial;

    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private VisualEffect vfx;

    [SerializeField] private float timeToDie = 2;

    private float disolveTimer;
    private bool startDissolve;

    private void Awake()
    {
        meshRenderer.material = defaultMaterial;
        startDissolve= false;
    }

    public void Dissolve()
    {
        meshRenderer.material = dissolveMaterial;
        disolveTimer = timeToDie;
        startDissolve = true;
        dissolveMaterial.SetFloat("_Dissolve", 1);
        vfx.Play();
    }

    private void Update()
    {
        if(startDissolve)
        {
            if(disolveTimer > 0)
            {
                disolveTimer -= Time.deltaTime;

                float remapedValue = Tools.Remap(disolveTimer, timeToDie, 0, 1, 0);

                dissolveMaterial.SetFloat("_Dissolve", remapedValue);
            }
            else
            {
                startDissolve= false;
                dissolveMaterial.SetFloat("_Dissolve", 0);
                gameObject.SetActive(false);
            }
        }
    }

}
