using DG.Tweening;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{

    [Header("Values")]
    private float rotationDuration = 0.3f;
    private float dashDuration = 0.3f;


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

    public void DashForward(Transform target)
    {
        playerMovement.DisableMovement();
        cameraController.DisableRotation();
        

        Vector3 compensatedLookAt = new Vector3(target.position.x, transform.position.y, target.position.z);

        transform.DOLookAt(compensatedLookAt, rotationDuration).OnComplete(StartDash);

        directionToTarget = target.position - transform.position;

        dashPos = transform.position + directionToTarget - directionToTarget.normalized;
    }

    public void StartDash()
    {
        rb.velocity = Vector3.zero;
        rb.DOMove(dashPos, dashDuration).OnComplete(EndDash);
    }
    public void EndDash()
    {
        playerMovement.EnableMovement();
        cameraController.EnableRotation();
    }

}
