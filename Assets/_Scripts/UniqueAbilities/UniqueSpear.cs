using UnityEngine;

public class UniqueSpear : UniqueAbility
{
    public override void ExecuteAbility(PlayerAttack playerAttack)
    {
        Debug.Log(playerAttack.transform.name + "Spear");
    }
}
