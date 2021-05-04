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

    public FiddlerCrabArenaManager fcmanager;
    public HyperiidManualControlArenaManager hmcmanager;
    public OptomotorManager optmanager;
    public HyperiidDualStimulusArenaManager hdsmanager;

    public override void OnEpisodeBegin() {
        // used for initialising and resetting the environment
        timeSinceStimulusStart = 0f;
        if (fcmanager != null) {
            fcmanager.Reset();
        }
        if (hmcmanager != null) {
            hmcmanager.Reset();
        }
        if (optmanager != null) {
            optmanager.Reset();
        }
        if (hdsmanager != null) {
            hdsmanager.Reset();
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        timeSinceStimulusStart += Time.deltaTime;
        if (timeSinceStimulusStart >= experimentDuration || Input.GetKeyDown(KeyCode.Escape)) {
            RequestDecision(); // gives control back to python until env.step() or env.reset() is called
            EndEpisode();
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // public override void CollectObservations(VectorSensor sensor) {
    // }
    // public override void OnActionReceived(float[] vectorAction) {
    // }
    // public override void Heuristic(float[] actionsOut) {
    // }
}
