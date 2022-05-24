// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SphereGenerator))]
public class SphereGenerator_Editor : Editor {
    
    private SphereGenerator sphere;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Update Mesh")) {
            Debug.Log("generating from button press");
            ((SphereGenerator)target).GenerateMesh();
        }
    }

    private void OnEnable() {
        sphere = (SphereGenerator)target;
    }
}
