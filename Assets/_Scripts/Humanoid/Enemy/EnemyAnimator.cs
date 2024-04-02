using EnemyAI;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    public float strafeThreshold = 0.25f;
    public Enemy enemy;
    public Animator animator { get; private set; }

    private float lerp;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        float dot = enemy.CalculateDotProduct();

        lerp = Mathf.Lerp(lerp, dot, Time.deltaTime * 5);

        animator.SetFloat("MovementX", lerp);

        //if (dot + strafeThreshold < 0)
        //{
        //    Debug.Log("Right");
        //}
        //else if (dot - strafeThreshold > 0)
        //{
        //    Debug.Log("Left");
        //}
        //else
        //{
        //    Debug.Log("Forward");
        //}

    }

    public void SetWalking(bool isWalking)
    {
        animator.SetBool("Walking", isWalking);
    }
    public void AttackEvent()
    {
        enemy.hitbox.OverlapCollider();
    }
    public void AttackEffect()
    {
        enemy.currentWeapon.Effect();
    }
}
