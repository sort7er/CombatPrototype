using UnityEngine;

public class UniqueGloves : UniqueAbility
{
    public override void ExecuteAbility(PlayerAttack playerAttack)
    {
        Debug.Log(playerAttack.transform.name + "Gloves");
    }
}
