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

    public virtual void ExecuteAbility(Player player, List<Enemy> enemies)
    {
        SetPlayer(player);
    }
    public virtual void ExecuteAbilityNoTarget(Player player)
    {
        SetPlayer(player);
    }
    public void SetPlayer(Player player)
    {
        if (this.player == null)
        {
            this.player = player;
            playerTrans = player.transform;
            camController = player.cameraController;
            rb = player.rb;
        }
    }
}