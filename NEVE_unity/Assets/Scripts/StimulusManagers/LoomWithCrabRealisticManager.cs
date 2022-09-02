using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class LoomWithCrabRealisticManager : LoomManager
{
    CrabMovementController crabController;
    public Transform floorTrans;

    void Start() {
        crabController = GameObject.FindObjectOfType<CrabMovementController>();
    }

    public override void SetupStimuli() {
        base.SetupStimuli();

        crabController.pos = GetVector3FromPython("crabPos", new Vector3(0, -base.eyeHeight, 50f));

        crabController.Reset();

        floorTrans.position = new Vector3(0, -base.eyeHeight, 0);
    }
}
