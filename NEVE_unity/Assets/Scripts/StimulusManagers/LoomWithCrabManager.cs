using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class LoomWithCrabManager : LoomManager
{
    CrabMovementController crabController;

    void Start() {
        crabController = GameObject.FindObjectOfType<CrabMovementController>();
    }

    public override void SetupStimuli() {
        base.SetupStimuli();

        crabController.pos = GetVector3FromPython("crabPos", new Vector3(0, 0, 60f));
        crabController.eyeHeight = GetFloatFromPython("eyeHeight", 2f);
        crabController.burrowColour = GetColorFromPython("burrowColour", Color.grey);
        crabController.crabType = GetIntFromPython("crabType", 0);
        crabController.crabSize = GetFloatFromPython("crabSize", 1f);
        crabController.sphereColour = GetColorFromPython("crabSphereColour", Color.white);

        crabController.Reset();
    }
}
