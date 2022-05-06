using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;

    public TerrainFace(Mesh mesh, int resolution, Vector3 localUp) {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh() {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution-1) * (resolution-1) * 2 * 3];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + (y * resolution);
                Vector2 percent = new Vector2(x, y) / (resolution - 1); // tells you how close to completion each loop is
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2f * axisA + (percent.y - .5f) * 2f * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = pointOnUnitSphere;

                // make sure current index is not along the right or bottom edge
                if (y != resolution-1 && x != resolution-1) {
                    // set indexes for each triangle (two triangles per four vertices)
                    // work clockwise
                    // first triangle
                    // *    * 
                    // | \
                    // |  \
                    // * -- *
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;
                    // second triangle
                    // * -- *
                    //   \  |
                    //    \ |
                    // *    *
                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;

                    // just added 6 vertices for triangles, so increase the index by 6
                    triIndex += 6;
                }
            }
        }

        // now add these vertices and triangles to the mesh
        mesh.Clear(); // clear first so old triangles don't cause errors when you update vertices
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
