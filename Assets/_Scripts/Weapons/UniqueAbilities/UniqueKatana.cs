using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using EnemyAI;
public class UniqueKatana : UniqueAbility
{
    [Header("Rotation")]
    private float rotationDuration = 0.35f;


    [Header("Dash")]
    private float dashDuration = 0.2f;

    private Vector3 directionToTarget;
    private Vector3 dashPos;

    public override void ExecuteAbility(Player player, List<Enemy> enemies)
    {
        base.ExecuteAbility(player, enemies);
        enemyTrans = enemies[0].transform;
        targetPosition= enemyTrans.position;
        player.DisableMovement();
        camController.DisableRotation();

        Vector3 compensatedLookAt = new Vector3(enemyTrans.position.x, playerTrans.position.y, enemyTrans.position.z);
        playerTrans.DOLookAt(compensatedLookAt, rotationDuration);

        LookAtTarget(rotationDuration);

        player.InvokeMethod(SetDirection, rotationDuration * 3);
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

        player.InvokeMethod(SetDirectionNoTarget, rotationDuration * 3);
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

        Vector3 compensatedLookAt = new Vector3(dashPos.x, playerTrans.position.y, dashPos.z) + player.transform.forward;
        playerTrans.DOLookAt(compensatedLookAt, dashDuration * 0.5f);

        LookAtTarget(dashDuration * 0.5f);

        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.DOMove(dashPos, dashDuration).OnComplete(EndDash);
    }

    private void EndDash()
    {
        player.EnableMovement();
        camController.EnableRotation();
    }
}
