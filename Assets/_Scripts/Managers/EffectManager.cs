using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    [Header("Hit effect")]
    public int poolSize = 10;
    public ParticleSystem hitEffect;

    [Header("Parry effect")]
    public int pPoolSize = 4;
    public ParticleSystem parryEffect;

    [Header("Perfect parry effect")]
    public int ppPoolSize = 3;
    public ParticleSystem perfectParryEffect;

    [Header("Parry feedback effect")]
    public int pfPoolSize = 3;
    public ParryFeedback parryFeedbackEffect;

    [Header("Slice effect")]
    public int sPoolSize = 10;
    public ParticleSystem slashEffect;

    [Header("Katana effect")]
    public int kPoolSize = 10;
    public ParticleSystem katanaEffect;

    [Header("Thrust effect")]
    public int tPoolSize = 10;
    public ParticleSystem thrustEffect;

    private ParticleSystem[] hit;
    private int currentHit;

    private ParticleSystem[] slash;
    private int currentSlash;

    private ParticleSystem[] katana;
    private int currentKatana;


    private ParticleSystem[] thrust;
    private int currentThrust;

    private ParticleSystem[] parry;
    private int currentParry;

    private ParticleSystem[] perfectParry;
    private int currentPerfectParry;

    private ParryFeedback[] parryFeedback;
    private int currentParryFeedback;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetUpEffect(hitEffect, ref hit, ref currentHit, poolSize);
        SetUpEffect(slashEffect, ref slash, ref currentSlash, sPoolSize);
        SetUpEffect(thrustEffect, ref thrust, ref currentThrust, tPoolSize);
        SetUpEffect(katanaEffect, ref katana, ref currentKatana, kPoolSize);
        SetUpEffect(parryEffect, ref parry, ref currentParry, pPoolSize);
        SetUpEffect(perfectParryEffect, ref perfectParry, ref currentPerfectParry, pPoolSize);
        SetUpFeedback(parryFeedbackEffect, ref parryFeedback, ref currentParryFeedback, pfPoolSize);
    }
    private void SetUpEffect(ParticleSystem prefab, ref ParticleSystem[] array, ref int current, int poolSize)
    {
        array = new ParticleSystem[poolSize];
        current = 0;

        for (int i = 0; i < poolSize; i++)
        {
            array[i] = Instantiate(prefab, ParentManager.instance.effects);
            array[i].gameObject.SetActive(false);
        }
    }
    private void SetUpFeedback(ParryFeedback prefab, ref ParryFeedback[] array, ref int current, int poolSize)
    {
        array = new ParryFeedback[poolSize];
        current = 0;

        for (int i = 0; i < poolSize; i++)
        {
            array[i] = Instantiate(prefab, ParentManager.instance.effects);
            array[i].gameObject.SetActive(false);
        }
    }


    public void Hit(Vector3 position, Vector3 direction, Vector3 upDirection)
    {
        ParticleSystem effect = hit[currentHit];

        effect.gameObject.SetActive(true);

        effect.transform.position = position;

        effect.transform.rotation = Quaternion.LookRotation(direction, upDirection);

        StartCoroutine(ResetEffect(effect));


        if(currentHit < poolSize - 1)
        {
            currentHit++;
        }
        else
        {
            currentHit = 0;
        }

    }
    public void Slash(ParticleSystem slash, Vector3 position, Vector3 direction, Vector3 upDirection, Transform parent, float sizeMultiplier = 1f)
    {
        ParticleSystem effect;

        if (slash == katanaEffect)
        {
            effect = katana[currentKatana];
            IncreasePool(ref currentKatana, kPoolSize);
        }
        else
        {
            effect = this.slash[currentSlash];
            IncreasePool(ref currentSlash, sPoolSize);
        }


        effect.gameObject.SetActive(true);

        effect.transform.parent = parent;

        effect.transform.position = position;
        effect.transform.rotation = Quaternion.LookRotation(direction, upDirection);
        effect.transform.localScale = Vector3.one * sizeMultiplier;

        StartCoroutine(ResetEffect(effect));
    }
    public void Thrust(Vector3 position, Vector3 direction, Vector3 upDirection, Transform parent, float sizeMultiplier = 1f)
    {
        ParticleSystem effect = thrust[currentThrust];
        IncreasePool(ref currentThrust, tPoolSize);

        effect.gameObject.SetActive(true);

        effect.transform.parent = parent;

        effect.transform.position = position;
        effect.transform.rotation = Quaternion.LookRotation(direction, upDirection);
        effect.transform.localScale = Vector3.one * sizeMultiplier;

        StartCoroutine(ResetEffect(effect));
    }

    public void Parry(Vector3 position)
    {
        ParticleSystem effect = parry[currentParry];

        IncreasePool(ref currentParry, pPoolSize);

        effect.gameObject.SetActive(true);
        effect.transform.position = position;

        StartCoroutine(ResetEffect(effect));
    }
    public void PerfectParry(Vector3 position)
    {
        ParticleSystem effect = perfectParry[currentPerfectParry];

        IncreasePool(ref currentPerfectParry, ppPoolSize);

        effect.gameObject.SetActive(true);
        effect.transform.position = position;

        StartCoroutine(ResetEffect(effect));
    }
    public void ParryFeedback(Vector3 position, string feedback)
    {
        ParryFeedback pFeedback = parryFeedback[currentParryFeedback];


        IncreasePool(ref currentParryFeedback, pfPoolSize);

        pFeedback.gameObject.SetActive(true);
        pFeedback.transform.position = position;
        pFeedback.StartFeedback(feedback);

        StartCoroutine(ResetEffect(pFeedback.gameObject, pFeedback.Duration()));
    }

    private IEnumerator ResetEffect(ParticleSystem effectToReset)
    {

        yield return new WaitForSeconds(effectToReset.duration);
        effectToReset.gameObject.SetActive(false);
        effectToReset.transform.parent = ParentManager.instance.effects;
    }
    private IEnumerator ResetEffect(GameObject go, float duration)
    {

        yield return new WaitForSeconds(duration);
        go.SetActive(false);
        go.transform.parent = ParentManager.instance.effects;
    }

    private void IncreasePool(ref int current, int poolSize)
    {
        if (current < poolSize - 1)
        {
            current++;
        }
        else
        {
            current = 0;
        }
    }




}
