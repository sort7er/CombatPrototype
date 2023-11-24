using UnityEngine;

public class UniqueGloves : UniqueAbility
{
    public override void ExecuteAbility(PlayerData playerData, Vector3 target)
    {

    }

    public override void ExecuteAbilityNoTarget(PlayerData playerData)
    {
        Debug.Log(playerData.transform.name + "Gloves");
    }
}
