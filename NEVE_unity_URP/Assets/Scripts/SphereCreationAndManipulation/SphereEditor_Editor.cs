// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SphereEditor))]
public class SphereEditor_Editor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Flip mesh")) {
            ((SphereEditor)target).FlipMesh();
        }
        if (GUILayout.Button("Crop mesh")) {
            ((SphereEditor)target).CropMesh();
        }
    }
}
