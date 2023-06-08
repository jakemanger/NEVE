# Creating a custom experiment

When creating a custom experiment you can either: 1) modify a similar pre-built experiment or 2) create a new experiment from scratch.
We will go through both scenarios below.

## Modify a pre-built experiment

### Setup

1. Setup Unity and the NEVE_unity project for development by following:
[Setting up unity](getting_started/unity_setup.md)

2. Open the NEVE_unity project. A rough guide of the sections can be seen in the diagram below.

![image](https://github.com/jakemanger/NEVE/assets/52495554/8607e510-5b13-4f88-830b-3550bac85c73)


3. Once open in Unity, in the "Project Window" navigate to Scenes > 3d Scenes and select a scene (an experiment) that you would like to modify. You can copy and paste this scene before you open it and rename it to make a new scene.

4. Setup python and the NEVE_python project for development by following:
[Starting an experiment from python](starting_an_experiment_from_python.md)

5. Make sure you have opened a config file in step 4 that matches the scene you opened in Unity. Update the `buildDir` field in this config file to `None`.
```
# before: buildDir: ./builds/Optomotor/
buildDir: None
```
This will let the python part of NEVE know that you are interacting with the Unity editor.

6. To test your setup, start up the python part of NEVE, specifying your config file:
e.g.
```
python control_simulation.py --ignore-gooey optomotor.yaml
```
and then press the big play button in the Unity editor.

You should now be able to test the scene, just like when it is built.

### Modifying the code

There are two main parts of every experiment scene that you should be aware of: 1) the Stimulus Manager and 2) the Stimulus Controller.

The Stimulus Manager is responsible for loading in parameters from your config file that it receives from the python part of NEVE and then it sends these parameters to the rest of the parts of the scene. The Stimulus Controller is one part of the scene that receives parameters from the Stimulus Manager.

Stimulus Managers can be found in: `Scripts/StimulusManagers` 
and Stimulus Controllers can be found in: `Scripts/StimulusControllers/`.

Each of these found in your open experiment scene can be found on objects that you can click on in the Editors "Heirarchy Menu". Once you have selected an object, you can see the Scripts attached to that object in the "Inspector Window". Double click on the type section of a stimulus manager or controller to edit it. It is a good idea to copy, paste and rename each one if you are making breaking changes that may modify another experiment.

For some simple experiments, e.g. an Optomotor experiment, you only need to modify the StimulusManager. Stimulus managers have two main functions: `GetPropertiesFromPython()` and `SetupStimuli()`, which load properties from python and setup the stimulus in the scene, respectively. 

In the Optomotor Manager (the stimulus manager for the optomotor experiment), these are the following:

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class OptomotorManager : GenericStimulusManager
{

    [Header("Specific stimulus parameters")]
    public float density = 5f; // density of sine waves
    public float offset = 0f;
    public float angle = 0f;
    public float speed = 2f;
    public bool square = false;
    public float minimumVal = 0f;
    public float maximumVal = 0.5f;
    public float reverseAfterSeconds = 0f;
    public float timeWaitedForReverse = 0f;


    protected override void GetPropertiesFromPython() {
        // get generic stimulus parameters from python
        base.GetPropertiesFromPython();

        // now get those specific to this stimuli
        density = GetFloatFromPython("density", 5f);
        offset = GetFloatFromPython("offset", 0f);
        angle = GetFloatFromPython("angle", 0f);
        speed = GetFloatFromPython("speed", 2f);
        square = GetBoolFromPython("square", false);
        minimumVal = GetFloatFromPython("minimumVal", 0f);
        maximumVal = GetFloatFromPython("maximumVal", 0.5f);
        reverseAfterSeconds = GetFloatFromPython("reverseAfterSeconds", 0f);
    }

    public override void SetupStimuli() {
        Material mat = RenderSettings.skybox;
        mat.SetFloat("_Density", density);
        mat.SetFloat("_Offset", offset);
        mat.SetFloat("_Angle", angle);
        mat.SetFloat("_Speed", speed);
        mat.SetInt("_Square", square ? 1 : 0);
        mat.SetFloat("_Minimum", minimumVal);
        mat.SetFloat("_Maximum", maximumVal);
        RenderSettings.skybox = mat;
    }

    void Update() {
        base.Update();
        Material mat = RenderSettings.skybox;
        if (reverseAfterSeconds >= 0f) {
            timeWaitedForReverse += Time.deltaTime;
            if (timeWaitedForReverse > reverseAfterSeconds) {
                timeWaitedForReverse = 0f;
                speed = -speed;
                mat.SetFloat("_Speed", speed);
                RenderSettings.skybox = mat;
            }
        }
        float progress = mat.GetFloat("_progress");
        progress += Time.deltaTime * speed;
        mat.SetFloat("_progress", progress);
    }
}
```

You can modify this file however you would like. For example, you could modify the reversing implementation.

For more complex experiments, you will also need to modify the stimulus controller. In the Loom experiment, this is the SphericalStimulusGenerator.
Like other GenericStimulusGenerators, these have two main functions:
- `Reset()` This is called by the stimulus manager and is used to reset parts of the scene whenever the experiments conditions change and
- `Update()` This is called once per frame and is used to update objects in the scene.

If you would like to modify this stimulus generator, it would be advisable to first duplicate the file in the "Project Window" and then rename it. Make sure when you rename a .cs file, that you change the Class name in the file to match that of the filename, otherwise you will get an error in the console.


## Create a new experiment from scratch

TODO
