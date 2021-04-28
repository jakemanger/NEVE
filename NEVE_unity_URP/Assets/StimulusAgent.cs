using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

public class StimulusAgent : Agent {

    public float stimulusDuration = 10f;
    float timeSinceStimulusStart = 0f;

    public FiddlerCrabArenaManager manager;
    
    // an extra variable to make sure we start counting
    // time from the start of the episode, not the end of the
    // last episode (which will be default behaviour with Time.deltaTime)
    bool ranFirstFrame = false; 

    public override void OnEpisodeBegin() {
        // used for initialising and resetting the environment
        timeSinceStimulusStart = 0f;
        manager.Setup();
        print("OnEpisodeBegin");
        ranFirstFrame = false;
    }

    void Update() {
        if (ranFirstFrame) {
            timeSinceStimulusStart += Time.deltaTime;
            if (timeSinceStimulusStart >= stimulusDuration) {
                print("endepisode");
                EndEpisode();
                RequestDecision(); // gives control back to python until env.step() or env.reset() is called
                ranFirstFrame = false;
            }
        }
        ranFirstFrame = true;
        print(timeSinceStimulusStart);
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(1);
    }

    public override void OnActionReceived(float[] vectorAction) {
        SetReward(1f);
    }


    // public override void CollectObservations(VectorSensor sensor) {
    // }
    // public override void OnActionReceived(float[] vectorAction) {
    // }
    // public override void Heuristic(float[] actionsOut) {
    // }
}
