using DynamicMeshCutter;
using UnityEngine;

public class SlicingController : MonoBehaviour
{
    public MeshTarget meshTarget;
    public DynamicRagdoll dynamicRagdoll;
    public Animator animator;

    public bool isDead;

    private Weapon weapon;
    public void Slice(Weapon attackingWeapon)
    {

        weapon = attackingWeapon;
        transform.parent = ParentManager.instance.meshes;

        if (dynamicRagdoll != null)
        {
            EnableRagdoll();
        }

        isDead = true;
        weapon.Slice(meshTarget);
    }

    private void EnableRagdoll()
    {
        animator.enabled = false;
        dynamicRagdoll.IsRagdollKinematic= false;
    }
}
