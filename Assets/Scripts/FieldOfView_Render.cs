using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView_Render : MonoBehaviour
{
    [SerializeField] float Angle = 90f;
    [SerializeField] float Distance = 5f;

    private MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private Vector3 AngleDirection(float Degrees)
    {
        float Radians = Degrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(Radians), 0f, Mathf.Cos(Radians));
    }

    public void DrawFieldOfView()
    {
        int rayCount = 50;
        float angleStep = Angle / rayCount;
        float currentAngle = -Angle * 0.5f;

        Vector3[] Vertices = new Vector3[rayCount + 2];
        Vector2[] Uv = new Vector2[Vertices.Length];
        int[] Triangles = new int[rayCount * 3];

        Vertices[0] = Vector3.zero;
        int vertixIndex = 1;
        int triangleIndex = 0;

        for ( int i = 0; i<= rayCount; i++)
        {
            Vector3 Vertix;
            RaycastHit Hit;

            if (Physics.Raycast(transform.position, AngleDirection(currentAngle), out Hit, Distance))
            {
                Vertix = Hit.point - transform.position;
            }
            else
            {
                Vertix = AngleDirection(Angle) * Distance;
            }

            Vertices[vertixIndex] = Vertix;
            Uv[vertixIndex] = new Vector2(Vertix.x, Vertix.z);
                if (i > 0)
                {
                    Triangles[triangleIndex + 0] = 0;
                    Triangles[triangleIndex + 1] = vertixIndex - 1;
                    Triangles[triangleIndex + 2] = vertixIndex;
                    triangleIndex += 3;
                }
            currentAngle += angleStep;
            vertixIndex++;

        }

        meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = Vertices;
        meshFilter.mesh.uv = Uv;
        meshFilter.mesh.triangles = Triangles;

    }

}
