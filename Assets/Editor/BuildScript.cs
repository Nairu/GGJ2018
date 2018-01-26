using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class BuildScript {
    static string[] SCENES = FindEnabledEditorScenes();

    static string APP_NAME = "GGJ2018";
    static string TARGET_DIR = "C:/Builds";

    [MenuItem("Custom/CI/Build Windows")]
    static void PerformWindowsBuild()
    {
        GenericBuild(SCENES, TARGET_DIR + "/windows/" + APP_NAME + ".exe" , BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    [MenuItem("Custom/CI/Build WebGL")]
    static void PerformOGLBuild()
    {
        string target_dir = APP_NAME + "/web";
        GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.WebGL, BuildOptions.None);
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, build_target);
        var res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
        if (res.summary.result == UnityEditor.Build.Reporting.BuildResult.Failed)
        {
            string error = "Failed to build. Run Locally to confirm the build issue.";
            Debug.Log(error);
            throw new System.Exception(error + res.summary.ToString());
        }

        //if (res.Length > 0)
        //{
        //    throw new System.Exception("BuildPlayer failure: " + res);
        //}
    }
}
