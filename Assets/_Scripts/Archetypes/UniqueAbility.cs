using UnityEngine;

public abstract class UniqueAbility : MonoBehaviour
{
    protected Transform playerTrans;
    protected Rigidbody rb;
    protected PlayerMovement playerMovement;
    protected CameraController cameraController;

    public abstract void ExecuteAbility(PlayerData playerData, Vector3 target);
    public abstract void ExecuteAbilityNoTarget(PlayerData playerData);

    protected virtual void CheckData(PlayerData playerData)
    {
        if (rb == null)
        {
            StoreData(playerData);
        }
    }

    protected virtual void StoreData(PlayerData playerData)
    {
        playerTrans = playerData.transform;
        rb = playerData.rb;
        playerMovement = playerData.playerMovement;
        cameraController = playerData.cameraController;
    }
}
