using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor.OSXStandalone;

public class BuildScript
{

    [MenuItem("File/Build All")]
    static void BuildAll()
    {
        var scenes = EditorBuildSettings.scenes;
        // log the scenes to be built
        Debug.Log("Scenes to be built (that were found in the build settings):");
        foreach (var scene in scenes)
        {
            Debug.Log(scene.path);
        }
        Debug.Log("If you want to change the scenes to be built, open the scene you want to add, go to File > Build Settings > Add Open Scene, and then click File > Build All again. If you want to remove a scene, right click it and click Remove selection.");

        Build(scenes, "../builds/", BuildTarget.StandaloneWindows, "Windows/NEVE_unity_urp.exe");
        Build(scenes, "../builds/", BuildTarget.StandaloneLinux64, "Linux/Linux.x86_64");
        // set use Intel 64-bit architecture
        UserBuildSettings.architecture = MacOSArchitecture.x64;
        Build(scenes, "../builds/", BuildTarget.StandaloneOSX, "Mac.app");
    }

    [MenuItem("File/Build Current Scene")]
    static void BuildCurrentScene()
    {
        var scenes = EditorBuildSettings.scenes;
        var currentScene = EditorSceneManager.GetActiveScene();

        // get the scene
        List<EditorBuildSettingsScene> currentSceneList = new List<EditorBuildSettingsScene>();
        foreach (var scene in scenes)
        {
            if (scene.path == currentScene.path)
            {
                currentSceneList.Add(scene);
            }
        }

        Build(currentSceneList.ToArray(), "../builds/", BuildTarget.StandaloneWindows, "Windows/NEVE_unity_urp.exe");
        Build(currentSceneList.ToArray(), "../builds/", BuildTarget.StandaloneLinux64, "Linux/Linux.x86_64");
        // set use Intel 64-bit architecture
        UserBuildSettings.architecture = MacOSArchitecture.x64;
        Build(currentSceneList.ToArray(), "../builds/", BuildTarget.StandaloneOSX, "Mac.app");
    } 

    static void Build(EditorBuildSettingsScene[] scenes, string buildDir, BuildTarget target, string targetName)
    {
        // build each scene as a seperate file
        for (int i = 0; i < scenes.Length; i++)
        {
            var scene = scenes[i];
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);
            Debug.Log(scene);
            var report = BuildPipeline.BuildPlayer(new[] { scene }, buildDir + sceneName + "/" + targetName, target, BuildOptions.None);
            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + report.summary.totalSize + " bytes");
            }
            if (report.summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }
    }

    static void PerformAssetBundleBuild()
    {
        BuildPipeline.BuildAssetBundles("../AssetBundles/", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneLinux64);
        BuildPipeline.BuildAssetBundles("../AssetBundles/", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
        BuildPipeline.BuildAssetBundles("../AssetBundles/", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneOSX);
    }
}