using System.Collections.Generic;
using UnityEngine;

public abstract class UniqueAbility : MonoBehaviour
{
    [Header("Target assistance paramaters")]
    public float range = 10f;
    public float idealDotProduct = 0.85f;
    public float acceptedDotProduct = 0.75f;


    protected Transform playerTrans;
    protected Rigidbody rb;
    protected Humanoid owner;
    protected CameraController cameraController;

    //protected List<Enemy> enemies;


    //public virtual void ExecuteAbility(PlayerData playerData, TargetAssistanceParams targetAssistanceParams, List<Enemy> enemies)
    //{
    //    StorePlayerData(playerData);
    //    StoreOtherData(targetAssistanceParams, enemies);
    //}
    //public virtual void ExecuteAbilityNoTarget(PlayerData playerData)
    //{
    //    StorePlayerData(playerData);
    //}


    //private void StorePlayerData(PlayerData playerData)
    //{
    //    playerTrans = playerData.transform;
    //    rb = playerData.rb;
    //    playerMovement = playerData.playerMovement;
    //    cameraController = playerData.cameraController;
    //}
    //private void StoreOtherData(TargetAssistanceParams targetAssistanceParams, List<Enemy> enemies)
    //{
    //    range = targetAssistanceParams.range;
    //    idealDot = targetAssistanceParams.idealDotProduct;
    //    acceptedDot = targetAssistanceParams.acceptedDotProduct;
    //    this.enemies = enemies;
    //}
}