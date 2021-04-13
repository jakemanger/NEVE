// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/
// editted to add functionality for cropping sphere and flipping tri faces to inside out

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SphereType {tetrasphere, cubesphere, octasphere, dodecasphere, icosphere, uvsphere}

/// <summary>
/// Mesh generator script to be used as component on gameobjects.
/// Resolution is how many subdivision should occur with 1 being the minimum.
/// Drop on a game object, chose a sphere type, set resolution and radius then click generate.
/// Poof! Magic happens.
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[DisallowMultipleComponent]
public class SphereGenerator : MonoBehaviour {
    public SphereType SphereType = SphereType.icosphere;
    public float Radius = 1000f;
    public int Resolution = 4;
    public bool Smooth = true;
    public bool RemapVertices = false;
    public Material material;

    public bool _generating = false;
    MeshData _sphereMesh = null;
    MeshFilter _filter = null;
    MeshRenderer _renderer = null;


    /// <summary>
    /// Starts the generation of a new mesh, erasing any previous mesh.
    /// </summary>
    public void GenerateMesh() {
        if (_generating) { return; }

        // check that # vertices doesn't exceed unity limits with 16 bits
        int resolution = 1 << (Resolution - 1);
        int verticesLength = (resolution + 1) * (resolution + 1) * 4 - (resolution * 2 - 1) * 3;

        if(_filter == null) {
            _filter = GetComponent<MeshFilter>();
        }
        if(_renderer == null) {
            _renderer = GetComponent<MeshRenderer>();
        }

        _sphereMesh = null;
        _generating = true;

        GenerateSphereMeshThread(null);
        UpdateMesh();
    }

    void Update() {
        if (_generating && _sphereMesh != null) {
            // for some reason you have to do UpdateMesh stuff in update for it to work
            UpdateMesh();
        }
    }

    void GenerateSphereMeshThread(object obj) {
        if(SphereType == SphereType.uvsphere) {
            _sphereMesh = UvSphereBuilder.Generate(Radius, Resolution);
        }
        else {
            IPlatonicSolid baseSolid = GetBaseSolid(SphereType);
            _sphereMesh = SphereBuilder.Build(baseSolid, Radius, Resolution, Smooth, RemapVertices);
        }
        // Debug.Log(SphereType.ToString() + " generated: " + _sphereMesh.Triangles.Length + " tris and " + _sphereMesh.Vertices.Length + " verts.");
    }

    IPlatonicSolid GetBaseSolid(SphereType type) {
        switch(type) {
            case SphereType.tetrasphere:
                return new Tetrahedron();

            case SphereType.cubesphere:
                return new Cube();

            case SphereType.octasphere:
                return new Octahedron();

            case SphereType.dodecasphere:
                return new Dodecahedron();

            case SphereType.icosphere:
                return new Icosahedron();

            case SphereType.uvsphere:
                return new UvPolyhedron();
                
            default:
                return new Icosahedron();
        }
    }

    void UpdateMesh() {
        _filter.sharedMesh = new Mesh();

        _filter.sharedMesh.Clear();
        _filter.sharedMesh.name = SphereType.ToString();
        _filter.sharedMesh.vertices = _sphereMesh.Vertices;
        _filter.sharedMesh.triangles = _sphereMesh.Triangles;
        _filter.sharedMesh.uv = _sphereMesh.Uv;
        _filter.sharedMesh.normals = _sphereMesh.Normals;
        _filter.sharedMesh.tangents = _sphereMesh.Tangents;

        _renderer.sharedMaterial = material;

        _generating = false;
    }
}	
