using UnityEngine;

public class PlayerDash : MonoBehaviour
{

    [Header("Values")]
    private float rotationDuration = 0.3f;


    private Rigidbody rb;
    private PlayerMovement playerMovement;
    private CameraController cameraController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraController = GetComponent<CameraController>();
    }

    public void DashForward(Transform target)
    {
        playerMovement.DisableMovement();
        cameraController.DisableRotation();
        

        //transform.

    }


}
