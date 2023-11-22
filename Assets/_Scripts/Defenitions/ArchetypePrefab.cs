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

    private int queueCapasity = 2;
    public bool isAttacking { get; private set; }


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
    public void UniqueFire()
    {
        CheckAttack(AttackType.unique);
    }

    //Temporary fuction until weapon switching is proparly made
    public void Abort()
    {
        ResetTriggers();
        attackQueue.Clear();
        isAttacking = false;
    }
    private void ResetTriggers()
    {
        archetypeAnim.ResetTrigger("Fire");
        archetypeAnim.ResetTrigger("HeavyFire");
    }

    //Checking if there is a queue
    private void CheckAttack(AttackType attackType)
    {
        if (isAttacking)
        {
            //If is currently attacking, try and add this attack to queue. Cannot queue unique attack
            if(attackType != AttackType.unique)
            {
                TryAddToQueue(attackType);
            }

        }
        else
        {
            //If not currently attacking
            Attack(attackType);
        }
    }

    private void TryAddToQueue(AttackType attackType)
    {
        // if there is capasity in the queue, add the new attack
        if (attackQueue.Count < queueCapasity)
        {
            attackQueue.Add(attackType);
        }
    }

    //This is called from the animation, to see if it should chain into a new attack
    public void AttackDoneAndCheckQueue()
    {
        isAttacking = false;
        // Attack if queue is not empty
        if (attackQueue.Count > 0)
        {
            Attack(attackQueue[0]);
            attackQueue.RemoveAt(0);
        }
    }
    //This is called from the animation, used at end of combo
    public void AttackDone()
    {
        isAttacking = false;
        attackQueue.Clear();
    }

    private void Attack(AttackType attackType)
    {
        isAttacking = true;
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
            ResetTriggers();
            archetypeAnim.Play("UniqueFire");
        }
    }
}
