using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

// used to control when an episode/experiment starts or is finished
public class EpisodeControllerAgent : Agent {

    public float experimentDuration = 99999f; // duration in seconds
    float timeSinceStimulusStart = 0f;

    public FiddlerCrabLoomingStimulusArenaManager fcmanager;
    public HyperiidDualStimulusArenaManager hdsmanager;
    public OptomotorManager optmanager;

    public override void OnEpisodeBegin() {
        // used for initialising and resetting the environment
        timeSinceStimulusStart = 0f;
        if (fcmanager != null) {
            fcmanager.Reset();
        }
        if (optmanager != null) {
            optmanager.Reset();
        }
        if (hdsmanager != null) {
            hdsmanager.Reset();
        }
        Cursor.visible = false;
    }

    void Update() {
        timeSinceStimulusStart += Time.deltaTime;
        if (timeSinceStimulusStart >= experimentDuration || Input.GetKeyDown(KeyCode.Escape)) {
            RequestDecision(); // gives control back to python until env.step() or env.reset() is called
            EndEpisode();
            Cursor.visible = true;
        }
    }

    // public override void CollectObservations(VectorSensor sensor) {
    // }
    // public override void OnActionReceived(float[] vectorAction) {
    // }
    // public override void Heuristic(float[] actionsOut) {
    // }
}
