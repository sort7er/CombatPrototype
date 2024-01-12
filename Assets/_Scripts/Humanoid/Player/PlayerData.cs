using UnityEngine;

public class PlayerData
{
    //Scripts
    public Transform transform;
    public PlayerMovement playerMovement;
    public CameraController cameraController;
    public PlayerActions playerAttack;

    //Components
    public Rigidbody rb;

    public PlayerData(PlayerMovement playerMovement, CameraController cameraController, PlayerActions playerAttack, Rigidbody rb)
    {
        this.playerMovement = playerMovement;
        this.cameraController = cameraController;
        this.playerAttack = playerAttack;
        this.rb = rb;
        transform = rb.transform;
    }
}
