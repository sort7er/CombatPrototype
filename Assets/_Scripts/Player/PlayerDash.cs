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
    private Vector3 target;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraController = GetComponent<CameraController>();
    }

    public void DashForward(Vector3 targetPos)
    {
        target = targetPos;

        playerMovement.DisableMovement();
        cameraController.DisableRotation();

        Vector3 compensatedLookAt = new Vector3(target.x, transform.position.y, target.z);
        transform.DOLookAt(compensatedLookAt, rotationDuration);
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

        target = transform.position + transform.forward * 10;

        StartDash();
    }

    private void StartDash()
    {
        directionToTarget = target - transform.position;
        dashPos = transform.position + directionToTarget - directionToTarget.normalized;

        Vector3 compensatedLookAt = new Vector3(dashPos.x, transform.position.y, dashPos.z);
        transform.DOLookAt(compensatedLookAt, dashDuration * 0.5f);

        rb.velocity = Vector3.zero;
        rb.DOJump(dashPos, jumpPower, 1, dashDuration).OnComplete(EndDash);
    }
    public void EndDash()
    {
        playerMovement.EnableMovement();
        cameraController.EnableRotation();
    }

}
