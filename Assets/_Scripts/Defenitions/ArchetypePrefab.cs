using System.Collections.Generic;
using UnityEngine;

public class ArchetypePrefab : MonoBehaviour
{
    public Animator archetypeAnim { get; private set; }


    public enum AttackType
    {
        light,
        heavy,
        unique
    }

    public List<AttackType> attackQueue = new();

    private int queueCapasity = 2;
    private bool isAttacking;


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

    //Checking if there is a queue
    private void CheckAttack(AttackType attackType)
    {
        if (isAttacking)
        {
            //If is currently attacking, try and add this attack to queue
            TryAddToQueue(attackType);
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
        if(attackQueue.Count > 0)
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
            Debug.Log("Unique");
        }
    }
}
