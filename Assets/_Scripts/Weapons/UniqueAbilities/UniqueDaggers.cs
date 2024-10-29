using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using EnemyAI;
using TMPro;

public class UniqueDaggers : UniqueAbility
{

    [Header("Rotation")]
    private float rotationDuration = 0.45f;

    [Header("Dash")]
    private float dashDuration = 0.35f;
    private float jumpPower = 2;

    private Vector3 directionToTarget;
    private Vector3 dashPos;

    public override void ExecuteAbility(Player player, List<Enemy> enemies)
    {
        base.ExecuteAbility(player, enemies);
        enemyTrans = enemies[0].transform;
        targetPosition = enemyTrans.position;
        player.DisableMovement();
        camController.DisableRotation();

        Vector3 compensatedLookAt = new Vector3(targetPosition.x, playerTrans.position.y, targetPosition.z);
        enemies[0].Takedown(player);
        playerTrans.DOLookAt(compensatedLookAt, rotationDuration).OnComplete(SetDirection);
        LookAtTarget(rotationDuration);
    }
    private void SetDirection()
    {
        targetPosition = enemyTrans.position;
        StartDash();
    }
    public override void ExecuteAbilityNoTarget(Player player)
    {
        base.ExecuteAbilityNoTarget(player);
        player.DisableMovement();
        player.InvokeMethod(SetDirectionNoTarget, rotationDuration);
    }
    private void SetDirectionNoTarget()
    {
        camController.DisableRotation();

        targetPosition = playerTrans.position + playerTrans.forward * 10;

        StartDash();
    }
    private void StartDash()
    {

        directionToTarget = targetPosition - playerTrans.position;
        dashPos = playerTrans.position + directionToTarget - directionToTarget.normalized * 1.5f;

        Vector3 compensatedLookAt = new Vector3(dashPos.x, playerTrans.position.y, dashPos.z);
        playerTrans.DOLookAt(compensatedLookAt, dashDuration * 0.5f);

        LookAtTarget(dashDuration * 0.5f);

        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.DOJump(dashPos, jumpPower, 1, dashDuration).OnComplete(EndDash);
    }
    private void EndDash()
    {
        player.EnableMovement();
        camController.EnableRotation();
    }

}
