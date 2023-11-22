using DG.Tweening;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{

    [Header("Rotation")]
    private float rotationDuration = 0.3f;

    [Header("Dash")]
    private float dashDuration = 0.5f;
    private float jumpPower = 2;


    private Rigidbody rb;
    private PlayerMovement playerMovement;
    private CameraController cameraController;

    private Vector3 directionToTarget;
    private Vector3 dashPos;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraController = GetComponent<CameraController>();
    }

    public void DashForward(Vector3 targetPos)
    {
        playerMovement.DisableMovement();
        cameraController.DisableRotation();

        Vector3 compensatedLookAt = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        transform.DOLookAt(compensatedLookAt, rotationDuration);

        directionToTarget = targetPos - transform.position;

        Invoke(nameof(StartDash), rotationDuration);
    }
    public void DashForward()
    {
        playerMovement.DisableMovement();
        Invoke(nameof(StartDashNoTarget), rotationDuration);
    }

    private void StartDashNoTarget()
    {
        cameraController.DisableRotation();

        Vector3 targetPos = transform.position + transform.forward * 10;

        directionToTarget = targetPos - transform.position;
        StartDash();
    }

    private void StartDash()
    {
        dashPos = transform.position + directionToTarget - directionToTarget.normalized;

        rb.velocity = Vector3.zero;
        rb.DOJump(dashPos, jumpPower, 1, dashDuration).OnComplete(EndDash);
    }
    public void EndDash()
    {
        playerMovement.EnableMovement();
        cameraController.EnableRotation();
    }

}
