using UnityEngine;

public class HitBox : MonoBehaviour
{

    //Temporary serilizable
    public Transform hitBoxRef;


    private Player player;
    private PlayerActions playerActions;

    private Collider[] hits;
    private int numberOfHits;
    private void Awake()
    {
        player = GetComponent<Player>();
        playerActions = player.playerActions;
        hits = new Collider[10];
    }


    public void SetHitBox(Vector3 center, Vector3 size)
    {
        hitBoxRef.position = player.cameraController.CameraPosition() + center;
        hitBoxRef.localScale = size;

    }

    //Called from animation
    public void OverlapCollider()
    {
        numberOfHits = Physics.OverlapBoxNonAlloc(hitBoxRef.position, hitBoxRef.localScale * 0.5f, hits, hitBoxRef.rotation);


        for (int i = 0; i < numberOfHits; i++)
        {
            CheckHitInfo(hits[i]);
        }
    }
    private void CheckHitInfo(Collider hit)
    {
        if (hit.TryGetComponent(out SlicableMesh mesh))
        {
            playerActions.currentWeapon.Slice(mesh);
        }
    }
}
