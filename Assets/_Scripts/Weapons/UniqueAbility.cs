using System.Collections.Generic;
using UnityEngine;

public abstract class UniqueAbility: MonoBehaviour
{
    [Header("Target assistance paramaters")]
    public float range = 10f;
    public float idealDotProduct = 0.85f;
    public float acceptedDotProduct = 0.75f;


    protected Player player;
    protected Rigidbody rb;
    protected Transform playerTrans;
    protected CameraController camController;
    protected List<Enemy> enemies;

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
}