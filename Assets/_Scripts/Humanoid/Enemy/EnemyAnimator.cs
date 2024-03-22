using EnemyAI;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    public float strafeThreshold = 0.25f;
    public Enemy enemy;
    public Animator animator { get; private set; }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        float dot = enemy.CalculateDotProduct();

        Debug.Log(dot);

        animator.SetFloat("MovementX", dot);

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

}
