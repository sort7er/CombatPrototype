using UnityEngine;

public class PlayerActions : MonoBehaviour
{
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
        Debug.Log("Attack");
    }

    private void Unique()
    {
        Debug.Log("Unique");
    }
    private void Block()
    {
        Debug.Log("Block");
    }

    private void Parry()
    {
        Debug.Log("Parry");
    }
}
