using System.Collections;
using CustomUtilities.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectCore.Architecture
{
    /// <summary>
    /// The gameplay state: loads the game scene and puts a HUD over it.
    ///
    /// A <see cref="UIViewState"/> whose view is the HUD rather than a
    /// full-screen menu, plus the scene the HUD sits on top of. Order matters —
    /// the scene loads first so the HUD is created last and renders above it.
    ///
    /// The scene is loaded <b>additively</b>, never single. `ApplicationBase`
    /// and `ApplicationFlowController` live in the boot scene and are not
    /// marked DontDestroyOnLoad, so a single-mode load would destroy the FSM
    /// owner mid-transition and strand the whole flow.
    ///
    /// Pause/Resume are inherited untouched: an overlay (Settings, Store) hides
    /// the HUD but leaves the game scene loaded and running underneath.
    /// </summary>
    [CreateAssetMenu(fileName = "s_", menuName = "ProjectCore/State Machine/Game State")]
    public class GameState : UIViewState
    {
        [Tooltip("Scene loaded additively while this state is current. Must be in Build Settings or the load silently does nothing.")]
        [SerializeField] private string SceneName = "GameScene";

        [System.NonSerialized] private bool _sceneLoaded;

        public override IEnumerator Execute()
        {
            yield return LoadGameScene();
            // HUD after the scene, so it is instantiated last and draws on top.
            yield return base.Execute();
        }

        public override IEnumerator Exit()
        {
            // HUD first: tearing the scene down under a live HUD can leave the
            // HUD referencing destroyed scene objects for a frame.
            yield return base.Exit();
            yield return UnloadGameScene();
        }

        public override IEnumerator Cleanup()
        {
            yield return base.Cleanup();
            yield return UnloadGameScene();
        }

        private IEnumerator LoadGameScene()
        {
            if (!Application.isPlaying || string.IsNullOrEmpty(SceneName)) yield break;

            if (SceneManager.GetSceneByName(SceneName).isLoaded)
            {
                _sceneLoaded = true;
                yield break;
            }

            if (!Application.CanStreamedLevelBeLoaded(SceneName))
            {
                Debug.LogError($"[GameState] Scene '{SceneName}' is not in Build Settings — gameplay will show the HUD over an empty boot scene. Add it via File > Build Profiles.");
                yield break;
            }

            var load = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            if (load == null) yield break;

            while (!load.isDone) yield return null;

            _sceneLoaded = true;
        }

        private IEnumerator UnloadGameScene()
        {
            if (!_sceneLoaded || !Application.isPlaying) { _sceneLoaded = false; yield break; }

            _sceneLoaded = false;

            var scene = SceneManager.GetSceneByName(SceneName);
            if (!scene.isLoaded) yield break;

            // Never unload the last remaining scene — that would leave no boot
            // scene and kill the application object with it.
            if (SceneManager.sceneCount <= 1) yield break;

            var unload = SceneManager.UnloadSceneAsync(scene);
            if (unload == null) yield break;

            while (!unload.isDone) yield return null;
        }
    }
}
