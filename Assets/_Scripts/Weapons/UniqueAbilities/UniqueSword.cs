using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class UniqueSword : UniqueAbility
{

    [Header("Rotation")]
    private float rotationDuration = 0.3f;

    [Header("Dash")]
    private float dashDuration = 0.5f;
    private float jumpPower = 2;

    private Vector3 directionToTarget;
    private Vector3 dashPos;
    private Vector3 target;



    public override void ExecuteAbility(Player player, List<Enemy> enemies)
    {
        base.ExecuteAbility(player, enemies);
        target = enemies[0].transform.position;
        player.DisableMovement();
        camController.DisableRotation();

        Vector3 compensatedLookAt = new Vector3(target.x, playerTrans.position.y, target.z);
        playerTrans.DOLookAt(compensatedLookAt, rotationDuration);
        Invoke(nameof(StartDash), rotationDuration);
    }

    public override void ExecuteAbilityNoTarget(Player player)
    {
        base.ExecuteAbilityNoTarget(player);
        player.DisableMovement();
        Invoke(nameof(StartDashNoTarget), rotationDuration);
    }
    private void StartDashNoTarget()
    {
        camController.DisableRotation();

        target = playerTrans.position + playerTrans.forward * 10;

        StartDash();
    }
    private void StartDash()
    {
        directionToTarget = target - playerTrans.position;
        dashPos = playerTrans.position + directionToTarget - directionToTarget.normalized;

        Vector3 compensatedPos = new Vector3(target.x, playerTrans.position.y, target.z);
        playerTrans.DOLookAt(compensatedPos, rotationDuration);

        camController.LookAt(compensatedPos, rotationDuration);

        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.DOJump(dashPos, jumpPower, 1, dashDuration).OnComplete(EndDash);
    }
    private void EndDash()
    {
        player.EnableMovement();
        camController.EnableRotation();
    }
}
