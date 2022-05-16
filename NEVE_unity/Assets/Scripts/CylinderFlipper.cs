using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CylinderFlipper : MonoBehaviour
{
    public Mesh cyclinderMesh;
    bool meshMade = false;
    bool meshFlipped = false;

    void Awake() {
        // create a cylinder
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = cyclinderMesh;
    }

    void Update() {
        if (!meshFlipped && meshMade) {
            // now flip it to be inside out, so you can see from the inside
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Vector3[] vertices = meshFilter.sharedMesh.vertices;
            int i = 0;
            var surfaceNormal = Vector3.Cross (vertices[i+1]-vertices[i], vertices[i+2]-vertices[i]);
            print(surfaceNormal);
            SphereEditor sphereEditor = GetComponent<SphereEditor>();
            sphereEditor.FlipMesh();
            meshFlipped = true;
        }
        meshMade = true;
    }
}
