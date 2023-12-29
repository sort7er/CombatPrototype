using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    [Header("Hit effect")]
    public int poolSize = 10;
    public ParticleSystem hitEffect;

    private ParticleSystem[] hitEffects;
    private int currentHitEffect;


    private void Awake()
    {
        instance = this;
        SetUpHitEffect();
    }

    private void SetUpHitEffect()
    {
        hitEffects = new ParticleSystem[poolSize];
        currentHitEffect = 0;

        for (int i = 0; i < poolSize; i++)
        {
            hitEffects[i] = Instantiate(hitEffect, ParentManager.instance.effects);
            hitEffects[i].gameObject.SetActive(false);
        }
    }

    public void Hit(Vector3 position, Vector3 direction, Vector3 upDirection)
    {
        ParticleSystem effect = hitEffects[currentHitEffect];

        effect.gameObject.SetActive(true);

        effect.transform.position = position;
        effect.transform.rotation = Quaternion.LookRotation(direction, upDirection);

        StartCoroutine(ResetEffect(effect));


        if(currentHitEffect < poolSize - 1)
        {
            currentHitEffect++;
        }
        else
        {
            currentHitEffect = 0;
        }

    }

    private IEnumerator ResetEffect(ParticleSystem effectToReset)
    {

        yield return new WaitForSeconds(effectToReset.startLifetime);
        effectToReset.gameObject.SetActive(false);
    }




}
