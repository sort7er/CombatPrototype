using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    [SerializeField] private int trailFrameLength;

    [SerializeField] private Transform tip;
    [SerializeField] private Transform bottom;
    [SerializeField] private MeshFilter trailMeshFilter;

    private Mesh mesh;
    private Vector3[] vertecies;
    private Vector2[] uv;
    private int[] triangles;
    private int frameCount;

    private Vector3 previouisTipPos;
    private Vector3 previouisBottomPos;

    private const int numVerticies = 12;



    private void Start()
    {
        mesh = new Mesh();
        trailMeshFilter.mesh = mesh;

        vertecies = new Vector3[trailFrameLength * numVerticies];
        uv = new Vector2[vertecies.Length];
        triangles = new int[vertecies.Length];

        previouisTipPos = tip.position;
        previouisBottomPos = bottom.position;

        DisableTrails();
    }
    public void EnableTrails()
    {
        trailMeshFilter.gameObject.SetActive(true);
    }
    public void DisableTrails()
    {
        trailMeshFilter.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(trailMeshFilter.gameObject.activeSelf)
        {
            trailMeshFilter.transform.position = Vector3.zero;
            trailMeshFilter.transform.rotation = Quaternion.identity;
        }

    }

    private void LateUpdate()
    {
        if (trailMeshFilter.gameObject.activeSelf)
        {
            trailMeshFilter.transform.position = Vector3.zero;
            trailMeshFilter.transform.rotation = Quaternion.identity;

            if (frameCount == (trailFrameLength * numVerticies))
            {
                frameCount = 0;
            }

            SetVerecies();
            SetTriangles();
            SetUV();
            SetMesh();


            previouisTipPos = tip.position;
            previouisBottomPos = bottom.position;
            frameCount += numVerticies;

            mesh.RecalculateBounds();
        }

    }
    private void SetVerecies()
    {
        //Previus tip face forwards
        vertecies[frameCount] = bottom.position;
        vertecies[frameCount + 1] = tip.position;
        vertecies[frameCount + 2] = previouisTipPos;
        //Previus tip face backwards
        vertecies[frameCount + 3] = bottom.position;
        vertecies[frameCount + 4] = previouisTipPos;
        vertecies[frameCount + 5] = tip.position;

        //Previous bottom face forwards
        vertecies[frameCount + 6] = previouisTipPos;
        vertecies[frameCount + 7] = bottom.position;
        vertecies[frameCount + 8] = previouisBottomPos;
        //Previous bottom face backwards
        vertecies[frameCount + 9] = previouisTipPos;
        vertecies[frameCount + 10] = previouisBottomPos;
        vertecies[frameCount + 11] = bottom.position;
    }

    private void SetTriangles()
    {
        for (int i = 0; i < numVerticies; i++)
        {
            triangles[frameCount + i] = frameCount + i;
        }
    }
    private void SetUV()
    {
        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new Vector2(vertecies[i].x, vertecies[i].z);
        }
    }
    private void SetMesh()
    {
        mesh.vertices = vertecies;
        mesh.triangles = triangles;
        mesh.uv = uv;
    }
}
