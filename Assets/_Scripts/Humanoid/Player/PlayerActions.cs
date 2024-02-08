using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private float parryWindow = 0.1f;

    private InputReader inputReader;

    private void Awake()
    {
        inputReader = GetComponent<Player>().inputReader;

        inputReader.OnAttack += Attack;
        inputReader.OnUnique += Unique;
        inputReader.OnBlock += Block;
        inputReader.OnParry += Parry;
    }

    private void OnDestroy()
    {
        inputReader.OnAttack -= Attack;
        inputReader.OnUnique -= Unique;
        inputReader.OnBlock -= Block;
        inputReader.OnParry -= Parry;
    }
    private void NewArchetype()
    {

    }

    private void Attack()
    {

    }

    private void Unique()
    {


    }
    private void Block()
    {

    }

    private void Parry()
    {

    }
}
