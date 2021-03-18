using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlStimulusController : MonoBehaviour
{
    public Vector3 rotation = Vector3.zero;

    // limits for cropping
    [Range(-90, 90)]
    public float xMin = -90f;
    [Range(-90, 90)]
    public float xMax = 90f;
    [Range(-180, 180)]
    public float yMin = -90f;
    [Range(-180, 180)]
    public float yMax = 90f;

    void Start() {
        // crop sphere

    }

    void Update()
    {
        transform.Rotate(rotation);
    }
}
