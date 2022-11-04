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

        crabController.pos = GetVector3FromPython("crabPos", new Vector3(0f, 0f, 50f));
        crabController.eyeHeight = GetFloatFromPython("eyeHeight", 0f);
        crabController.burrowColour = GetColorFromPython("burrowColour", Color.grey);
        crabController.crabType = GetIntFromPython("crabType", 0);
        crabController.crabSize = GetFloatFromPython("crabSize", 2f);
        crabController.sphereColour = GetColorFromPython("crabSphereColour", Color.white);
        crabController.rotationOffset = GetVector2FromPython("rotationOffset", Vector2.zero);

        crabController.Reset();
    }
}
