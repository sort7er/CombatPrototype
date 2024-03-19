using DynamicMeshCutter;
using UnityEngine;

public class SlicingController : MonoBehaviour
{
    public MeshTarget meshTarget;
    public DynamicRagdoll dynamicRagdoll;
    public Animator animator;

    private Weapon weapon;


    public void Slice(Weapon attackingWeapon)
    {
        CancelInvoke(nameof(Delay));

        weapon = attackingWeapon;
        transform.parent = ParentManager.instance.meshes;

        Invoke(nameof(Delay), 0.05f);
    }
    private void Delay()
    {
        if (dynamicRagdoll != null)
        {
            EnableRagdoll();
        }

        weapon.Slice(meshTarget);
    }

    private void EnableRagdoll()
    {
        animator.enabled = false;
        dynamicRagdoll.IsRagdollKinematic= false;
    }
}
