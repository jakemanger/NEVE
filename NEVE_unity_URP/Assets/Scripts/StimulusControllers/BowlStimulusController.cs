using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlStimulusController : MonoBehaviour
{
    public Vector3 rotation = Vector3.zero;

    [Range(-90, 90)]
    public float croppedAngle = -90f;


    void Start() {
        // crop sphere

    }

    void Update()
    {
        transform.Rotate(rotation);
    }
}
