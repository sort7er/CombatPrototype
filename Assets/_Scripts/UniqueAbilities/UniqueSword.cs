using UnityEngine;

public class UniqueSword : UniqueAbility
{
    public override void ExecuteAbility(PlayerAttack playerAttack)
    {
        Debug.Log(playerAttack.transform.name + "Sword");
    }
}
