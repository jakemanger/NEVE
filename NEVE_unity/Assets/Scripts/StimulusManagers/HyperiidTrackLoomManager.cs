using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class HyperiidTrackLoomManager : LoomManager
{
    bool randomXRot = false;
    bool randomYRot = false;

    Vector2 randomMin = new Vector2(0f, 0f);
    Vector2 randomMax = new Vector2(360f, 360f);
    
    public override void SetupStimuli() {
        base.SetupStimuli();

        // get different random numbers each time you run the experiment
        Random.InitState((int)System.DateTime.Now.Ticks);

        // override the default loom manager setup
        // if specified as using random in the config file
        randomXRot = GetBoolFromPython("useRandomRotationOffsetX", false);
        randomYRot = GetBoolFromPython("useRandomRotationOffsetY", false);

        randomMin = GetVector2FromPython("randomRotationOffsetMin", new Vector2(0f, 0f));
        randomMax = GetVector2FromPython("randomRotationOffsetMax", new Vector2(360f, 360f));

        if (randomXRot) {
            float xRot = Random.Range(0, 4) * 90f;
            base.stimGenerator.startPolarPosition.x += xRot;
            base.stimGenerator.endPolarPosition.x += xRot;
        }
        if (randomYRot) {
            float yRot = Random.Range(0, 4) * 90f;
            base.stimGenerator.startPolarPosition.y += yRot;
            base.stimGenerator.endPolarPosition.y += yRot;
        }

        base.stimGenerator.Reset();
    }
}
