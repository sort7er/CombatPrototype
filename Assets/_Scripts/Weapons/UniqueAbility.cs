using System.Collections.Generic;
using UnityEngine;
using EnemyAI;
using TMPro;

public abstract class UniqueAbility
{
    public float range = 10f;
    public float idealDotProduct = 0.85f;
    public float acceptedDotProduct = 0.75f;


    protected Player player;
    protected Rigidbody rb;
    protected Transform playerTrans;
    protected CameraController camController;
    protected List<Enemy> enemies;

    protected Transform enemyTrans;
    protected Vector3 targetPosition;

    public virtual void SetParamaters()
    {

    }

    public virtual void ExecuteAbility(Player player, List<Enemy> enemies)
    {
        SetPlayer(player);
        SetEnemies(enemies);
    }
    public virtual void ExecuteAbilityNoTarget(Player player)
    {
        SetPlayer(player);
        NoEnemies();
    }
    private void SetPlayer(Player player)
    {
        if (this.player == null)
        {
            this.player = player;
            playerTrans = player.transform;
            camController = player.cameraController;
            rb = player.rb;
        }
    }
    private void SetEnemies(List<Enemy> enemies)
    {
        this.enemies = enemies;
    }
    private void NoEnemies()
    {
        if(enemies != null)
        {
            enemies.Clear();
        }
    }
    protected void LookAtTarget(float duration)
    {
        Vector3 compensatedCamLookAt = new Vector3(targetPosition.x, targetPosition.y + 1.3f, targetPosition.z);
        camController.LookAt(compensatedCamLookAt, duration);
    }
}