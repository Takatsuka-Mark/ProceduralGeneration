using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    private Mesh mesh;
    private int resolution;
    private Vector3 normalVector;
    private Vector3 axisA;
    private Vector3 axisB;
    private ShapeGenerator shapeGenerator;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 normalVector)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.normalVector = normalVector;
        this.shapeGenerator = shapeGenerator;

        axisA = new Vector3(normalVector.y, normalVector.z, normalVector.x);
        axisB = Vector3.Cross(normalVector, axisA);
    }


    public void GenerateMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[((resolution - 1) * (resolution - 1)) * 6];    // this is the number of verticies on the triangles
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                Vector2 percent = new Vector2(x, y) / (resolution - 1);    // how close to complete each of the loops are
                Vector3 pointOnUnitCube =
                    normalVector + (percent.x - 0.5f) * 2 * axisA +
                    (percent.y - 0.5f) * 2 * axisB;    // like a unit circle, but it is the world cube
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;


                var i = y * resolution + x;
                // vertices[i] = pointOnUnitCube;      // generates a cube
                // vertices[i] = pointOnUnitSphere;    // generates a sphere
                vertices[i] = shapeGenerator.CalcPointOnPlanet(pointOnUnitSphere);    // generates sphere with radius

                // this will prevent us from counting triangles 1 to the right, and below the mesh
                if (x != resolution - 1 && y != resolution - 1)
                {
                    
                    // these are the triangles that are part of the square on the mesh we are looking at
                    // triangle 1
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;
                    
                    // triangle 2
                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;

                    triIndex += 6;
                }
            }
        }

        mesh.Clear();    // when vertices are mapped at lower res, the triangles have incorrect indexing, unless cleared
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
