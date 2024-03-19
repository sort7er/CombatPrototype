using DynamicMeshCutter;
using UnityEngine;

public class SlicingController : MonoBehaviour
{
    public MeshTarget meshTarget;
    public DynamicRagdoll dynamicRagdoll;
    public Animator animator;


    public void Slice(Weapon attackingWeapon)
    {
        if(dynamicRagdoll != null)
        {
            EnableRagdoll();
        }
       
        transform.parent = ParentManager.instance.meshes;
        attackingWeapon.Slice(meshTarget);
    }

    private void EnableRagdoll()
    {
        animator.enabled = false;
        dynamicRagdoll.IsRagdollKinematic= false;
    }
}
