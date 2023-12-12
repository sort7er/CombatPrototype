using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class UniqueGloves : UniqueAbility
{
    [Header("Rotation")]
    private float rotationDuration = 0.2f;

    [Header("Dash")]
    private float jumpDuration = 0.4f;
    private float jumpPower = 1;

    private Vector3 target;
    public override void ExecuteAbility(PlayerData playerData, TargetAssistanceParams targetAssistanceParams, List<Enemy> enemies)
    {
        base.ExecuteAbility(playerData, targetAssistanceParams, enemies);
        target = enemies[0].transform.position;
        playerMovement.DisableMovement();
        cameraController.DisableRotation();

        Vector3 compensatedLookAt = new Vector3(target.x, playerTrans.position.y, target.z);
        playerTrans.DOLookAt(compensatedLookAt, rotationDuration);
        Invoke(nameof(StartUppercutWithEnemies), rotationDuration);

    }

    private void StartUppercutWithEnemies()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            Vector3 forwardDir = (enemies[i].transform.position - playerTrans.transform.position).normalized;
            enemies[i].AddForce(Vector3.up * 10 + forwardDir * 2);
        }
        StartUppercut();

    }
    public override void ExecuteAbilityNoTarget(PlayerData playerData)
    {
        base.ExecuteAbilityNoTarget(playerData);
        playerMovement.DisableMovement();
        Invoke(nameof(StartUppercutNoTarget), rotationDuration);
    }
    private void StartUppercutNoTarget()
    {
        cameraController.DisableRotation();
        target = playerTrans.position + playerTrans.forward * 10;

        StartUppercut();
    }
    private void StartUppercut()
    {
        Vector3 jumpPos = playerTrans.transform.position + playerTrans.transform.up * 1.5f;

        Vector3 compensatedLookAt = new Vector3(target.x, playerTrans.position.y + 2.1f, target.z);
        cameraController.LookAt(compensatedLookAt, jumpDuration * 0.5f);

        rb.velocity = Vector3.zero;

        rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        Invoke(nameof(EndUppercut), jumpDuration);

    }
    private void EndUppercut()
    {
        playerMovement.EnableMovement();
        cameraController.EnableRotation();
    }
}
