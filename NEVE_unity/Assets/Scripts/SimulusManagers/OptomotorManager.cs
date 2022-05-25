using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class OptomotorManager : GenericStimulusManager
{

    [Header("Specific stimulus parameters")]
    public float density = 5f; // density of sine waves
    public float offset = 0f;
    public float angle = 0f;
    public float speed = 2f;
    public bool square = false;
    public float minimumVal = 0f;
    public float maximumVal = 0.5f;


    protected override void GetPropertiesFromPython() {
        // get generic stimulus parameters from python
        base.GetPropertiesFromPython();

        // now get those specific to this stimuli
        density = GetFloatFromPython("density", 5f);
        offset = GetFloatFromPython("offset", 0f);
        angle = GetFloatFromPython("angle", 0f);
        speed = GetFloatFromPython("speed", 2f);
        square = GetBoolFromPython("square", false);
        minimumVal = GetFloatFromPython("minimumVal", 0f);
        maximumVal = GetFloatFromPython("maximumVal", 0.5f);
    }

    public override void SetupStimuli() {
        Material mat = RenderSettings.skybox;
        mat.SetFloat("_Density", density);
        mat.SetFloat("_Offset", offset);
        mat.SetFloat("_Angle", angle);
        mat.SetFloat("_Speed", speed);
        mat.SetInt("_Square", square ? 1 : 0);
        mat.SetFloat("_Minimum", minimumVal);
        mat.SetFloat("_Maximum", maximumVal);
        RenderSettings.skybox = mat;
    }
}
