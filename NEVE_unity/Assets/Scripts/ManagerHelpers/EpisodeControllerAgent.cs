using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.IO;
using UnityEngine.SceneManagement;

// used to control when an episode/experiment starts or is finished
public class EpisodeControllerAgent : Agent {

    public float experimentDuration = 99999f; // duration in seconds
    float timeSinceStimulusStart = 0f;

    public EnvironmentParameters floatChannel;

    public override void OnEpisodeBegin() {
        // check if a new scene needs to be loaded
        floatChannel = Academy.Instance.EnvironmentParameters;

        // if we need to switch scenes, switch now
        int scene = GetIntFromPython("scene", 0);
        Scene currentScene = SceneManager.GetActiveScene();
        print("Requested scene: " + scene);
        int currentSceneIndex = currentScene.buildIndex;
        print("Current scene: " + currentSceneIndex);
        if (scene != currentSceneIndex) {
            print("Changing scene!");
            SceneManager.LoadScene(scene);
            Academy.Instance.EnvironmentStep();
            return;
        }

        // used for initialising and resetting the environment
        timeSinceStimulusStart = 0f;
        GetComponent<GenericStimulusManager>().Reset();
        Cursor.visible = false;
    }

    void Update() {
        timeSinceStimulusStart += Time.deltaTime;
        if (timeSinceStimulusStart >= experimentDuration || Input.GetKeyDown(KeyCode.Escape)) {
            RequestDecision(); // gives control back to python until env.step() or env.reset() is called
            EndEpisode();
            Cursor.visible = false;
        }
    }

    // public override void CollectObservations(VectorSensor sensor) {
    // }
    // public override void OnActionReceived(float[] vectorAction) {
    // }
    // public override void Heuristic(float[] actionsOut) {
    // }
    int GetIntFromPython(string parameterName, int defaultValue, string extraSuffix = "") {
        string name = parameterName + extraSuffix;
        return (int)floatChannel.GetWithDefault(name, (float)defaultValue);
    }
}
