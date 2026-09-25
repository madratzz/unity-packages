using System.Collections;
using CustomUtilities.Attributes;
using ProjectCore.StateMachine;
using UnityEngine;

namespace ProjectCore.Architecture
{
    /// <summary>
    /// A <see cref="State"/> that owns one screen. Instantiates its
    /// <see cref="ViewPrefab"/> when the FSM enters the state and destroys it on
    /// the way out, so only the active screens exist at runtime.
    ///
    /// Lifecycle mapping:
    ///   Execute -> instantiate + show
    ///   Pause   -> hide            (an overlay state took over; see PausesPreviousState)
    ///   Resume  -> show            (the overlay closed and we are current again)
    ///   Exit    -> destroy
    ///
    /// Pause/Resume rather than destroy/recreate is what makes an overlay cheap:
    /// a Settings or Store state with PausesPreviousState set leaves the screen
    /// underneath alive and merely hidden, so resuming restores it as it was.
    /// </summary>
    [CreateAssetMenu(fileName = "s_", menuName = "ProjectCore/State Machine/UI View State")]
    public class UIViewState : State
    {
        [Tooltip("Screen to instantiate while this state is current. Destroyed on Exit.")]
        [RequireReference]
        [SerializeField] private UIView ViewPrefab;

        [Tooltip("Keep the instantiated view across scene loads. Leave on unless the view belongs to a specific scene.")]
        [SerializeField] private bool PersistAcrossScenes = true;

        // Runtime-only: a ScriptableObject outlives play mode in the Editor, so
        // this must never serialize or it would hold a destroyed instance.
        [System.NonSerialized] private UIView _instance;

        public override IEnumerator Execute()
        {
            if (ViewPrefab == null)
            {
                Debug.LogError($"[UIViewState] '{name}' has no ViewPrefab assigned — the FSM will sit in this state with nothing on screen.", this);
                yield break;
            }

            // Defensive: a hard domain reload or an aborted Exit can leave a stale
            // instance behind, which would otherwise stack views on re-entry.
            DestroyInstance();

            _instance = Instantiate(ViewPrefab);
            _instance.name = ViewPrefab.name;

            if (PersistAcrossScenes && Application.isPlaying)
            {
                DontDestroyOnLoad(_instance.gameObject);
            }

            _instance.Show();
            yield break;
        }

        public override IEnumerator Pause()
        {
            if (_instance != null) _instance.Hide();
            yield break;
        }

        public override IEnumerator Resume()
        {
            if (_instance != null) _instance.Show();
            yield break;
        }

        public override IEnumerator Exit()
        {
            DestroyInstance();
            yield break;
        }

        public override IEnumerator Cleanup()
        {
            DestroyInstance();
            yield break;
        }

        private void DestroyInstance()
        {
            if (_instance == null) return;

            if (Application.isPlaying) Destroy(_instance.gameObject);
            else DestroyImmediate(_instance.gameObject);

            _instance = null;
        }
    }
}
