using DG.Tweening;
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

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        canRotate = true;
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    private void Update()
    {

        float mouseY = input.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.localRotation = Quaternion.Euler(xRotation, 0, 0);

        if(canRotate)
        {
            float mouseX = input.x * mouseSensitivity * Time.deltaTime;
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

    public void LookAt(Vector3 target, float duration)
    {
        Vector3 lookDir = target - playerCam.position;
        Quaternion look = Quaternion.LookRotation(lookDir, Vector3.up);
        Quaternion newLook = Quaternion.Euler(look.eulerAngles.x, 0, 0);

        playerCam.DOLocalRotateQuaternion(newLook, duration).SetEase(Ease.OutSine).OnComplete(LookAtDone);
    }
    private void LookAtDone()
    {
        Debug.Log(playerCam.transform.eulerAngles);
        xRotation = playerCam.transform.eulerAngles.x;
        if(xRotation > 90)
        {
            xRotation -= 360;
        }
        Debug.Log(playerCam.transform.eulerAngles);
    }


}
