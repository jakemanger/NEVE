# Getting started: Unity setup
To set up unity with NEVE, follow the below steps:

1) Clone this repository into a local directory.

2) Ensure you are using a unity version compatible with the project. With each update of Unity, new additions to code and the unity engine can cause breaking compatibility changes.
NEVE is currently built and tested with Unity 2020.3.3f1. 
If you currently don't have this Unity version, then install Unity Hub from https://unity3d.com/get-unity/download, click "Installs" > "ADD" and select this Unity version to install.

3) In Unity Hub, ensure you have the build support modules installed for all target platforms. Click "Installs", then the gear icon on your Unity installation, select "Add Modules", and enable **Linux Build Support (Mono)**, **Mac Build Support (Mono)**, and **Windows Build Support (Mono)** (Windows is included by default on Windows installs). These are required for the build scripts to compile without errors.

4) From Unity Hub, click "Projects" > "ADD" and select the NEVE_unity_HDR folder in the location of where you cloned this repository.

5) If you don't know how to use Unity, follow some of its [amazing guides](https://learn.unity.com/) or find some guides elsewhere (e.g. youtube) to get familiar with all the windows, settings and how everything works. You should have a general understanding of how the engine works, how to place game objects in the scene and how to alter components on game objects.

6) Then get started tinkering. Example scenes for experiments you can view and run are found in the [NEVE_unity/Assets/Scenes](/NEVE_unity/Assets/Scenes) folder. 
