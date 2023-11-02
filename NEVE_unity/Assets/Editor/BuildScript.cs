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
        // set use Intel 64-bit or ARM64 architecture (universal). This is undocumented but see https://forum.unity.com/threads/cannot-build-unity-2020-projects-using-command-line-on-macos-with-xcode-10-or-11.1084085/
        UserBuildSettings.architecture = MacOSArchitecture.x64ARM64;
        Build(currentSceneList.ToArray(), "../builds/", BuildTarget.StandaloneOSX, "Mac.app");
    }

    [MenuItem("File/Build All Scenes")]
    static void BuildAllScenes()
    {
        var scenes = EditorBuildSettings.scenes;
        var currentScene = EditorSceneManager.GetActiveScene();

        // get the scene
        List<EditorBuildSettingsScene> currentSceneList = new List<EditorBuildSettingsScene>();
        foreach (var scene in scenes)
        {
            EditorBuildSettingsScene[] sceneToBuild = { scene };

            Build(sceneToBuild, "../builds/", BuildTarget.StandaloneWindows, "Windows/NEVE_unity_urp.exe");
            Build(sceneToBuild, "../builds/", BuildTarget.StandaloneLinux64, "Linux/Linux.x86_64");
            // set use Intel 64-bit or ARM64 architecture (universal). This is undocumented but see https://forum.unity.com/threads/cannot-build-unity-2020-projects-using-command-line-on-macos-with-xcode-10-or-11.1084085/
            UserBuildSettings.architecture = MacOSArchitecture.x64ARM64;
            Build(sceneToBuild, "../builds/", BuildTarget.StandaloneOSX, "Mac.app");
        }
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