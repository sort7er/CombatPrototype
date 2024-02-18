using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    [Header("Hit effect")]
    public int poolSize = 10;
    public ParticleSystem hitEffect;

    [Header("Anticipation effect")]
    public int aPoolSize = 5;
    public ParticleSystem anticipationEffect;
    
    [Header("Parry effect")]
    public ParticleSystem parryEffect;

    [Header("Slice effect")]
    public int sPoolSize = 10;
    public ParticleSystem slashEffect;

    [Header("Slice effect")]
    public int tPoolSize = 10;
    public ParticleSystem thrustEffect;

    private ParticleSystem[] hit;
    private int currentHit;

    private ParticleSystem[] anticipation;
    private int currentAnticipation;

    private ParticleSystem[] slash;
    private int currentSlash;

    private ParticleSystem[] thrust;
    private int currentThrust;

    private ParticleSystem parry;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetUpHitEffect();
        SetUpSlashEffect();
        SetUpAnticipationEffect();
        SetUpParryEffect();
        SetUpThrustEffect();
    }

    private void SetUpHitEffect()
    {
        hit = new ParticleSystem[poolSize];
        currentHit = 0;

        for (int i = 0; i < poolSize; i++)
        {
            hit[i] = Instantiate(hitEffect, ParentManager.instance.effects);
            hit[i].gameObject.SetActive(false);
        }
    }
    private void SetUpSlashEffect()
    {
        slash = new ParticleSystem[sPoolSize];
        currentSlash = 0;

        for (int i = 0; i < sPoolSize; i++)
        {
            slash[i] = Instantiate(slashEffect, ParentManager.instance.effects);
            slash[i].gameObject.SetActive(false);
        }
    }
    private void SetUpThrustEffect()
    {
        thrust = new ParticleSystem[tPoolSize];
        currentThrust = 0;

        for (int i = 0; i < tPoolSize; i++)
        {
            thrust[i] = Instantiate(thrustEffect, ParentManager.instance.effects);
            thrust[i].gameObject.SetActive(false);
        }
    }
    private void SetUpAnticipationEffect()
    {
        anticipation = new ParticleSystem[aPoolSize];
        currentAnticipation = 0;

        for (int i = 0; i < aPoolSize; i++)
        {
            anticipation[i] = Instantiate(anticipationEffect, ParentManager.instance.effects);
            anticipation[i].gameObject.SetActive(false);
        }
    }
    private void SetUpParryEffect()
    {
        parry = Instantiate(parryEffect, ParentManager.instance.effects);
        parry.gameObject.SetActive(false);
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
    public void Slash(Vector3 position, Vector3 direction, Vector3 upDirection, Transform parent)
    {
        ParticleSystem effect = slash[currentSlash];

        effect.gameObject.SetActive(true);

        effect.transform.parent = parent;

        effect.transform.position = position;
        effect.transform.rotation = Quaternion.LookRotation(direction, upDirection);

        StartCoroutine(ResetEffect(effect));


        if (currentSlash < sPoolSize - 1)
        {
            currentSlash++;
        }
        else
        {
            currentSlash = 0;
        }

    }
    public void Thrust(Vector3 position, Vector3 direction, Vector3 upDirection, Transform parent)
    {
        ParticleSystem effect = thrust[currentThrust];

        effect.gameObject.SetActive(true);

        effect.transform.parent = parent;

        effect.transform.position = position;
        effect.transform.rotation = Quaternion.LookRotation(direction, upDirection);

        StartCoroutine(ResetEffect(effect));


        if (currentThrust < tPoolSize - 1)
        {
            currentThrust++;
        }
        else
        {
            currentThrust = 0;
        }

    }
    public void Anticipation(Vector3 position)
    {
        ParticleSystem effect = anticipation[currentAnticipation];

        effect.gameObject.SetActive(true);

        effect.transform.position = position;

        StartCoroutine(ResetEffect(effect));


        if (currentAnticipation < aPoolSize - 1)
        {
            currentAnticipation++;
        }
        else
        {
            currentAnticipation = 0;
        }

    }
    public void Parry(Vector3 position)
    {
        parry.transform.position = position;
        parry.gameObject.SetActive(true);
        StartCoroutine(ResetEffect(parry));
    }

    private IEnumerator ResetEffect(ParticleSystem effectToReset)
    {

        yield return new WaitForSeconds(effectToReset.startLifetime);
        effectToReset.gameObject.SetActive(false);
        effectToReset.transform.parent = ParentManager.instance.effects;
    }




}
