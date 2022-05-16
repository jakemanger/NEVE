using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class MovingRectangleManager : GenericStimulusManager
{
    [Header("Specific stimulus parameters")]
    public float horizonHeight = 0f;
    public Color aboveHorizonColour = Color.grey;
    public Color belowHorizonColour = Color.white;

    public float width = 100f;
    public float height = 100f;
    public Vector2 startPos = Vector2.zero;
    public Vector2 endPos = new Vector2(5f, 5f);
    public float duration;
    public float delayToApproach;
    public Color stimulusColour;
    public float numReps;

    [Header("Specific components")]
    public SquareStimulusController squareController;


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

        width = floatChannel.GetWithDefault("width", 5f);
        height = floatChannel.GetWithDefault("height", 5f);
        float startPosX = floatChannel.GetWithDefault("startPosX", 0f);
        float startPosY = floatChannel.GetWithDefault("startPosY", 0f);
        startPos = new Vector2(startPosX, startPosY);
        float endPosX = floatChannel.GetWithDefault("endPosX", 5f);
        float endPosY = floatChannel.GetWithDefault("endPosY", 5f);
        endPos = new Vector2(endPosX, endPosY);
        numReps = floatChannel.GetWithDefault("numReps", 2f);

        duration = floatChannel.GetWithDefault("duration", 5f);
        delayToApproach = floatChannel.GetWithDefault("delayToApproach", 5f);
        r = floatChannel.GetWithDefault("stimulusColourR", 0.1f);
        g = floatChannel.GetWithDefault("stimulusColourG", 0.1f);
        b = floatChannel.GetWithDefault("stimulusColourB", 0.1f);
        a = floatChannel.GetWithDefault("stimulusColourA", 1f);
        stimulusColour = new Color(r, g, b, a);
    }

    public override void SetupStimuli() {
        // skybox
        Material mat = RenderSettings.skybox;
        mat.SetFloat("_horizonHeight", horizonHeight);
        mat.SetColor("_aboveHorizonColour", aboveHorizonColour);
        mat.SetColor("_belowHorizonColour", belowHorizonColour);
        RenderSettings.skybox = mat;

        // square
        squareController.width = width;
        squareController.height = height;
        squareController.startPos = startPos;
        squareController.endPos = endPos;
        squareController.duration = duration;
        squareController.delayToApproach = delayToApproach;
        squareController.stimulusColour = stimulusColour;
        squareController.numReps = numReps;

        squareController.Reset();
    }
}
