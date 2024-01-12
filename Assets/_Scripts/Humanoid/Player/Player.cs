using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public CameraController cameraController;
    public WeaponSelector weaponSelector;
    public PlayerActions playerAttack;
    public TargetAssistance targetAssistance;
    public Health health;

    public Vector3 Position()
    {
        return transform.position;
    }
}
