using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor.OSXStandalone;

public class BuildScript
{
    [MenuItem("File/Build Current Scene")]
    static void BuildCurrentScene()
    {
        var currentScene = EditorSceneManager.GetActiveScene().path;

        // Build only the current scene
        Build(new[] { currentScene }, "../builds/", BuildTarget.StandaloneWindows, "Windows/NEVE_unity_urp.exe");
        Build(new[] { currentScene }, "../builds/", BuildTarget.StandaloneLinux64, "Linux/Linux.x86_64");
        UserBuildSettings.architecture = MacOSArchitecture.x64ARM64;
        Build(new[] { currentScene }, "../builds/", BuildTarget.StandaloneOSX, "Mac.app");
    }

    [MenuItem("File/Build All Scenes Separately")]
    static void BuildAllScenesSeparately()
    {
        var scenes = EditorBuildSettings.scenes;

        foreach (var scene in scenes)
        {
            string[] sceneToBuild = { scene.path };

            // Build each scene separately
            Build(sceneToBuild, "../builds/" + System.IO.Path.GetFileNameWithoutExtension(scene.path) + "/", BuildTarget.StandaloneWindows, "Windows/NEVE_unity_urp.exe");
            Build(sceneToBuild, "../builds/" + System.IO.Path.GetFileNameWithoutExtension(scene.path) + "/", BuildTarget.StandaloneLinux64, "Linux/Linux.x86_64");
            UserBuildSettings.architecture = MacOSArchitecture.x64ARM64;
            Build(sceneToBuild, "../builds/" + System.IO.Path.GetFileNameWithoutExtension(scene.path) + "/", BuildTarget.StandaloneOSX, "Mac.app");
        }
    }

    [MenuItem("File/Build All Scenes")]
    static void BuildAllScenes()
    {
        var scenes = EditorBuildSettings.scenes;

        // Convert all scenes to a format that BuildPipeline.BuildPlayer can understand
        string[] allScenes = new string[scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            allScenes[i] = scenes[i].path;
        }

        // Call the Build function once for each platform with all scenes
        Build(allScenes, "../builds/All/", BuildTarget.StandaloneWindows, "Windows/NEVE_unity_urp.exe");
        Build(allScenes, "../builds/All/", BuildTarget.StandaloneLinux64, "Linux/Linux.x86_64");
        UserBuildSettings.architecture = MacOSArchitecture.x64ARM64;
        Build(allScenes, "../builds/All/", BuildTarget.StandaloneOSX, "Mac.app");
    }

    static void Build(string[] scenes, string buildDir, BuildTarget target, string targetName)
    {
        // Build all scenes into one executable
        var report = BuildPipeline.BuildPlayer(scenes, buildDir + targetName, target, BuildOptions.None);
        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + report.summary.totalSize + " bytes");
        }
        if (report.summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }

    static void PerformAssetBundleBuild()
    {
        BuildPipeline.BuildAssetBundles("../AssetBundles/", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneLinux64);
        BuildPipeline.BuildAssetBundles("../AssetBundles/", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
        BuildPipeline.BuildAssetBundles("../AssetBundles/", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneOSX);
    }
}