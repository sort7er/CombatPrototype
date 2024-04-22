using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Values")]
    public float mouseSensitivity = 100f;
    public float cameraInterpolation = 50;

    [Header("References")]
    public Transform camTrans;
    public Transform camTarget;

    public bool canRotate { get; private set; }

    private bool followMouse;

    private Player player;
    private Vector2 input;
    private float xRotation = 0f;


    void Awake()
    {
        player = GetComponent<Player>();
        FollowMouse();
        canRotate = true;
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if(followMouse)
        {
            float mouseY = input.y * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);


            if (canRotate)
            {
                float mouseX = input.x * mouseSensitivity * Time.deltaTime;

                player.SetRotateDirection(Vector3.up * mouseX);

                Quaternion targetRotation = Quaternion.Euler(xRotation, camTarget.eulerAngles.y, 0);

                camTrans.rotation = Quaternion.Slerp(camTrans.rotation, targetRotation, Time.deltaTime * cameraInterpolation);

                Quaternion rotationToReset = Quaternion.Euler(camTrans.eulerAngles.x, camTrans.eulerAngles.y, 0);
                camTrans.rotation = rotationToReset;
            }

            

            camTrans.position = Vector3.Lerp(camTrans.position, camTarget.position, Time.deltaTime * cameraInterpolation);
        }


    }


    public void DisableRotation()
    {
        canRotate = false;

        player.SetRotateDirection(Vector3.zero);
    }
    public void EnableRotation()
    {
        canRotate = true;
    }

    public void LookAt(Vector3 target, float duration)
    {
        //Vector3 lookDir = target - camTrans.position;
        //Quaternion look = Quaternion.LookRotation(lookDir, Vector3.up);
        //Quaternion newLook = Quaternion.Euler(look.eulerAngles.x, 0, 0);

        camTrans.DOLookAt(target, duration).SetEase(Ease.OutSine).OnComplete(LookAtDone);
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
    public void FollowMouse()
    {
        followMouse = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void DontFollowMouse()
    {
        followMouse = false;
        Cursor.lockState = CursorLockMode.None;
    }


}
