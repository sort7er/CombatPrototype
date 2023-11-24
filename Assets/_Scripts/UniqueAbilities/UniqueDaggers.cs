using UnityEngine;

public class UniqueDaggers : UniqueAbility
{
    public override void ExecuteAbility(PlayerAttack playerAttack)
    {
        Debug.Log(playerAttack.transform.name + "Daggers");
    }
}
