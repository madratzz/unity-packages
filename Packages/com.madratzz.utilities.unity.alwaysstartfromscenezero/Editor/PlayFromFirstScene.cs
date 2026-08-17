#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;



[ExecuteInEditMode]
  public static class PlayFromFirstScene
 {      
     const string playFromFirstMenuStr = "EditorUtilities/Always Start From Scene 0 &p";
     
     private const string SceneIndexEmptyWarning = "The scene build list is empty. Can't play from first scene.";
 
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
 
     // This method is called before any Awake. It's the perfect callback for this feature
     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 
     static void LoadFirstSceneAtGameBegins()
     {
         if(!playFromFirstScene)
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