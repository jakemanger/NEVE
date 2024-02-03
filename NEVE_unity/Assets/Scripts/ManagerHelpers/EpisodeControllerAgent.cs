using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.IO;
using System.Reflection;
using UnityEngine.SceneManagement;

// used to control when an episode/experiment starts or is finished
public class EpisodeControllerAgent : Agent {

    public float experimentDuration = 99999f; // duration in seconds
    float timeSinceStimulusStart = 0f;

    public EnvironmentParameters floatChannel;


    public override void OnEpisodeBegin() {
        // used for initialising and resetting the environment
        timeSinceStimulusStart = 0f;

        // check if a new scene needs to be loaded
        floatChannel = Academy.Instance.EnvironmentParameters;
        Academy.Instance.AutomaticSteppingEnabled = false; // manually control steps as there appears to be a mlagents bug that sometimes causes steps to be called twice when switching scenes
        // if we need to switch scenes, switch now
        int scene = GetIntFromPython("scene", 0);
        Scene currentScene = SceneManager.GetActiveScene();
        int currentSceneIndex = currentScene.buildIndex;
        if ((int)scene != (int)currentSceneIndex) {
            print("Changing scene");
            SceneManager.LoadSceneAsync(scene);
            return;
        }
        Cursor.visible = false;

        print("Running reset");
        GetComponent<GenericStimulusManager>().Reset();
        Cursor.visible = false;
    }

    void Update() {
        timeSinceStimulusStart += Time.deltaTime;
        if (timeSinceStimulusStart >= experimentDuration || Input.GetKeyDown(KeyCode.Escape)) {
            ClearEnvironmentParameters();
            RequestDecision(); // gives control back to python until env.step() or env.reset() is called
            Academy.Instance.EnvironmentStep(); // manually control steps as there appears to be a mlagents bug that sometimes causes steps to be called twice when switching scenes
            EndEpisode();
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


    public static void ClearEnvironmentParameters()
    {
        // Access the EnvironmentParameters instance
        var envParameters = Academy.Instance.EnvironmentParameters;
        
        // Use reflection to get the 'm_Channel' field from the EnvironmentParameters instance
        var channelField = envParameters.GetType().GetField("m_Channel", BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (channelField == null)
        {
            throw new InvalidOperationException("Could not find the 'm_Channel' field.");
        }

        // Get the value of the 'm_Channel' field, which is an instance of EnvironmentParametersChannel
        var channel = channelField.GetValue(envParameters);

        if (channel == null)
        {
            throw new InvalidOperationException("The 'm_Channel' field is null.");
        }

        // Use reflection to access the 'm_Parameters' field within the EnvironmentParametersChannel
        var parametersField = channel.GetType().GetField("m_Parameters", BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (parametersField == null)
        {
            throw new InvalidOperationException("Could not find the 'm_Parameters' field.");
        }

        // Get the dictionary object from the 'm_Parameters' field
        var parameters = (System.Collections.Generic.Dictionary<string, Func<float>>)parametersField.GetValue(channel);
        
        // Clear the dictionary
        parameters.Clear();
    }
}
