using UnityEngine;

public class WeaponEffects : MonoBehaviour
{
    [Header("Model")]
    public TrailEffect[] trailEffects;
    public MeshRenderer[] meshRenderer;
    public Material defaultMaterial;
    public Material glowMaterial;


    private bool enableGlow;
    private bool updateGlow;
    private float transsision;
    private float enableSpeed;
    private float disableSpeed;

    private void Awake()
    {
        updateGlow = false;
    }

    #region Effect related
    public void DisableTrails()
    {
        for (int i = 0; i < trailEffects.Length; i++)
        {
            trailEffects[i].DisableTrails(0.25f);
        }
    }
    public void EnableTrails()
    {
        for (int i = 0; i < trailEffects.Length; i++)
        {
            trailEffects[i].EnableTrails(transform);
        }
    }

    public void EnableGlow(float enableSpeed = 2.5f)
    {
        if(meshRenderer.Length > 0)
        {
            SetMaterial(glowMaterial);
            SetGlowValue(1);
            this.enableSpeed = enableSpeed;
            transsision = 1;
            enableGlow = true;
            updateGlow = true;
        }

    }



    public void DisableGlow(float disableSpeed = 0.5f)
    {
        this.disableSpeed = disableSpeed;
        updateGlow = true;
        enableGlow  = false;
    }
    private void SetMaterial(Material material)
    {
        for (int i = 0; i < meshRenderer.Length; i++)
        {
            meshRenderer[i].material = material;
        }
    }
    private void SetGlowValue(float value)
    {
        glowMaterial.SetFloat("_Transission", value);
    }

    private void Update()
    {
        if (updateGlow)
        {
            Debug.Log("Updating");
            if (enableGlow)
            {
                GlowOn();
            }
            else
            {
                GlowOff();
            }
        }
        
    }
    private void GlowOn()
    {
        if (transsision > 0)
        {
            transsision -= Time.deltaTime * enableSpeed;
            SetGlowValue(transsision);
        }
        else
        {
            SetGlowValue(0);
            updateGlow = false;
        }
    }
    private void GlowOff()
    {
        if (transsision < 1)
        {
            transsision += Time.deltaTime * disableSpeed;
            SetGlowValue(transsision);
        }
        else
        {
            DisableTransissionDone();
        }
    }
    private void DisableTransissionDone()
    {
        SetGlowValue(1);
        SetMaterial(defaultMaterial);
        updateGlow = false;
    }


    #endregion
}
