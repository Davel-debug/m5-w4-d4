using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FOVVisualizer : MonoBehaviour
{
    public FieldOfView fov;
    public int meshResolution = 30;
    public float heightOffset = -1f; // quanto abbassare le mesh rispetto all'enemy

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
        if (fov == null || meshResolution <= 0) return;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // y di riferimento abbassata
        float baseY = fov.transform.position.y + heightOffset;

        // centro locale del cono
        vertices.Add(new Vector3(0, heightOffset, 0));

        float stepAngle = fov.viewAngle / meshResolution;
        for (int i = 0; i <= meshResolution; i++)
        {
            float angle = -fov.viewAngle / 2 + stepAngle * i;
            Vector3 dir = fov.DirFromAngle(angle, false);
            RaycastHit hit;

            Vector3 worldVertex;
            if (Physics.Raycast(fov.transform.position, dir, out hit, fov.viewRadius, fov.obstacleMask))
                worldVertex = hit.point;
            else
                worldVertex = fov.transform.position + dir * fov.viewRadius;

            worldVertex.y = baseY; // forza l’altezza
            vertices.Add(transform.InverseTransformPoint(worldVertex));
        }

        // triangoli cono
        for (int i = 1; i < meshResolution + 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        // cerchio extra
        int circleStartIndex = vertices.Count;
        vertices.Add(new Vector3(0, heightOffset, 0)); // pivot cerchio extra

        for (int i = 0; i <= meshResolution; i++)
        {
            float angle = (360f / meshResolution) * i;
            Vector3 dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
            Vector3 worldVertex = fov.transform.position + dir * fov.enemy.extraViewRadius;
            worldVertex.y = baseY;
            vertices.Add(transform.InverseTransformPoint(worldVertex));
        }

        // triangoli cerchio
        for (int i = circleStartIndex + 1; i < vertices.Count - 1; i++)
        {
            triangles.Add(circleStartIndex);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        // aggiorna mesh
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
}
