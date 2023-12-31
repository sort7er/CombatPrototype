using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class UniqueSpear : UniqueAbility
{
    [Header("Rotation")]
    private float rotationDuration = 0.35f;


    [Header("Dash")]
    private float dashDuration = 0.2f;

    private Vector3 directionToTarget;
    private Vector3 dashPos;
    private Vector3 target;
    public override void ExecuteAbility(PlayerData playerData, TargetAssistanceParams targetAssistanceParams, List<Enemy> enemies)
    {
        base.ExecuteAbility(playerData, targetAssistanceParams, enemies);
        target = enemies[0].transform.position;
        playerMovement.DisableMovement();
        cameraController.DisableRotation();

        Vector3 compensatedLookAt = new Vector3(target.x, playerTrans.position.y, target.z);
        playerTrans.DOLookAt(compensatedLookAt, rotationDuration);
        Invoke(nameof(StartDash), rotationDuration);
    }

    public override void ExecuteAbilityNoTarget(PlayerData playerData)
    {
        base.ExecuteAbilityNoTarget(playerData);
        playerMovement.DisableMovement();
        Invoke(nameof(StartDashNoTarget), rotationDuration);
    }
    private void StartDashNoTarget()
    {
        cameraController.DisableRotation();

        target = playerTrans.position + playerTrans.forward * 10;

        StartDash();
    }

    private void StartDash()
    {
        directionToTarget = target - playerTrans.position;
        dashPos = playerTrans.position + directionToTarget - directionToTarget.normalized * 1.5f;

        Vector3 compensatedLookAt = new Vector3(dashPos.x, playerTrans.position.y, dashPos.z);
        playerTrans.DOLookAt(compensatedLookAt, dashDuration * 0.5f);

        Vector3 compensatedCamLookAt = new Vector3(target.x, target.y + 1.3f, target.z);
        cameraController.LookAt(compensatedCamLookAt, dashDuration * 0.5f);

        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.DOMove(dashPos, dashDuration).OnComplete(EndDash);
    }
    private void EndDash()
    {
        playerMovement.EnableMovement();
        cameraController.EnableRotation();
    }
}
