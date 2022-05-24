using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class LoomManager : GenericStimulusManager
{
    // A class for controlling looming stimuli

    [Header("Looming Background stimulus parameters")]
    public float horizonHeight = 0f;
    public Color aboveHorizonColour = Color.grey;
    public Color belowHorizonColour = Color.white;

    [Header("Looming transform parameters")]
    public Vector3 startScale = Vector3.one;
    public Vector3 endScale = Vector3.one;
    public Vector2 stimulusPolarPosition = new Vector2(0f, 0f);
    public Vector3 targetLocationOffset = new Vector3(0f, 0f, 0f);
    public float startOffset = 10f;
    public float endOffset = 10f;
    public float duration = 1f; // units (cm) per second
    public float delayToApproach = 5f;
    public bool fixedAngularSize = false;
    public bool fixXAxis = true; // otherwise fix the Y axis
    public float minAngularAngle = -30f;
    public float maxAngularAngle = 30f;

    [Header("Looming object appearance parameters")]
    public int stimulusType = 0; // 0 = icosphere, 1 = unity cube
    public Color stimulusColour = Color.white;

    // settings if stimulus type is 2 (grating stimulus)
    public float gratingNum = 100f;
    public int gratingIsSquare = 0;
    public float gratingMaxIntensity = 0.1f;
    public float gratingMinIntensity = 0f;

    public bool drawOutline = false;
    public float outlineWidth = 5f;
    public Color outlineColor = Color.black;

    [Header("Looming Components")]
    public SphericalStimulusGenerator stimGenerator;

    protected override void GetPropertiesFromPython() {
        // get generic stimulus parameters from python
        base.GetPropertiesFromPython();

        // now get those specific to this stimuli
        // load properties from python
        var floatChannel = Academy.Instance.EnvironmentParameters;
        // set properties from python
        float stimulusPolarPositionX = floatChannel.GetWithDefault("stimulusPolarPositionX", 0f);
        float stimulusPolarPositionY = floatChannel.GetWithDefault("stimulusPolarPositionY", 0f);
        stimulusPolarPosition = new Vector2(stimulusPolarPositionX, stimulusPolarPositionY);
        float targetLocationOffsetX = floatChannel.GetWithDefault("targetLocationOffsetX", 0f);
        float targetLocationOffsetY = floatChannel.GetWithDefault("targetLocationOffsetY", 0f);
        float targetLocationOffsetZ = floatChannel.GetWithDefault("targetLocationOffsetZ", 0f);
        targetLocationOffset = new Vector3(targetLocationOffsetX, targetLocationOffsetY, targetLocationOffsetZ);
        startOffset = floatChannel.GetWithDefault("startOffset", 50f);
        endOffset = floatChannel.GetWithDefault("endOffset", 1f);
        stimulusType = (int)floatChannel.GetWithDefault("stimulusType", 0); // 0 = icosphere, 1 = unity cube
        drawOutline = floatChannel.GetWithDefault("drawOutline", 0) != 0;
        outlineWidth = floatChannel.GetWithDefault("outlineWidth", 5f);
        float r = floatChannel.GetWithDefault("outlineColourR", 0f);
        float g = floatChannel.GetWithDefault("outlineColourG", 0f);
        float b = floatChannel.GetWithDefault("outlineColourB", 0f);
        float a = floatChannel.GetWithDefault("outlineColourA", 1f);
        outlineColor = new Color(r, g, b, a);
        r = floatChannel.GetWithDefault("stimulusColourR", 0.1f);
        g = floatChannel.GetWithDefault("stimulusColourG", 0.1f);
        b = floatChannel.GetWithDefault("stimulusColourB", 0.1f);
        a = floatChannel.GetWithDefault("stimulusColourA", 1f);
        stimulusColour = new Color(r, g, b, a);
        horizonHeight = floatChannel.GetWithDefault("horizonHeight", 0f);
        r = floatChannel.GetWithDefault("aboveHorizonColourR", 0.1f);
        g = floatChannel.GetWithDefault("aboveHorizonColourG", 0.1f);
        b = floatChannel.GetWithDefault("aboveHorizonColourB", 0.1f);
        a = floatChannel.GetWithDefault("aboveHorizonColourA", 1f);
        aboveHorizonColour = new Color(r, g, b, a);
        r = floatChannel.GetWithDefault("belowHorizonColourR", 0.1f);
        g = floatChannel.GetWithDefault("belowHorizonColourG", 0.1f);
        b = floatChannel.GetWithDefault("belowHorizonColourB", 0.1f);
        a = floatChannel.GetWithDefault("belowHorizonColourA", 1f);
        belowHorizonColour = new Color(r, g, b, a);
        gratingNum = floatChannel.GetWithDefault("gratingNum", 100f);
        gratingIsSquare = (int)floatChannel.GetWithDefault("gratingIsSquare", 0f);
        gratingMaxIntensity = floatChannel.GetWithDefault("gratingMaxIntensity", 0.1f);
        gratingMinIntensity = floatChannel.GetWithDefault("gratingMinIntensity", 0f);
        float x = floatChannel.GetWithDefault("startScaleX", 1f);
        float y = floatChannel.GetWithDefault("startScaleY", 1f);
        float z = floatChannel.GetWithDefault("startScaleZ", 1f);
        startScale = new Vector3(x, y, z);
        x = floatChannel.GetWithDefault("endScaleX", 1f);
        y = floatChannel.GetWithDefault("endScaleY", 1f);
        z = floatChannel.GetWithDefault("endScaleZ", 1f);
        endScale = new Vector3(x, y, z);
        duration = floatChannel.GetWithDefault("duration", 1f);
        fixedAngularSize = floatChannel.GetWithDefault("fixedAngularSize", 0) != 0;
        fixXAxis = floatChannel.GetWithDefault("fixXAxis", 1) != 0; // otherwise fix the Y axis
        minAngularAngle = floatChannel.GetWithDefault("minAngularAngle", -30f);
        maxAngularAngle = floatChannel.GetWithDefault("maxAngularAngle", 30f);
        delayToApproach = floatChannel.GetWithDefault("delayToApproach", 5f);
    }

    public override void SetupStimuli() {
        // skybox
        Material mat = RenderSettings.skybox;
        mat.SetFloat("_horizonHeight", horizonHeight);
        mat.SetColor("_aboveHorizonColour", aboveHorizonColour);
        mat.SetColor("_belowHorizonColour", belowHorizonColour);
        RenderSettings.skybox = mat;

        // sphere
        stimGenerator.stimulusColour = stimulusColour;
        stimGenerator.startScale = startScale;
        stimGenerator.endScale = endScale;
        stimGenerator.startPolarPosition = stimulusPolarPosition;
        stimGenerator.endPolarPosition = stimulusPolarPosition;
        stimGenerator.startOffset = startOffset;
        stimGenerator.endOffset = endOffset;
        stimGenerator.delayToApproach = delayToApproach;
        stimGenerator.targetLocationOffset = targetLocationOffset;
        stimGenerator.flickerDuration = base.flickerDuration;
        stimGenerator.numReps = 0.5f;
        stimGenerator.stimulusType = stimulusType;
        stimGenerator.drawOutline = drawOutline;
        stimGenerator.outlineWidth = outlineWidth;
        stimGenerator.outlineColor = outlineColor;
        stimGenerator.gratingNum = gratingNum;
        stimGenerator.gratingIsSquare = gratingIsSquare;
        stimGenerator.gratingMaxIntensity = gratingMaxIntensity;
        stimGenerator.gratingMinIntensity = gratingMinIntensity;
        stimGenerator.fixedAngularSize = fixedAngularSize;
        stimGenerator.fixXAxis = fixXAxis; // otherwise fix the Y axis
        stimGenerator.minAngularAngle = minAngularAngle;
        stimGenerator.maxAngularAngle = maxAngularAngle;

        stimGenerator.duration = duration; 

        stimGenerator.manualControl = manualControl;
        stimGenerator.mouseMoveSpeed = mouseMoveSpeed;
        stimGenerator.Reset();
    }
}
