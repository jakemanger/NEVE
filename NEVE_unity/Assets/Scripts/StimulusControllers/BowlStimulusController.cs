using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(SphereGenerator)), RequireComponent(typeof(SphereEditor))]
public class BowlStimulusController : GenericStimulusController
{
    public Vector3 rotation = Vector3.zero;

    [Range(-90, 90)]
    public float croppedAngle = 0f;

    [Range(1, 10)]
    public int sphereResolution = 8;
    public float radius = 1000f;
    public Material material;

    public Color materialColor = Color.grey;
    SphereGenerator sphereGenerator;
    SphereEditor sphereEditor;
    MeshRenderer meshRenderer;

    bool bowlMade = false;

    void Start() {
        sphereGenerator = GetComponent<SphereGenerator>();
        sphereEditor = GetComponent<SphereEditor>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void CreateBowl() {
        sphereGenerator.Resolution = sphereResolution;
        sphereGenerator.Radius = radius;
        material.color = materialColor;
        sphereGenerator.material = material;
        sphereGenerator.GenerateMesh();
        bowlMade = false;
    }

    void EditBowl() {
        sphereEditor.xMin = croppedAngle;
        sphereEditor.stretchEdgeVerticesToAngle = true;
        sphereEditor.CropMesh();
        sphereEditor.FlipMesh();
        meshRenderer.sharedMaterial = material;
        bowlMade = true;
    }

    void Update()
    {
        transform.localEulerAngles = rotation;

        bool meshGenerated = !sphereGenerator._generating;

        if (meshGenerated && !bowlMade) {
            EditBowl();
        }
    }
}
