using EnemyAI;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : Ability
{
    private Transform[] oldParents;
    private Transform[] abilityTransforms;
    public override void ExecuteAbility(Player player, List<Enemy> enemies)
    {
        base.ExecuteAbility(player, enemies);
        Common();

    }
    public override void ExecuteAbilityNoTarget(Player player)
    {
        base.ExecuteAbilityNoTarget(player);
        Common();
    }

    private void Common()
    {
        abilityTransforms = player.abilityTransforms;

        oldParents = player.currentWeapon.SetParentForModels(player.abilityTransforms);
        player.InvokeMethod(MaestroDone, 0.5f);
        for (int i = 0; i < player.currentWeapon.weaponModel.Length; i++)
        {
            abilityTransforms[i].position = player.currentWeapon.weaponModel[i].Position();
            abilityTransforms[i].parent = null;
        }
    }

    private void MaestroDone()
    {
        player.currentWeapon.SetParentForModels(oldParents);
        for (int i = 0; i < abilityTransforms.Length; i++)
        {
            abilityTransforms[i].parent = player.transform;
        }
    }



}
