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
        horizonHeight = GetFloatFromPython("horizonHeight", 0f);
        aboveHorizonColour = GetColorFromPython("aboveHorizonColour", aboveHorizonColour);
        belowHorizonColour = GetColorFromPython("belowHorizonColour", belowHorizonColour);
        width = GetFloatFromPython("width", 5f);
        height = GetFloatFromPython("height", 5f);
        startPos = GetVector2FromPython("startPos", startPos);
        endPos = GetVector2FromPython("endPos", endPos);
        numReps = GetFloatFromPython("numReps", 2f);
        duration = GetFloatFromPython("duration", 5f);
        delayToApproach = GetFloatFromPython("delayToApproach", 5f);
        stimulusColour = GetColorFromPython("stimulusColour", stimulusColour);
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
