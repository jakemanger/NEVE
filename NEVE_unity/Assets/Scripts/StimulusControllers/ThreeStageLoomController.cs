using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeStageLoomController : GenericStimulusController
{
    SphericalStimulusGenerator stimGenerator;

    // delay before moving (hover)
    public float stage1Duration = 1f;
    public Vector2 stage1PolarPosition = Vector2.zero;
    public float stage1Distance = 10f;
    // to maintain equidistance from stage 1 to stage 2 when moving around animal
    public bool sphericalMovement = true; 
    // moving to new position (non-loom flight)
    public float stage2Duration = 1f;
    public Vector2 stage2PolarPosition = Vector2.zero;
    public float stage2Distance = 10f;
    // move to animal (loom)
    public float stage3Duration = 1f;
    public Vector2 stage3PolarPosition = Vector2.zero;
    public float stage3Distance = 1f;

    void Start()
    {
        stimGenerator = GetComponent<SphericalStimulusGenerator>();
    }

    public override void Reset()
    {
        stimGenerator.Reset();
    }

    void Update()
    {
        // move from stage 1 to stage 2
        if (Input.GetKeyDown(KeyCode.Space))
        {
            base.stimulusState = StimulusState.Started;
            stimGenerator.delayToApproach = stage1Duration;
            stimGenerator.directPath = !sphericalMovement;
            stimGenerator.startPolarPosition = stage1PolarPosition;
            stimGenerator.startDistance = stage1Distance;
            stimGenerator.endPolarPosition = stage2PolarPosition;
            stimGenerator.endDistance = stage2Distance;
            stimGenerator.duration = stage2Duration;
            stimGenerator.PrepareToMove();
        }
        // move from stage 2 to stage 3
        if (stimGenerator.stimulusState == StimulusState.Ended && base.stimulusState != StimulusState.Ended) {
            stimGenerator.directPath = !sphericalMovement;
            stimGenerator.startPolarPosition = stage2PolarPosition;
            stimGenerator.startDistance = stage2Distance;
            stimGenerator.endPolarPosition = stage3PolarPosition;
            stimGenerator.endDistance = stage3Distance;
            stimGenerator.duration = stage3Duration;
            stimGenerator.PrepareToMove();
        }
    }
}
