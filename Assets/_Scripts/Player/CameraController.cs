using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class CameraController : MonoBehaviour
{
    [Header("Values")]
    public float mouseSensitivity = 100f;

    [Header("References")]
    public Transform playerCam;

    public bool canRotate { get; private set; }
    
    private Vector2 input;
    private float xRotation = 0f;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        float mouseX = input.x * mouseSensitivity * Time.deltaTime;
        float mouseY = input.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.localRotation = Quaternion.Euler(xRotation, 0, 0);



        if(canRotate)
        {
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    public void DisableRotation()
    {
        canRotate = false;
    }
    public void EnableRotation()
    {
        canRotate = true;
    }


}
