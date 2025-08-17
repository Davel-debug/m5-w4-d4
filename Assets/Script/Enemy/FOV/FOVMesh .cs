using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FOVVisualizer : MonoBehaviour
{
    public FieldOfView fov; // riferimento al FieldOfView
    public int meshResolution = 30;

    private MeshFilter meshFilter;
    private Mesh mesh;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        mesh.name = "FOV Mesh";
        meshFilter.mesh = mesh;
    }

    void LateUpdate()
    {
        if (fov == null || fov.enemy == null) return;
        DrawFOV();
    }

    void DrawFOV()
    {
        if (fov == null || meshResolution <= 0) return; // evita crash

        float stepAngle = fov.viewAngle / meshResolution;
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        vertices.Add(Vector3.zero); // centro

        for (int i = 0; i <= meshResolution; i++)
        {
            float angle = -fov.viewAngle / 2 + stepAngle * i;
            Vector3 dir = fov.DirFromAngle(angle, false);
            RaycastHit hit;

            Vector3 vertex;
            if (Physics.Raycast(fov.transform.position, dir, out hit, fov.viewRadius, fov.obstacleMask))
            {
                vertex = transform.InverseTransformPoint(hit.point);
            }
            else
            {
                vertex = transform.InverseTransformPoint(fov.transform.position + dir * fov.viewRadius);
            }

            vertices.Add(vertex);
        }

        // Ora ci sono vertices.Length = meshResolution + 2
        for (int i = 1; i < vertices.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }


}
