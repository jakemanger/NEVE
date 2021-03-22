using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiddlerCrabArenaManager : MonoBehaviour
{
    [Header("Background parameters")]
    public Color aboveHorizonColour = Color.white;
    public Color belowHorizonColour = Color.grey;
    [Range(-90, 90)]
    public float horizonHeight = 0f;

    [Header("Camera view parameters")]
    public float crabEyeHeight = 0.5f;

    [Header("Components")]
    public Transform crabEye;
    
    void Start()
    {
        // set background stimuli parameters


        // set crab eye position and rotation
        crabEye.position = new Vector3(0f, crabEyeHeight, 0f);
        crabEye.rotation = Quaternion.identity;
        
        // set computer monitor positions
        



    }

    void Update()
    {
        
    }
}
