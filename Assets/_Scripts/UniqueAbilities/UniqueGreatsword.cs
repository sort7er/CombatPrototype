using UnityEngine;

public class UniqueGreatsword : UniqueAbility
{
    public override void ExecuteAbility(PlayerAttack playerAttack)
    {
        Debug.Log(playerAttack.transform.name + "Greatsword");
    }
}
