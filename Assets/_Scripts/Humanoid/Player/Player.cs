using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public CameraController cameraController;
    public WeaponSelector weaponSelector;
    public PlayerAttack playerAttack;
    public TargetAssistance targetAssistance;
    public Health health;

    public Vector3 Position()
    {
        return transform.position;
    }
}
