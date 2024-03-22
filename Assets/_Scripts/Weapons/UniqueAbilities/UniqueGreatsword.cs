using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using EnemyAI;

public class UniqueGreatsword : UniqueAbility
{
    [Header("Rotation")]
    private float rotationDuration = 0.8f;
    private float attackDuration = 1.3f;

    private Vector3 target;

    public override void ExecuteAbility(Player player, List<Enemy> enemies)
    {
        base.ExecuteAbility(player, enemies);
        target = enemies[0].transform.position;
        player.DisableMovement();
        camController.DisableRotation();

        Vector3 compensatedLookAt = new Vector3(target.x, playerTrans.position.y, target.z);
        playerTrans.DOLookAt(compensatedLookAt, rotationDuration);

        Vector3 compensatedCamLookAt = new Vector3(target.x, target.y + 1.5f, target.z);
        camController.LookAt(compensatedCamLookAt, rotationDuration * 0.5f);

        Invoke(nameof(EndDash), attackDuration);
    }

    public override void ExecuteAbilityNoTarget(Player player)
    {
        base.ExecuteAbilityNoTarget(player);
        player.DisableMovement();
        Invoke(nameof(EndDash), attackDuration);
    }
    private void EndDash()
    {
        player.EnableMovement();
        camController.EnableRotation();
    }
}
