using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Values")]
    public float mouseSensitivity = 100f;

    [Header("References")]
    public Camera playerCam;
    public Transform camTrans;

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

        camTrans.localRotation = Quaternion.Euler(xRotation, 0, 0);

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
        Vector3 lookDir = target - camTrans.position;
        Quaternion look = Quaternion.LookRotation(lookDir, Vector3.up);
        Quaternion newLook = Quaternion.Euler(look.eulerAngles.x, 0, 0);

        camTrans.DOLocalRotateQuaternion(newLook, duration).SetEase(Ease.OutSine).OnComplete(LookAtDone);
    }
    public void LookAtAngle(float angle, float duration)
    {
        camTrans.DOLocalRotate(new Vector3(angle, 0, 0), duration).SetEase(Ease.InCirc).OnComplete(LookAtDone);
    }
    private void LookAtDone()
    {
        xRotation = camTrans.transform.eulerAngles.x;
        if(xRotation > 90)
        {
            xRotation -= 360;
        }
    }
    public Vector3 CameraPosition()
    {
        return camTrans.position;
    }
    public Quaternion CameraRotation()
    {
        return camTrans.rotation;
    }


}
