using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class EightLoomManager : GenericStimulusManager
{
    [Header("Specific background stimulus parameters")]
    public float horizonHeight = 0f;
    public Color aboveHorizonColour = Color.grey;
    public Color belowHorizonColour = Color.white;

    [Header("Specific stimulus parameters")]
    public List<Vector3> startScales = new List<Vector3>();
    public List<Vector3> endScales = new List<Vector3>();
    public List<Vector3> rotations = new List<Vector3>();
    public List<Vector2> startPolarPositions = new List<Vector2>();
    public List<Vector2> endPolarPositions = new List<Vector2>();
    public List<Vector3> origins = new List<Vector3>();
    public List<Vector2> rotationOffsets = new List<Vector2>();
    public List<float> startDistances = new List<float>();
    public List<float> endDistances = new List<float>();
    public List<float> delayToApproaches = new List<float>();
    public List<float> numReps = new List<float>();
    public List<Color> stimulusColours = new List<Color>();
    public List<bool> opaqueObjects = new List<bool>();
    public List<float> stimulusDurations = new List<float>();
    public List<int> stimulusTypes = new List<int>();
    public List<bool> drawOutlines = new List<bool>();
    public List<float> outlineWidths = new List<float>();
    public List<Color> outlineColors = new List<Color>();
    public List<int> outlineTypes = new List<int>(); // 0 = world space fixed size, 1 = pixel space fixed size 
    public List<bool> fixedAngularSizes = new List<bool>();
    public List<bool> fixXAxes = new List<bool>(); // otherwise fix the Y axis
    public List<float> minAngularAngles = new List<float>();
    public List<float> maxAngularAngles = new List<float>();
    public List<float> delayToAppears = new List<float>();
    public List<bool> directPaths = new List<bool>();
    public List<bool> hideAtEnds = new List<bool>();

    [Header("General Grating Parameters")]
    public float gratingNum = 100f; // only used if the stimulusType has a grating material
    public bool gratingIsSquare = false;
    public float gratingMaxIntensity = 0.1f;
    public float gratingMinIntensity = 0f;

    protected override void GetPropertiesFromPython()
    {
        base.GetPropertiesFromPython();

        horizonHeight = GetFloatFromPython("horizonHeight", horizonHeight);
        aboveHorizonColour = GetColorFromPython("aboveHorizonColour", aboveHorizonColour);
        belowHorizonColour = GetColorFromPython("belowHorizonColour", belowHorizonColour);

        gratingNum = GetFloatFromPython("gratingNum", gratingNum);
        gratingIsSquare = GetBoolFromPython("gratingIsSquare", false);
        gratingMaxIntensity = GetFloatFromPython("gratingMaxIntensity", gratingMaxIntensity);
        gratingMinIntensity = GetFloatFromPython("gratingMinIntensity", gratingMinIntensity);

        for (int i = 0; i < 8; i++)
        {
            startScales.Add(GetVector3FromPython("startScale", Vector3.one, (i + 1).ToString()));
            endScales.Add(GetVector3FromPython("endScale", Vector3.one, (i + 1).ToString()));
            rotations.Add(GetVector3FromPython("rotation", Vector3.zero, (i + 1).ToString()));
            stimulusDurations.Add(GetFloatFromPython("duration", 5f, (i + 1).ToString()));
            origins.Add(GetVector3FromPython("origin", Vector3.zero, (i + 1).ToString()));
            rotationOffsets.Add(GetVector2FromPython("rotationOffset", Vector2.zero, (i + 1).ToString()));
            startPolarPositions.Add(new Vector2(
                -1 * GetFloatFromPython("startElevation", 0f, (i + 1).ToString()),
                GetFloatFromPython("startAzimuth", 0f, (i + 1).ToString())));
            endPolarPositions.Add(new Vector2(
                -1 * GetFloatFromPython("endElevation", 0f, (i + 1).ToString()),
                GetFloatFromPython("endAzimuth", 0f, (i + 1).ToString())));
            startDistances.Add(GetFloatFromPython("startDistance", 50f, (i + 1).ToString()));
            endDistances.Add(GetFloatFromPython("endDistance", 1f, (i + 1).ToString()));
            delayToApproaches.Add(GetFloatFromPython("delayToApproach", 5f, (i + 1).ToString()));
            numReps.Add(GetFloatFromPython("numReps", 1f, (i + 1).ToString()));
            stimulusColours.Add(GetColorFromPython("stimulusColour", Color.white, (i + 1).ToString()));
            opaqueObjects.Add(GetBoolFromPython("opaqueObject", false, (i + 1).ToString()));
            stimulusTypes.Add(GetIntFromPython("stimulusType", 0, (i + 1).ToString()));
            drawOutlines.Add(GetBoolFromPython("drawOutline", false, (i + 1).ToString()));
            outlineWidths.Add(GetFloatFromPython("outlineWidth", 5f, (i + 1).ToString()));
            outlineColors.Add(GetColorFromPython("outlineColour", Color.black, (i + 1).ToString()));
            outlineTypes.Add(GetIntFromPython("outlineType", 0, (i + 1).ToString()));
            fixedAngularSizes.Add(GetBoolFromPython("fixedAngularSize", false, (i + 1).ToString()));
            fixXAxes.Add(GetBoolFromPython("fixElevation", false, (i + 1).ToString()));
            if (fixXAxes[i])
            {
                minAngularAngles.Add(-1 * GetFloatFromPython("maxAngularAngle", -30f, (i + 1).ToString()));
                maxAngularAngles.Add(-1 * GetFloatFromPython("minAngularAngle", 30f, (i + 1).ToString()));
            }
            else
            {
                minAngularAngles.Add(GetFloatFromPython("minAngularAngle", -30f, (i + 1).ToString()));
                maxAngularAngles.Add(GetFloatFromPython("maxAngularAngle", 30f, (i + 1).ToString()));
            }
            delayToAppears.Add(GetFloatFromPython("delayToAppear", 0f, (i + 1).ToString()));
            directPaths.Add(GetBoolFromPython("directPath", true, (i + 1).ToString()));
            hideAtEnds.Add(GetBoolFromPython("hideAtEnd", false, (i + 1).ToString()));
        }
    }

    public override void SetupStimuli()
    {
        // skybox
        Material mat = RenderSettings.skybox;
        mat.SetFloat("_horizonHeight", horizonHeight);
        mat.SetColor("_aboveHorizonColour", aboveHorizonColour);
        mat.SetColor("_belowHorizonColour", belowHorizonColour);
        RenderSettings.skybox = mat;

        SphericalStimulusGenerator[] stimGenerators = GameObject.FindObjectsOfType<SphericalStimulusGenerator>();

        for (int i = 0; i < stimGenerators.Length; i++)
        {
            var stimGenerator = stimGenerators[i];
            stimGenerator.flickerDuration = flickerDuration;
            stimGenerator.stimulusColour = stimulusColours[i];
            stimGenerator.opaqueObject = opaqueObjects[i];
            stimGenerator.startScale = startScales[i];
            stimGenerator.endScale = endScales[i];
            stimGenerator.rotation = rotations[i];
            stimGenerator.startDistance = startDistances[i];
            stimGenerator.endDistance = endDistances[i];
            stimGenerator.delayToApproach = delayToApproaches[i];
            stimGenerator.origin = origins[i];
            stimGenerator.rotationOffset = rotationOffsets[i];
            stimGenerator.startPolarPosition = startPolarPositions[i] + rotationOffsets[i];
            stimGenerator.endPolarPosition = endPolarPositions[i] + rotationOffsets[i];
            stimGenerator.numReps = numReps[i];
            stimGenerator.duration = stimulusDurations[i];
            stimGenerator.stimulusType = stimulusTypes[i];
            stimGenerator.drawOutline = drawOutlines[i];
            stimGenerator.outlineWidth = outlineWidths[i];
            stimGenerator.outlineColor = outlineColors[i];
            stimGenerator.outlineType = outlineTypes[i];
            stimGenerator.gratingNum = gratingNum;
            stimGenerator.gratingIsSquare = gratingIsSquare ? 1 : 0;
            stimGenerator.gratingMaxIntensity = gratingMaxIntensity;
            stimGenerator.gratingMinIntensity = gratingMinIntensity;
            stimGenerator.fixedAngularSize = fixedAngularSizes[i];
            stimGenerator.fixXAxis = fixXAxes[i]; // otherwise fix the Y axis
            stimGenerator.minAngularAngle = minAngularAngles[i];
            stimGenerator.maxAngularAngle = maxAngularAngles[i];
            stimGenerator.manualControl = manualControl;
            stimGenerator.mouseMoveSpeed = mouseMoveSpeed;
            stimGenerator.delayToAppear = delayToAppears[i];
            stimGenerator.directPath = directPaths[i];
            stimGenerator.hideAtEnd = hideAtEnds[i];

            stimGenerator.Reset();
        }
    }
}
