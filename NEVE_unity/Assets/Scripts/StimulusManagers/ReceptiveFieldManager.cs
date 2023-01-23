using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class ReceptiveFieldManager : GenericStimulusManager
{
    [Header("Specific stimulus parameters")]
    public float horizonHeight = 0f;
    public Color aboveHorizonColour = Color.grey;
    public Color belowHorizonColour = Color.white;


    [Header("Specific components")]
    public ReceptiveFieldStimulusController RFController;


    protected override void GetPropertiesFromPython() {
        // get generic stimulus parameters from python
        base.GetPropertiesFromPython();

        // now get those specific to this stimuli
        horizonHeight = GetFloatFromPython("horizonHeight", 0f);
        aboveHorizonColour = GetColorFromPython("aboveHorizonColour", aboveHorizonColour);
        belowHorizonColour = GetColorFromPython("belowHorizonColour", belowHorizonColour);
    }

    public override void SetupStimuli() {
        // skybox
        Material mat = RenderSettings.skybox;
        mat.SetFloat("_horizonHeight", horizonHeight);
        mat.SetColor("_aboveHorizonColour", aboveHorizonColour);
        mat.SetColor("_belowHorizonColour", belowHorizonColour);
        RenderSettings.skybox = mat;

        // controller
        RFController.imageColour = GetColorFromPython("colour", Color.white);
        RFController.bgColour = GetColorFromPython("bgColour", Color.black);
        RFController.number_of_columns = GetIntFromPython("numberOfColumns", 200);
        RFController.number_of_rows = GetIntFromPython("numberOfRows", 200);
        RFController.Stim_Size = GetFloatFromPython("stimSize", 10f);
        RFController.screenXpixels = GetIntFromPython("screenXpixels", 100);
        RFController.screenYpixels = GetIntFromPython("screenYpixels", 100);
        // use provided float or derive from current time
        RFController.seed = GetIntFromPython("seed", (int)System.DateTime.Now.Ticks);
        RFController.speed = GetFloatFromPython("speed", 30f);

        RFController.Reset();
    }
}
