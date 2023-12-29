using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SlicableObject : MonoBehaviour
{
    [SerializeField] public Material cutMaterial { get; private set; }

    public MeshRenderer meshRenderer { get; private set; }
    public Mesh mesh { get; private set; }

    private Material defaultMaterial;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>().mesh;

        defaultMaterial = meshRenderer.material;

        if(cutMaterial == null)
        {
            cutMaterial = defaultMaterial;
        }
    }

    public void SetUpSlicableObject(Transform parent, float cutForce = 500f)
    {
        transform.parent = parent;
        gameObject.layer = 7;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, transform.position, 1);
        Destroy(gameObject, 4f);
    }

}
