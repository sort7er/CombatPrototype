using System.Collections.Generic;
using UnityEngine;

public abstract class UniqueAbility : MonoBehaviour
{
    protected Transform playerTrans;
    protected Rigidbody rb;
    protected PlayerMovement playerMovement;
    protected CameraController cameraController;

    protected float range;
    protected float idealDot;
    protected float acceptedDot;

    protected List<Enemy> enemies;


    public virtual void ExecuteAbility(PlayerData playerData, TargetAssistanceParams targetAssistanceParams, List<Enemy> enemies)
    {
        StorePlayerData(playerData);
        StoreOtherData(targetAssistanceParams, enemies);
    }
    public virtual void ExecuteAbilityNoTarget(PlayerData playerData)
    {
        StorePlayerData(playerData);
    }


    private void StorePlayerData(PlayerData playerData)
    {
        playerTrans = playerData.transform;
        rb = playerData.rb;
        playerMovement = playerData.playerMovement;
        cameraController = playerData.cameraController;
    }
    private void StoreOtherData(TargetAssistanceParams targetAssistanceParams, List<Enemy> enemies)
    {
        range = targetAssistanceParams.range;
        idealDot = targetAssistanceParams.idealDotProduct;
        acceptedDot = targetAssistanceParams.acceptedDotProduct;
        this.enemies = enemies;
    }
}

