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
    public int square = 0;
    public float minimumVal = 0f;
    public float maximumVal = 0.5f;


    protected override void GetPropertiesFromPython() {
        // get generic stimulus parameters from python
        base.GetPropertiesFromPython();

        // now get those specific to this stimuli
        // load properties from python
        var floatChannel = Academy.Instance.EnvironmentParameters;
        // set properties from python
        density = floatChannel.GetWithDefault("density", 5f);
        offset = floatChannel.GetWithDefault("offset", 0f);
        angle = floatChannel.GetWithDefault("angle", 0f);
        speed = floatChannel.GetWithDefault("speed", 2f);
        square = (int)floatChannel.GetWithDefault("square", 5f);
        minimumVal = floatChannel.GetWithDefault("minimumVal", 0f);
        maximumVal = floatChannel.GetWithDefault("maximumVal", 0.5f);
    }

    public override void SetupStimuli() {
        Material mat = RenderSettings.skybox;
        mat.SetFloat("_Density", density);
        mat.SetFloat("_Offset", offset);
        mat.SetFloat("_Angle", angle);
        mat.SetFloat("_Speed", speed);
        mat.SetInt("_Square", square);
        mat.SetFloat("_Minimum", minimumVal);
        mat.SetFloat("_Maximum", maximumVal);
        RenderSettings.skybox = mat;
    }
}
