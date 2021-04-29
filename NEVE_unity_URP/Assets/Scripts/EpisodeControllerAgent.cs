using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

// used to control when an episode/experiment starts or is finished
public class EpisodeControllerAgent : Agent {

    public float stimulusDuration = 50f; // duration in seconds
    float timeSinceStimulusStart = 0f;

    public FiddlerCrabArenaManager fcmanager;
    public HyperiidManualControlArenaManager hmcmanager;
    
    // an extra variable to make sure we start counting
    // time from the start of the episode, not the end of the
    // last episode (which will be default behaviour with Time.deltaTime)
    bool ranFirstFrame = false; 

    public override void OnEpisodeBegin() {
        // used for initialising and resetting the environment
        timeSinceStimulusStart = 0f;
        if (fcmanager != null) {
            fcmanager.Reset();
        }
        if (hmcmanager != null) {
            hmcmanager.Reset();
        }
        print("OnEpisodeBegin");
        ranFirstFrame = false;
    }

    void Update() {
        if (ranFirstFrame) {
            timeSinceStimulusStart += Time.deltaTime;
            if (timeSinceStimulusStart >= stimulusDuration || Input.GetKey(KeyCode.Escape)) {
                EndEpisode();
                ranFirstFrame = false;
                RequestDecision(); // gives control back to python until env.step() or env.reset() is called
            }
        }
        ranFirstFrame = true;
    }

    // public override void CollectObservations(VectorSensor sensor) {
    // }
    // public override void OnActionReceived(float[] vectorAction) {
    // }
    // public override void Heuristic(float[] actionsOut) {
    // }
}
