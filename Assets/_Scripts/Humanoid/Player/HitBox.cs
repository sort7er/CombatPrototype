using UnityEngine;

public class HitBox : MonoBehaviour
{

    //Temporary serilizable
    public Transform hitBoxRef;
    private Weapon currentWeapon;
    private Humanoid owner;
    private Collider[] hits;

    private int numberOfHits;
    private void Awake()
    {
        hits = new Collider[10];
    }
    private void Start()
    {
        //Moved GetOwner to start to ensure that playerActions actually has the current weapon
        GetOwner();
    }

    public void GetOwner()
    {
        owner = GetComponent<Humanoid>();
        if(owner is Player player)
        {
            currentWeapon = player.playerActions.currentWeapon;
        }
    }


    public void SetHitBox(Transform parent, Vector3 localCenter, Vector3 size)
    {
        hitBoxRef.parent = parent;
        hitBoxRef.localPosition = localCenter;
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
        if(hit.TryGetComponent(out Health health))
        {
            if(health != owner.health)
            {
                DoDamage(health, hit.ClosestPointOnBounds(currentWeapon.transform.position));
            }
        }
        else if (hit.TryGetComponent(out SlicableMesh mesh))
        {
            currentWeapon.Slice(mesh);
        }
    }

    private void DoDamage(Health health, Vector3 hitPoint)
    {
        currentWeapon.Hit(hitPoint);
        health.TakeDamage(currentWeapon);
    }
}
