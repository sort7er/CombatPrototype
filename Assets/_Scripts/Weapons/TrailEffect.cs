using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    [SerializeField] private int trailFrameLength;

    [SerializeField] private Transform top;
    [SerializeField] private Transform bottom;
    [SerializeField] private MeshFilter trailMeshFilter;
    [SerializeField] private ParticleSystem particleSystem;

    private Transform transformToAjustTo;

    private Mesh mesh;
    private Vector3[] vertecies;
    private Vector2[] uv;
    private int[] triangles;

    private Vector3[] topPositions;
    private Vector3[] bottomPositions;

    private int numVerticies;
    private int halfNumVerticies;
    private int numTris;

    private float timer;
    private int targetFPS = 60;

    private void Start()
    {
        mesh = new Mesh();
        trailMeshFilter.mesh = mesh;

        numVerticies = trailFrameLength * 2 + 2;
        halfNumVerticies = Mathf.RoundToInt(numVerticies * 0.5f);
        numTris = trailFrameLength * 6;

        vertecies = new Vector3[numVerticies];
        uv = new Vector2[numVerticies];
        triangles = new int[numTris];

        topPositions = new Vector3[halfNumVerticies];
        bottomPositions = new Vector3[halfNumVerticies];

        CalculatePositions();

        DisableTrails();
    }
    public void EnableTrails(Transform transform)
    {
        particleSystem.Play();
        trailMeshFilter.gameObject.SetActive(true);
        transformToAjustTo = transform;

        for(int i = 0; i < halfNumVerticies; i++)
        {
            topPositions[i] = top.position;
            bottomPositions[i] = bottom.position;
        } 
        CancelInvoke(nameof(Delay));
    }
    public void DisableTrails(float delay = 0)
    {
        particleSystem.Stop();
        if(delay == 0)
        {
            trailMeshFilter.gameObject.SetActive(false);
            transformToAjustTo = null;
        }
        else
        {
            Invoke(nameof(Delay), delay);   
        }
    }

    private void Delay()
    {
        trailMeshFilter.gameObject.SetActive(false);
        transformToAjustTo = null;
    }

    private void Update()
    {
        if(trailMeshFilter.gameObject.activeSelf)
        {
            trailMeshFilter.transform.position = Vector3.zero;
            trailMeshFilter.transform.rotation = Quaternion.identity;
        }

    }

    private void FixedUpdate()
    {
        //Is in fixed to ensure constant framerate across devices
        if (trailMeshFilter.gameObject.activeSelf)
        {
            trailMeshFilter.transform.position = Vector3.zero;
            trailMeshFilter.transform.rotation = Quaternion.identity;

            SetVerecies();
            SetTriangles();
            SetUV();
            SetMesh();

            CalculatePositions();

            mesh.RecalculateBounds();
        }
    }

    private void CalculatePositions()
    {
        topPositions[0] = top.position;
        bottomPositions[0] = bottom.position;

        for (int i = halfNumVerticies - 1; i >= 1; i--)
        {
            topPositions[i] = topPositions[i - 1];
            bottomPositions[i] = bottomPositions[i - 1];
        }
    }
    private void SetVerecies()
    {
        vertecies[0] = bottom.position;
        vertecies[1] = top.position;

        for (int frame = 0, vertexIndex = 2; frame < trailFrameLength; frame++)
        {
            vertecies[vertexIndex] = bottomPositions[frame + 1];
            vertecies[vertexIndex + 1] = topPositions[frame + 1];


            vertexIndex += 2;
        }
    }

    private void SetTriangles()
    {
        for (int frame = 0, triangle = 0, line = 0; frame < trailFrameLength; frame++)
        {
            //Needs to be 6 to avoid null reference in index
            triangles[triangle] = line;
            triangles[triangle + 1] = line + 1;
            triangles[triangle + 2] = line + 3;

            triangles[triangle + 3] = line;
            triangles[triangle + 4] = line + 3;
            triangles[triangle + 5] = line + 2;

            line += 2;
            triangle += 6;
        }
    }
    private void SetUV()
    {
        float ratio = 1 / (float)numVerticies;

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);

        for (int frame = 0, uvIndex = 2; frame < trailFrameLength; frame ++)
        {

            uv[uvIndex] = new Vector2(ratio * uvIndex, 0);
            uv[uvIndex + 1] = new Vector2(ratio * uvIndex, 1);

            uvIndex += 2;
        }
    }
    private void SetMesh()
    {
        mesh.vertices = vertecies;
        mesh.triangles = triangles;
        mesh.uv = uv;
    }


}
