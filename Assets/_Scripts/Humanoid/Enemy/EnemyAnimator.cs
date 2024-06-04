using EnemyAI;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    public float strafeThreshold = 0.25f;
    public Enemy enemy;
    public Animator animator;

    private float lerp;

    private bool parry;
    private bool perfectParry;
    private bool tooLate;
    private float parryTime;
    private float perfectParryTime;
    private float tooLateTime;


    private void Update()
    {
        float dot = enemy.CalculateDotProduct();

        lerp = Mathf.Lerp(lerp, dot, Time.deltaTime * 5);

        animator.SetFloat("MovementX", lerp);


        if(parry)
        {
            parryTime += Time.deltaTime;
        }
        if (perfectParry)
        {
            perfectParryTime += Time.deltaTime;
        }
        if(tooLate)
        {
            tooLateTime += Time.deltaTime;
        }
    }

    public void SetWalking(bool isWalking)
    {
        if(animator == null)
        {
            Debug.Log(name);
        }

        animator.SetBool("Walking", isWalking);
    }
    public void SetAnimatorBool(string name, bool state)
    {
        animator.SetBool(name, state);
    }


    //Events in order
    public void Parry()
    {
        parry = true;
    }
    public void PerfectParry()
    {
        perfectParry = true;
    }
    public void AttackEffect()
    {
        //tooLate= true;
        if (enemy.currentWeapon.CurrentAttackExists())
        {
            enemy.currentWeapon.Effect();
        }
    }
    public void AttackEvent()
    {

        perfectParry = false;
        parry = false;
        tooLate = false;

        if (enemy.currentWeapon.CurrentAttackExists())
        {
            enemy.SetAttackData(parryTime, perfectParryTime, tooLateTime);
            enemy.OverlapCollider();
            enemy.hitBox.OverlapCollider();
        }

        parryTime = 0;
        tooLateTime = 0;
        perfectParryTime = 0;
    }
    public void AttackDone()
    {
        enemy.currentWeapon.AttackDone();
    }

}
