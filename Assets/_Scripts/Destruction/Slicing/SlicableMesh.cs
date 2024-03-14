using DynamicMeshCutter;
using UnityEngine;

public class SlicableMesh : MonoBehaviour
{
    public void SetUpSlicableObject(GameObject mesh, float cutForce = 500f)
    {
        transform.parent = ParentManager.instance.meshes;
        gameObject.layer = 7;

        Rigidbody rb = mesh.GetComponent<Rigidbody>();

        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            Debug.Log("Backup");
        }

        rb.AddExplosionForce(cutForce, transform.position, 1);

        Destroy(gameObject, 4f);
    }
}
