using DynamicMeshCutter;
using UnityEngine;

public class SlicingController : MonoBehaviour
{
    public MeshTarget meshTarget;
    private Weapon weapon;
    public void Slice(Weapon attackingWeapon)
    {

        weapon = attackingWeapon;
        transform.parent = ParentManager.instance.meshes;

        Invoke(nameof(Delay), 0.05f);
    }

    private void Delay()
    {
        weapon.Slice(meshTarget);
    }
    public void JustCut(Weapon attackingWeapon, Vector3 worldPosition, Vector3 worldNormal)
    {
        weapon = attackingWeapon;
        weapon.JustCut(meshTarget, worldPosition, worldNormal);
    }
}
