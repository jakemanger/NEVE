using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class DualLoomManager : GenericStimulusManager
{
    [Header("Specific background stimulus parameters")]
    public float horizonHeight = 0f;
    public Color aboveHorizonColour = Color.grey;
    public Color belowHorizonColour = Color.white;

    [Header("Specific stimulus parameters")]
    // public Color frontBackgroundColour = new Color(0f, 0f, 0f, 1f);
    // public Color rightBackgroundColour = new Color(0f, 0f, 0f, 1f);
    // public Color backBackgroundColour = new Color(0f, 0f, 0f, 1f);
    // public Color leftBackgroundColour = new Color(0f, 0f, 0f, 1f);
    public Vector3 startScale1 = Vector3.one;
    public Vector3 endScale1 = Vector3.one;
    public Vector3 startScale2 = Vector3.one;
    public Vector3 endScale2 = Vector3.one;
    public Vector2 startPolarPosition1 = new Vector2(0f, 0f);
    public Vector2 startPolarPosition2 = new Vector2(0f, 0f);
    public Vector2 endPolarPosition1 = new Vector2(0f, 0f);
    public Vector2 endPolarPosition2 = new Vector2(0f, 0f);
    public Vector3 targetLocationOffset1 = new Vector3(0f, 0f, 0f);
    public Vector3 targetLocationOffset2 = new Vector3(0f, 0f, 0f);
    public float startOffset1 = 10f;
    public float startOffset2 = 10f;
    public float endOffset1 = 10f;
    public float endOffset2 = 10f;
    public float delayToApproach1 = 5f;
    public float delayToApproach2 = 5f;
    public float numReps1 = 2;
    public float numReps2 = 2;
    public Color stimulusColour1 = Color.white;
    public Color stimulusColour2 = Color.white;
    public float stimulusDuration1 = 5f;
    public float stimulusDuration2 = 5f;
    public int stimulusType1 = 0;
    public bool drawOutline1 = false;
    public float outlineWidth1 = 5f;
    public Color outlineColor1 = Color.black;
    public int stimulusType2 = 0;
    public bool drawOutline2 = false;
    public float outlineWidth2 = 5f;
    public Color outlineColor2 = Color.black;
    public float gratingNum = 100f; // only used if the stimulusType has a grating material
    public int gratingIsSquare = 0;
    public float gratingMaxIntensity = 0.1f;
    public float gratingMinIntensity = 0f;


    protected override void GetPropertiesFromPython() {
        // get generic stimulus parameters from python
        base.GetPropertiesFromPython();

        // now get those specific to this stimuli
        // load properties from python
        var floatChannel = Academy.Instance.EnvironmentParameters;
        // set properties from python
        horizonHeight = floatChannel.GetWithDefault("horizonHeight", 0f);
        float r = floatChannel.GetWithDefault("aboveHorizonColourR", 0.1f);
        float g = floatChannel.GetWithDefault("aboveHorizonColourG", 0.1f);
        float b = floatChannel.GetWithDefault("aboveHorizonColourB", 0.1f);
        float a = floatChannel.GetWithDefault("aboveHorizonColourA", 1f);
        aboveHorizonColour = new Color(r, g, b, a);
        r = floatChannel.GetWithDefault("belowHorizonColourR", 0.1f);
        g = floatChannel.GetWithDefault("belowHorizonColourG", 0.1f);
        b = floatChannel.GetWithDefault("belowHorizonColourB", 0.1f);
        a = floatChannel.GetWithDefault("belowHorizonColourA", 1f);
        belowHorizonColour = new Color(r, g, b, a);

        float x = floatChannel.GetWithDefault("startScaleX1", 1f);
        float y = floatChannel.GetWithDefault("startScaleY1", 1f);
        float z = floatChannel.GetWithDefault("startScaleZ1", 1f);
        startScale1 = new Vector3(x, y, z);
        x = floatChannel.GetWithDefault("endScaleX1", 1f);
        y = floatChannel.GetWithDefault("endScaleY1", 1f);
        z = floatChannel.GetWithDefault("endScaleZ1", 1f);
        endScale1 = new Vector3(x, y, z);
        stimulusDuration1 = floatChannel.GetWithDefault("stimulusDuration1", 5f);
        float startPolarPositionX1 = floatChannel.GetWithDefault("startPolarPositionX1", 0f);
        float startPolarPositionY1 = floatChannel.GetWithDefault("startPolarPositionY1", 0f);
        startPolarPosition1 = new Vector2(startPolarPositionX1, startPolarPositionY1);
        float endPolarPositionX1 = floatChannel.GetWithDefault("endPolarPositionX1", 0f);
        float endPolarPositionY1 = floatChannel.GetWithDefault("endPolarPositionY1", 0f);
        endPolarPosition1 = new Vector2(endPolarPositionX1, endPolarPositionY1);
        float targetLocationOffsetX1 = floatChannel.GetWithDefault("targetLocationOffsetX1", 0f);
        float targetLocationOffsetY1 = floatChannel.GetWithDefault("targetLocationOffsetY1", 0f);
        float targetLocationOffsetZ1 = floatChannel.GetWithDefault("targetLocationOffsetZ1", 0f);
        targetLocationOffset1 = new Vector3(targetLocationOffsetX1, targetLocationOffsetY1, targetLocationOffsetZ1);
        startOffset1 = floatChannel.GetWithDefault("startOffset1", 50f);
        endOffset1 = floatChannel.GetWithDefault("endOffset1", 1f);
        delayToApproach1 = floatChannel.GetWithDefault("delayToApproach1", 5f);
        numReps1 = floatChannel.GetWithDefault("numReps1", 1f);
        r = floatChannel.GetWithDefault("stimulusColourR1", 0.1f);
        g = floatChannel.GetWithDefault("stimulusColourG1", 0.1f);
        b = floatChannel.GetWithDefault("stimulusColourB1", 0.1f);
        a = floatChannel.GetWithDefault("stimulusColourA1", 1f);
        stimulusColour1 = new Color(r, g, b, a);
        stimulusType1 = (int)floatChannel.GetWithDefault("stimulusType1", 0); // 0 = icosphere, 1 = unity cube
        drawOutline1 = floatChannel.GetWithDefault("drawOutline1", 0) != 0;
        outlineWidth1 = floatChannel.GetWithDefault("outlineWidth1", 5f);
        r = floatChannel.GetWithDefault("outlineColourR1", 0f);
        g = floatChannel.GetWithDefault("outlineColourG1", 0f);
        b = floatChannel.GetWithDefault("outlineColourB1", 0f);
        a = floatChannel.GetWithDefault("outlineColourA1", 1f);
        outlineColor1 = new Color(r, g, b, a);

        x = floatChannel.GetWithDefault("startScaleX2", 1f);
        y = floatChannel.GetWithDefault("startScaleY2", 1f);
        z = floatChannel.GetWithDefault("startScaleZ2", 1f);
        startScale2 = new Vector3(x, y, z);
        x = floatChannel.GetWithDefault("endScaleX2", 1f);
        y = floatChannel.GetWithDefault("endScaleY2", 1f);
        z = floatChannel.GetWithDefault("endScaleZ2", 1f);
        endScale2 = new Vector3(x, y, z);
        stimulusDuration2 = floatChannel.GetWithDefault("stimulusDuration2", 5f);
        float startPolarPositionX2 = floatChannel.GetWithDefault("startPolarPositionX2", 0f);
        float startPolarPositionY2 = floatChannel.GetWithDefault("startPolarPositionY2", 0f);
        startPolarPosition2 = new Vector2(startPolarPositionX2, startPolarPositionY2);
        float endPolarPositionX2 = floatChannel.GetWithDefault("endPolarPositionX2", 0f);
        float endPolarPositionY2 = floatChannel.GetWithDefault("endPolarPositionY2", 0f);
        endPolarPosition2 = new Vector2(endPolarPositionX2, endPolarPositionY2);
        float targetLocationOffsetX2 = floatChannel.GetWithDefault("targetLocationOffsetX2", 0f);
        float targetLocationOffsetY2 = floatChannel.GetWithDefault("targetLocationOffsetY2", 0f);
        float targetLocationOffsetZ2 = floatChannel.GetWithDefault("targetLocationOffsetZ2", 0f);
        targetLocationOffset2 = new Vector3(targetLocationOffsetX2, targetLocationOffsetY2, targetLocationOffsetZ2);
        startOffset2 = floatChannel.GetWithDefault("startOffset2", 50f);
        endOffset2 = floatChannel.GetWithDefault("endOffset2", 1f);
        delayToApproach2 = floatChannel.GetWithDefault("delayToApproach2", 5f);
        numReps2 = floatChannel.GetWithDefault("numReps2", 1f);
        r = floatChannel.GetWithDefault("stimulusColourR2", 0.1f);
        g = floatChannel.GetWithDefault("stimulusColourG2", 0.1f);
        b = floatChannel.GetWithDefault("stimulusColourB2", 0.1f);
        a = floatChannel.GetWithDefault("stimulusColourA2", 1f);
        stimulusColour2 = new Color(r, g, b, a);
        stimulusType2 = (int)floatChannel.GetWithDefault("stimulusType1", 0); // 0 = icosphere, 1 = unity cube
        drawOutline2 = floatChannel.GetWithDefault("drawOutline1", 0) != 0;
        outlineWidth2 = floatChannel.GetWithDefault("outlineWidth1", 5f);
        r = floatChannel.GetWithDefault("outlineColourR1", 0f);
        g = floatChannel.GetWithDefault("outlineColourG1", 0f);
        b = floatChannel.GetWithDefault("outlineColourB1", 0f);
        a = floatChannel.GetWithDefault("outlineColourA1", 1f);
        outlineColor2 = new Color(r, g, b, a);
        
        gratingNum = floatChannel.GetWithDefault("gratingNum", 100f);
        gratingIsSquare = (int)floatChannel.GetWithDefault("gratingIsSquare", 0f);
        gratingMaxIntensity = floatChannel.GetWithDefault("gratingMaxIntensity", 0.1f);
        gratingMinIntensity = floatChannel.GetWithDefault("gratingMinIntensity", 0f);
    }

    public override void SetupStimuli() {
        // skybox
        Material mat = RenderSettings.skybox;
        mat.SetFloat("_horizonHeight", horizonHeight);
        mat.SetColor("_aboveHorizonColour", aboveHorizonColour);
        mat.SetColor("_belowHorizonColour", belowHorizonColour);
        RenderSettings.skybox = mat;

        SphericalStimulusGenerator[] stimGenerators = GameObject.FindObjectsOfType<SphericalStimulusGenerator>();
        SphericalStimulusGenerator stimGenerator1 = stimGenerators[0];
        // stimulus 1
        stimGenerator1.flickerDuration = flickerDuration;
        stimGenerator1.stimulusColour = stimulusColour1;
        stimGenerator1.startScale = startScale1;
        stimGenerator1.endScale = endScale1;
        stimGenerator1.startOffset = startOffset1;
        stimGenerator1.endOffset = endOffset1;
        stimGenerator1.delayToApproach = delayToApproach1;
        stimGenerator1.targetLocationOffset = targetLocationOffset1;
        stimGenerator1.startPolarPosition = startPolarPosition1;
        stimGenerator1.endPolarPosition = endPolarPosition1;
        stimGenerator1.numReps = numReps1;
        stimGenerator1.duration = stimulusDuration1; 
        stimGenerator1.stimulusType = stimulusType1;
        stimGenerator1.drawOutline = drawOutline1;
        stimGenerator1.outlineWidth = outlineWidth1;
        stimGenerator1.outlineColor = outlineColor1;
        stimGenerator1.gratingNum = gratingNum;
        stimGenerator1.gratingIsSquare = gratingIsSquare;
        stimGenerator1.gratingMaxIntensity = gratingMaxIntensity;
        stimGenerator1.gratingMinIntensity = gratingMinIntensity;

        stimGenerator1.manualControl = manualControl;
        stimGenerator1.mouseMoveSpeed = mouseMoveSpeed;

        stimGenerator1.Reset();

        // stimulus 2
        if (stimGenerators.Length > 1) {
            SphericalStimulusGenerator stimGenerator2 = stimGenerators[1];
            stimGenerator2.flickerDuration = flickerDuration;
            stimGenerator2.stimulusColour = stimulusColour2;
            stimGenerator2.startScale = startScale2;
            stimGenerator2.endScale = endScale2;
            stimGenerator2.startOffset = startOffset2;
            stimGenerator2.endOffset = endOffset2;
            stimGenerator2.delayToApproach = delayToApproach2;
            stimGenerator2.targetLocationOffset = targetLocationOffset2;
            stimGenerator2.startPolarPosition = startPolarPosition2;
            stimGenerator2.endPolarPosition = endPolarPosition2;
            stimGenerator2.numReps = numReps2;
            stimGenerator2.duration = stimulusDuration2; 
            stimGenerator2.stimulusType = stimulusType2;
            stimGenerator2.drawOutline = drawOutline2;
            stimGenerator2.outlineWidth = outlineWidth2;
            stimGenerator2.outlineColor = outlineColor2;
            stimGenerator2.gratingNum = gratingNum;
            stimGenerator2.gratingIsSquare = gratingIsSquare;
            stimGenerator2.gratingMaxIntensity = gratingMaxIntensity;
            stimGenerator2.gratingMinIntensity = gratingMinIntensity;

            stimGenerator2.manualControl = manualControl;
            stimGenerator2.mouseMoveSpeed = mouseMoveSpeed;
            stimGenerator2.Reset();
        }
    }
}