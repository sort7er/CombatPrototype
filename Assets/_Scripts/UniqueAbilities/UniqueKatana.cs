using UnityEngine;

public class UniqueKatana : UniqueAbility
{
    public override void ExecuteAbility(PlayerAttack playerAttack)
    {
        Debug.Log(playerAttack.transform.name + "Katana");
    }
}
