#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;



[ExecuteInEditMode]
  public static class PlayFromFirstScene
 {      
     const string playFromFirstMenuStr = "EditorUtilities/Always Start From Scene 0 &p";
     
     private const string SceneIndexEmptyWarning = "The scene build list is empty. Can't play from first scene.";

     // The Test Framework drives a Play Mode run by creating a bootstrap scene, saved as
     // Assets/InitTestScene<guid>.unity and holding a "Code-based tests runner" GameObject,
     // making it the active scene, and only then entering Play Mode. Loading scene 0 over
     // the top destroys that controller while the runner is still waiting for it, so the
     // run HANGS rather than failing — see .agents/LEARNINGS.md.
     //
     // Both names below are Test Framework internals, so neither is checked alone: if one
     // is renamed upstream the other still holds the guard. Detection is deliberately
     // reflection-free and reference-free — adding UnityEditor.TestRunner to this asmdef
     // would make com.unity.test-framework a hard dependency for every consumer of a
     // package that has nothing to do with testing.
     //
     // The controller check is scoped to objects that belong to a real scene on purpose.
     // After the first Play Mode run of an Editor session the Test Framework leaves a
     // detached HideFlags.DontSave GameObject of the same name alive (scene invalid, not
     // loaded), and an unscoped GameObject.Find would match it for the rest of the
     // session — which would silently disable this package's whole feature.
     private const string TestRunnerScenePrefix = "InitTestScene";
     private const string TestRunnerControllerObjectName = "Code-based tests runner";

     // Set while still in Edit Mode, where the active scene is unambiguously readable, and
     // read back after the domain reload. SessionState survives the reload into Play Mode
     // and is cleared when the Editor exits, which is exactly the lifetime we want.
     private const string TestRunSessionKey = "PlayFromFirstScene.IsPlayModeTestRun";
 
     static bool playFromFirstScene
     {
         get{return EditorPrefs.HasKey(playFromFirstMenuStr) && EditorPrefs.GetBool(playFromFirstMenuStr);}
         set{EditorPrefs.SetBool(playFromFirstMenuStr, value);}
     }
 
     [MenuItem(playFromFirstMenuStr, false, 150)]
     static void PlayFromFirstSceneCheckMenu() 
     {
         playFromFirstScene = !playFromFirstScene;
         Menu.SetChecked(playFromFirstMenuStr, playFromFirstScene);
 
         ShowNotifyOrLog(playFromFirstScene ? "Play from scene 0" : "Play from current scene");
     }
 
     // The menu won't be gray out, we use this validate method for update check state
     [MenuItem(playFromFirstMenuStr, true)]
     static bool PlayFromFirstSceneCheckMenuValidate()
     {
         Menu.SetChecked(playFromFirstMenuStr, playFromFirstScene);
         return true;
     }
 
     // Entering Play Mode always passes through ExitingEditMode before the domain reload
     // that runs LoadFirstSceneAtGameBegins, so the flag is always current by the time it
     // is read.
     [InitializeOnLoadMethod]
     static void WatchForPlayModeTestRuns()
     {
         EditorApplication.playModeStateChanged -= RecordPlayModeTestRun;
         EditorApplication.playModeStateChanged += RecordPlayModeTestRun;
     }

     static void RecordPlayModeTestRun(PlayModeStateChange state)
     {
         if(state == PlayModeStateChange.ExitingEditMode)
             SessionState.SetBool(TestRunSessionKey, IsTestRunnerBootstrapScene());
     }

     static bool IsTestRunnerBootstrapScene()
     {
         Scene active = SceneManager.GetActiveScene();

         if(active.IsValid())
         {
             if(active.name.StartsWith(TestRunnerScenePrefix, StringComparison.Ordinal))
                 return true;

             if(active.path.StartsWith("Assets/" + TestRunnerScenePrefix, StringComparison.Ordinal))
                 return true;
         }

         GameObject controller = GameObject.Find(TestRunnerControllerObjectName);

         return controller != null && controller.scene.IsValid();
     }

     // This method is called before any Awake. It's the perfect callback for this feature
     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 
     static void LoadFirstSceneAtGameBegins()
     {
         if(!playFromFirstScene)
             return;

         // A Play Mode test run owns the scene it set up — never replace it.
         if(SessionState.GetBool(TestRunSessionKey, false) || IsTestRunnerBootstrapScene())
             return;

         if(EditorBuildSettings.scenes.Length  == 0)
         {
             Debug.LogWarning(SceneIndexEmptyWarning);
             return;
         }

         // Single mode replaces the active scene — no need to deactivate the
         // current scene's objects first.
         EditorSceneManager.LoadScene(0, LoadSceneMode.Single);
     }
 
     static void ShowNotifyOrLog(string msg)
     {
        if (Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
            EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
        else 
            Debug.Log(msg); // When there's no scene view opened, we just print a log
     }
 }
 
 #endif