using System.Collections.Generic;
using UnityEngine;

public class ArchetypePrefab : MonoBehaviour
{
    public Animator archetypeAnim { get; private set; }


    private enum AttackType
    {
        light,
        heavy,
        unique
    }

    private List<AttackType> attackQueue = new();

    private int queueCapasity = 1;

    private void Awake()
    {
        archetypeAnim = GetComponent<Animator>();
    }

    public void Fire()
    {
        CheckAttack(AttackType.light);
    }
    public void HeavyFire()
    {
        CheckAttack(AttackType.heavy);
    }
    
    private void CheckAttack(AttackType attackType)
    {
        if(attackQueue.Count > 0)
        {
            //Add to queue
        }
        else
        {
            Attack(attackType);
        }
    }

    private void AddToQueue(AttackType attackType)
    {
        //if(attackQueue.Count < queueCapasity)
    }

    public void CheckQueue()
    {
        // Attack if queue is not empty
        if(attackQueue.Count > 0)
        {
            Attack(attackQueue[0]);
            attackQueue.RemoveAt(0);
        }
    }

    private void Attack(AttackType attackType)
    {
        if(attackType == AttackType.light)
        {
            archetypeAnim.SetTrigger("Fire");
        }
        else if(attackType == AttackType.heavy)
        {
            archetypeAnim.SetTrigger("HeavyFire");
        }
        else if (attackType == AttackType.unique)
        {
            Debug.Log("Unique");
        }
    }

    public void StopFire()
    {
        archetypeAnim.ResetTrigger("Fire");
        archetypeAnim.ResetTrigger("HeavyFire");
    }

}
