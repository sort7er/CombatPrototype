using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class UniqueGreatsword : UniqueAbility
{
    [Header("Rotation")]
    private float rotationDuration = 0.8f;
    private float attackDuration = 1.3f;

    private Vector3 target;

    public override void ExecuteAbility(PlayerData playerData, TargetAssistanceParams targetAssistanceParams, List<Enemy> enemies)
    {
        base.ExecuteAbility(playerData, targetAssistanceParams, enemies);
        target = enemies[0].transform.position;
        playerMovement.DisableMovement();
        cameraController.DisableRotation();

        Vector3 compensatedLookAt = new Vector3(target.x, playerTrans.position.y, target.z);
        playerTrans.DOLookAt(compensatedLookAt, rotationDuration);

        Vector3 compensatedCamLookAt = new Vector3(target.x, target.y + 1.5f, target.z);
        cameraController.LookAt(compensatedCamLookAt, rotationDuration * 0.5f);

        Invoke(nameof(EndDash), attackDuration);
    }

    public override void ExecuteAbilityNoTarget(PlayerData playerData)
    {
        base.ExecuteAbilityNoTarget(playerData);
        playerMovement.DisableMovement();
        Invoke(nameof(EndDash), attackDuration);
    }
    private void EndDash()
    {
        playerMovement.EnableMovement();
        cameraController.EnableRotation();
    }
}
