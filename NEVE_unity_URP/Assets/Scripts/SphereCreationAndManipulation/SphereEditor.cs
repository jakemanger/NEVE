using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum Cut {xMin, xMax, yMin, yMax}

public class SphereEditor : MonoBehaviour
{
    public MeshFilter meshFilter;

    public bool stretchEdgeVerticesToAngle = true;

    // limits for cropping
    [Range(-90, 90)]
    public float xMin = -90f;
    [Range(-90, 90)]
    public float xMax = 90f;
    [Range(-180, 180)]
    public float yMin = -180f;
    [Range(-180, 180)]
    public float yMax = 180f;

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
        MakeCut(Cut.yMax);
        // MakeCut(Cut.yMin);
        // MakeCut(Cut.xMin);
        // MakeCut(Cut.xMax);
    }

    void MakeCut(Cut cut) {
        // get vertices and triangles
        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        List<bool> keepVertex = new List<bool>();
        int[] triangles = mesh.triangles;
        List<int> newTriangles = new List<int>();
        float[] xCoords = new float[vertices.Length];
        float[] yCoords = new float[vertices.Length];

        // loop through each vertex and get polar coordinates
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 polCoord = CartesianToPolar(transform.TransformPoint(vertices[i]));

            // check if within x and y limits and keep a record for each vertex
            bool withinLimits = (
                polCoord.x <= xMax + cropBuffer &&
                polCoord.x >= xMin - cropBuffer &&
                polCoord.y <= yMax + cropBuffer &&
                polCoord.y >= yMin - cropBuffer
            );
            if (withinLimits) {
                keepVertex.Add(true);
            } else {
                keepVertex.Add(false);
            }
            // save coords to find edges of cropping to stretch
            xCoords[i] = polCoord.x;
            yCoords[i] = polCoord.y;
        }

        if (stretchEdgeVerticesToAngle) {
            bool[] edgeIndices = new bool[xCoords.Length];
            if (cut == Cut.xMin) {
                // find edges for each crop direction
                edgeIndices = FindEdgeIndices(xCoords, keepVertex, -90f, true);
                // stretch these to precise angle of crop
                Vector3[] newVertices = StretchEdgeVerticesToAngle(mesh, edgeIndices, xMin, true);
                mesh.vertices = newVertices;
            }
            if (cut == Cut.xMax) {
                edgeIndices = FindEdgeIndices(xCoords, keepVertex, 90f, false);
                Vector3[] newVertices = StretchEdgeVerticesToAngle(mesh, edgeIndices, xMax, true);
                mesh.vertices = newVertices;
            }
            if (cut == Cut.yMin) {
                // because y loops around 360 degrees, the min extreme will also incorporate step size
                float[] uniqueCoords = yCoords.Distinct().ToArray();
                Array.Reverse(uniqueCoords);
                float stepSize = uniqueCoords[1] - uniqueCoords[0];
                edgeIndices = FindEdgeIndices(yCoords, keepVertex, -180f + stepSize, true);
                Vector3[] newVertices = StretchEdgeVerticesToAngle(mesh, edgeIndices, yMin, false);
                mesh.vertices = newVertices;
            }
            if (cut == Cut.yMax) {
                edgeIndices = FindEdgeIndices(yCoords, keepVertex, 180f, false);
                Vector3[] newVertices = StretchEdgeVerticesToAngle(mesh, edgeIndices, yMax, false);
                mesh.vertices = newVertices;
            }
            
            // and keep them by adding to keepVertex
            for (int i = 0; i < keepVertex.Count; i++)
            {
                if (!keepVertex[i]) {
                    bool stretched = edgeIndices[i];
                    keepVertex[i] = stretched;
                }
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

    bool[] FindEdgeIndices(float[] coords, List<bool> keepVertex, float extreme, bool isMin) {
        bool[] edge = new bool[coords.Length];
        float edgeValue;
        bool noCrop = false; // to tell whether a crop isn't necessary

        // get coords that should be kept
        List<float> coordsToKeep = new List<float>();

        for (int i = 0; i < coords.Length; i++)
        {
            if (keepVertex[i]) {
                coordsToKeep.Add(coords[i]);
            }
        }

        if (isMin) {
            edgeValue = Mathf.Min(coordsToKeep.ToArray());
        } else {
            edgeValue = Mathf.Max(coordsToKeep.ToArray());
        }
        print(Mathf.Abs(extreme - edgeValue));
        if (Mathf.Abs(extreme - edgeValue) < cropBuffer) {
            noCrop = true;
        }
        print("no crop" + noCrop);
        if (!noCrop) {
            print("cropping");
            print("edgeValue" + edgeValue);
            print("extreme" + extreme);
            for (int i = 0; i < coords.Length; i++)
            {
                edge[i] = (coords[i] < edgeValue + cropBuffer) && (coords[i] > edgeValue - cropBuffer);
            }
        }
        return edge;
    }

    // stretches the final vertices to a specific angle to get precise angles
    public Vector3[] StretchEdgeVerticesToAngle(Mesh mesh, bool[] valuesToStretch, float stretchAngle, bool isXAxis) {
        Vector3[] currentVertices = mesh.vertices;
        // get all vertex positions in polar coordinates
        Vector2[] polarVertices = new Vector2[currentVertices.Length];
        for (int i = 0; i < currentVertices.Length; i++)
        {
            polarVertices[i] = CartesianToPolar(currentVertices[i]);
        }
        float distToCenter = Vector3.Distance(currentVertices[0], transform.position);

        for (int i = 0; i < polarVertices.Length; i++)
        {
            if (valuesToStretch[i]) {
                if (isXAxis) {
                    // keep the y axis coordinates and reset the x and distToCenter
                    polarVertices[i].x = stretchAngle;
                } else { // is a Y axis cut
                    // keep the x axis coordinates and reset the y and distToCenter
                    polarVertices[i].y = stretchAngle;
                }
                print("stretching");
                print(stretchAngle);

                currentVertices[i] = PolarToCartesian(polarVertices[i], distToCenter);
                print("new vertices");
                print(currentVertices[i]);
            }
        }

        print("Stretching edge vertices");

        return currentVertices;
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
            Vector2 polCoord = CartesianToPolar(transform.TransformPoint(vertices[i]));
            // check if within x and y limits and keep a record for each vertex
            bool withinLimits = (
                polCoord.x <= xMax + cropBuffer &&
                polCoord.x >= xMin - cropBuffer &&
                polCoord.y <= yMax + cropBuffer &&
                polCoord.y >= yMin - cropBuffer
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

    Vector3 PolarToCartesian(Vector2 polar, float offsetFromCenter) {
        Vector3 origin = new Vector3(0, 0, offsetFromCenter);

        // build a quaternion using euler angles for lat and lon
        Quaternion rotation = Quaternion.Euler(polar.x, polar.y, 0);
        // transform reference vector by the rotation
        Vector3 point = rotation * origin;

        return point;
    }
}
