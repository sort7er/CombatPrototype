using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using EnemyAI;
public class UniqueGloves : UniqueAbility
{
    [Header("Rotation")]
    private float rotationDuration = 0.3f;

    [Header("Dash")]
    private float jumpDuration = 0.4f;
    private float jumpPower = 1;

    private Vector3 target;
    public override void ExecuteAbility(Player player, List<Enemy> enemies)
    {
        base.ExecuteAbility(player, enemies);
        target = enemies[0].transform.position;
        player.DisableMovement();
        camController.DisableRotation();

        Vector3 compensatedLookAt = new Vector3(target.x, playerTrans.position.y, target.z);
        playerTrans.DOLookAt(compensatedLookAt, rotationDuration);
        Invoke(nameof(StartUppercutWithEnemies), rotationDuration);

    }

    private void StartUppercutWithEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Vector3 forwardDir = (enemies[i].transform.position - playerTrans.transform.position).normalized;
            enemies[i].AddForce(Vector3.up * 10 + forwardDir * 2);
        }
        StartUppercut();

    }
    public override void ExecuteAbilityNoTarget(Player player)
    {
        base.ExecuteAbilityNoTarget(player);
        player.DisableMovement();
        Invoke(nameof(StartUppercutNoTarget), rotationDuration);
    }
    private void StartUppercutNoTarget()
    {
        camController.DisableRotation();
        target = playerTrans.position + playerTrans.forward * 10;

        StartUppercut();
    }
    private void StartUppercut()
    {
        Vector3 jumpPos = playerTrans.transform.position + playerTrans.transform.up * 1.5f;

        Vector3 compensatedLookAt = new Vector3(target.x, playerTrans.position.y + 2.1f, target.z);
        camController.LookAt(compensatedLookAt, jumpDuration * 0.5f);

        rb.velocity = Vector3.zero;

        rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        Invoke(nameof(EndUppercut), jumpDuration);

    }
    private void EndUppercut()
    {
        player.EnableMovement();
        camController.EnableRotation();
    }
}
