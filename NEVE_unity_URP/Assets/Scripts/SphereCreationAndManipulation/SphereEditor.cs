using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SphereEditor : MonoBehaviour
{
    public MeshFilter meshFilter;

    // limits for cropping
    [Range(-90, 90)]
    public float xMin = -90f;
    [Range(-90, 90)]
    public float xMax = 90f;
    [Range(-180, 180)]
    public float yMin = -90f;
    [Range(-180, 180)]
    public float yMax = 90f;

    // we add a buffer as the position can vary very slightly
    float cropBuffer = 0.005f;

    List<Vector3> gizmosCirclePos = new List<Vector3>();

    void OnValidate() {
        FindVerticesToCrop();
    }


    public void FlipMesh() {
        Mesh mesh = meshFilter.sharedMesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }

    public void CropMesh() {
        // get vertices and triangles
        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        List<bool> keepVertex = new List<bool>();
        int[] triangles = mesh.triangles;
        List<int> newTriangles = new List<int>();

        // loop through each vertex and get polar coordinates
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 polCoords = CartesianToPolar(transform.TransformPoint(vertices[i]));
            // check if within x and y limits and keep a record for each vertex
            bool withinLimits = (
                polCoords.x <= xMax + cropBuffer &&
                polCoords.x >= xMin - cropBuffer &&
                polCoords.y <= yMax + cropBuffer &&
                polCoords.y >= yMin - cropBuffer
            );
            if (withinLimits) {
                keepVertex.Add(true);
            } else {
                keepVertex.Add(false);
            }
        }

        // now create a list of triangle indices to keep
        for (int i = 0; i < triangles.Length; i += 3)
        {
            // if all three vertices of the triangle should be kept, then add to newTriangles
            if (keepVertex[triangles[i]] && keepVertex[triangles[i + 1]] && keepVertex[triangles[i + 2]]) {
                newTriangles.Add(triangles[i]);
                newTriangles.Add(triangles[i + 1]);
                newTriangles.Add(triangles[i + 2]);
            }
        }

        // and add it to the mesh
        mesh.triangles = newTriangles.ToArray();
    }

    // for viewing when cropping in the editor
    void FindVerticesToCrop() {
        meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        gizmosCirclePos = new List<Vector3>();
        // loop through each vertex and get polar coordinates
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 polCoords = CartesianToPolar(transform.TransformPoint(vertices[i]));
            // check if within x and y limits and keep a record for each vertex
            bool withinLimits = (
                polCoords.x <= xMax + cropBuffer &&
                polCoords.x >= xMin - cropBuffer &&
                polCoords.y <= yMax + cropBuffer &&
                polCoords.y >= yMin - cropBuffer
            );
            if (withinLimits) {
                gizmosCirclePos.Add(vertices[i]);
            }
        }
    }

    void OnDrawGizmosSelected() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        foreach (Vector3 circle in gizmosCirclePos)
        {
            Gizmos.DrawSphere(circle, 0.01f);
        }
    }

    Vector2 CartesianToPolar(Vector3 point) {
        Vector2 polar;
        // calculate longitude
        polar.y = Mathf.Atan2(point.x, point.z);
        // calculate sqrt(pow(x, 2), pow(y, 2))
        float xzLen = new Vector2(point.x, point.z).magnitude;
        polar.x = Mathf.Atan2(-point.y, xzLen);
        // convert to deg
        polar *= Mathf.Rad2Deg;

        return polar;
    }
}
