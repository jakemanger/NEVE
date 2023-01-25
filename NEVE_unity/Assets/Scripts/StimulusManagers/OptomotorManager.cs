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
    public float reverseAfterSeconds = 0f;
    public float timeWaitedForReverse = 0f;

    public bool onlyShowOneHalfCycle = false;


    protected override void GetPropertiesFromPython() {
        // get generic stimulus parameters from python
        base.GetPropertiesFromPython();

        // now get those specific to this stimuli
        density = GetFloatFromPython("density", 5f);
        offset = GetFloatFromPython("offset", 0f);
        angle = GetFloatFromPython("angle", 0f);
        speed = GetFloatFromPython("speed", -1f);
        square = GetBoolFromPython("square", false);
        minimumVal = GetFloatFromPython("minimumVal", 0f);
        maximumVal = GetFloatFromPython("maximumVal", 0.5f);
        reverseAfterSeconds = GetFloatFromPython("reverseAfterSeconds", 6f);
        onlyShowOneHalfCycle = GetBoolFromPython("onlyShowOneHalfCycle", false);
    }

    public override void SetupStimuli() {
        Material mat = RenderSettings.skybox;
        mat.SetFloat("_Density", density);
        if (offset < 0f) {
            offset += 10000f;
        }
        mat.SetFloat("_Offset", offset / 360);
        mat.SetFloat("_Angle", angle);
        mat.SetFloat("_Speed", speed);
        mat.SetInt("_Square", square ? 1 : 0);
        mat.SetFloat("_Minimum", minimumVal);
        mat.SetFloat("_Maximum", maximumVal);
        mat.SetInt("_OnlyShowOneHalfCycle", onlyShowOneHalfCycle ? 1 : 0);
        RenderSettings.skybox = mat;
    }

    void Update() {
        base.Update();
        Material mat = RenderSettings.skybox;
        if (reverseAfterSeconds > 0f) {
            timeWaitedForReverse += Time.deltaTime;
            if (timeWaitedForReverse > reverseAfterSeconds) {
                timeWaitedForReverse = 0f;
                speed = -speed;
                mat.SetFloat("_Speed", speed);
                RenderSettings.skybox = mat;
            }
        }
        float progress = mat.GetFloat("_progress");
        progress += Time.deltaTime * speed;
        // keep progress in safe range where all calculations of shader are valid
        if (progress < 0f) {
            progress += 1000f;
        }
        mat.SetFloat("_progress", progress);
    }
}
