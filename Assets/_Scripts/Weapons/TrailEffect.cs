using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    [SerializeField] private int trailFrameLength;

    [SerializeField] private Transform tip;
    [SerializeField] private Transform bottom;
    [SerializeField] private MeshFilter trailMeshFilter;

    private Transform transformToAjustTo;

    private Mesh mesh;
    private Vector3[] vertecies;
    private Vector2[] uv;
    private int[] triangles;
    private int vertexCount;
    private int frameCount;

    private Vector3 previouisTipPos;
    private Vector3 previouisBottomPos;

    private const int numVerticies = 6;



    private void Start()
    {
        mesh = new Mesh();
        trailMeshFilter.mesh = mesh;

        vertecies = new Vector3[trailFrameLength * numVerticies];
        uv = new Vector2[vertecies.Length];
        triangles = new int[vertecies.Length];

        previouisTipPos = tip.position;
        previouisBottomPos = bottom.position;

        //DisableTrails();
    }
    public void EnableTrails(Transform transform)
    {
        //trailMeshFilter.gameObject.SetActive(true);
        //transformToAjustTo = transform;
    }
    public void DisableTrails()
    {
        //trailMeshFilter.gameObject.SetActive(false);
        //transformToAjustTo = null;
    }

    private void Update()
    {
        //if(trailMeshFilter.gameObject.activeSelf)
        //{
        //    trailMeshFilter.transform.position = Vector3.zero;
        //    trailMeshFilter.transform.rotation = Quaternion.identity;
        //}
        trailMeshFilter.transform.position = Vector3.zero;
        trailMeshFilter.transform.rotation = Quaternion.identity;

    }

    private void LateUpdate()
    {
        //if (trailMeshFilter.gameObject.activeSelf)
        //{
        //    trailMeshFilter.transform.position = Vector3.zero;
        //    trailMeshFilter.transform.rotation = Quaternion.identity;

        //    if (frameCount == (trailFrameLength * numVerticies))
        //    {
        //        frameCount = 0;
        //    }

        //    SetVerecies();
        //    SetTriangles();
        //    SetUV();
        //    SetMesh();


        //    previouisTipPos = tip.position;
        //    previouisBottomPos = bottom.position;
        //    frameCount += numVerticies;

        //    mesh.RecalculateBounds();
        //}
        trailMeshFilter.transform.position = Vector3.zero;
        trailMeshFilter.transform.rotation = Quaternion.identity;

        if (vertexCount == (trailFrameLength * numVerticies))
        {
            vertexCount = 0;
            frameCount = 0;
        }

        SetVerecies();
        SetTriangles();
        SetUV();
        SetMesh();


        previouisTipPos = tip.position;
        previouisBottomPos = bottom.position;
        vertexCount += numVerticies;
        frameCount++;

        mesh.RecalculateBounds();
    }
    private void SetVerecies()
    {
        //Tip triangle
        vertecies[vertexCount] = bottom.position;
        vertecies[vertexCount + 1] = tip.position;
        vertecies[vertexCount + 2] = previouisTipPos;

        //Bottom triangle
        vertecies[vertexCount + 3] = previouisTipPos;
        vertecies[vertexCount + 4] = bottom.position;
        vertecies[vertexCount + 5] = previouisBottomPos;

    }

    private void SetTriangles()
    {
        for (int i = 0; i < numVerticies; i++)
        {
            triangles[vertexCount + i] = vertexCount + i;
        }
    }
    private void SetUV()
    {
        float ratio = 1 / (float)trailFrameLength;

        Debug.Log(vertexCount);

        for (int i = 0; i < trailFrameLength; i++)
        {
            //The number of UVs here NEED to be the same amount as numVertecies

            int offset = i * numVerticies;
            int ajusdedOffset = offset /*+ frameCount*/;

            //if(ajusdedOffset > uv.Length - 1)
            //{
            //    int difference = ajusdedOffset - uv.Length - 1;
            //    ajusdedOffset = 0 + difference;
            //}


            uv[0 + ajusdedOffset] = new Vector2(ratio * i + ratio, 1);
            uv[1 + ajusdedOffset] = new Vector2(ratio * i + ratio, 0);
            uv[2 + ajusdedOffset] = new Vector2(ratio * i, 0);

            uv[3 + ajusdedOffset] = new Vector2(ratio * i, 0);
            uv[4 + ajusdedOffset] = new Vector2(ratio * i, 1);
            uv[5 + ajusdedOffset] = new Vector2(ratio * i + ratio, 1);


        }
    }
    private void SetMesh()
    {
        mesh.vertices = vertecies;
        mesh.triangles = triangles;
        mesh.uv = uv;
    }


}
