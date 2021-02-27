using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float fov = 90f, viewDistance = 50f;
    Mesh mesh;
    Vector3 origin;
    public LayerMask layerMask;
    float startingAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            float angleRad = angle * Mathf.PI / 180f;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)), viewDistance, layerMask);
            if (hit.collider == null)
            {
                vertex = origin + new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * viewDistance;
            }
            else
            {
                vertex = hit.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;



                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetDirection(Vector3 direction)
    {
        direction = direction.normalized;
        startingAngle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) + 90f;
        if (startingAngle < 0) startingAngle += 360;
        startingAngle -= fov / 2f;
    }
}
