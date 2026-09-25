using System.IO;
using ProjectCore.Architecture;
using ProjectCore.Events;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectBootstrap
{
    /// <summary>
    /// One-shot scaffolder for the placeholder screen UI. Builds a uGUI prefab
    /// per screen, converts the four bare State assets into UIViewStates wired
    /// to those prefabs, and drops an EventSystem into the boot scene.
    ///
    /// Re-runnable: every step overwrites rather than duplicates.
    ///
    /// Deliberately uses legacy UnityEngine.UI.Text, not TextMeshPro — TMP's
    /// essential resources are not imported in this project, so TMP labels would
    /// render as missing-font boxes.
    /// </summary>
    public static class ViewPrefabBuilder
    {
        const string UIDir = "Assets/UI";
        const string StatesDir = "Assets/StateMachine/States";
        const string EventsDir = "Assets/GameEvents";

        // UICloseReasons values, kept as ints because that is what the Button
        // persistent-listener argument and GameEventWithInt payload carry.
        const int Home = 1, Game = 2, Settings = 3, ResumeGame = 4, Store = 7;

        [MenuItem("Tools/Project Bootstrap/Build Screen Views")]
        public static void BuildAll()
        {
            Directory.CreateDirectory(UIDir);

            BuildView("MainMenuView", new Color(0.13f, 0.17f, 0.25f), "MAIN MENU", "e_MainMenuViewClosed",
                ("Play", Game), ("Settings", Settings), ("Store", Store));

            BuildHud("GameHudView", "e_GameplayViewClosed",
                ("Settings", Settings), ("Store", Store), ("Home", Home));

            BuildView("SettingsView", new Color(0.24f, 0.19f, 0.10f), "SETTINGS  (overlay)", "e_SettingsViewClosed",
                ("Back", ResumeGame), ("Home", Home));

            BuildView("StoreView", new Color(0.23f, 0.12f, 0.24f), "STORE  (overlay)", "e_StoreViewClosed",
                ("Back", ResumeGame), ("Home", Home));

            ConvertStates();
            EnsureGameSceneInBuild();
            StripGameSceneCamera();
            EnsureEventSystem();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[ViewPrefabBuilder] Done.");
        }

        static void BuildView(string viewName, Color bg, string title, string closedEventName,
            params (string Label, int Reason)[] buttons)
        {
            var root = new GameObject(viewName, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            // Overlays must paint above the screen they cover.
            canvas.sortingOrder = viewName.Contains("Settings") || viewName.Contains("Store") ? 10 : 0;

            var scaler = root.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 0.5f;

            var panel = NewRect("Panel", root.transform);
            Stretch(panel);
            panel.gameObject.AddComponent<Image>().color = bg;

            var titleRect = NewRect("Title", panel);
            titleRect.anchorMin = new Vector2(0.5f, 1f);
            titleRect.anchorMax = new Vector2(0.5f, 1f);
            titleRect.pivot = new Vector2(0.5f, 1f);
            titleRect.anchoredPosition = new Vector2(0, -160);
            titleRect.sizeDelta = new Vector2(900, 120);
            var titleText = titleRect.gameObject.AddComponent<Text>();
            StyleText(titleText, title, 64, FontStyle.Bold);

            var view = root.AddComponent<UIView>();

            for (int i = 0; i < buttons.Length; i++)
            {
                var (label, reason) = buttons[i];
                var btnRect = NewRect(label + "Button", panel);
                btnRect.anchorMin = btnRect.anchorMax = new Vector2(0.5f, 0.5f);
                btnRect.pivot = new Vector2(0.5f, 0.5f);
                btnRect.sizeDelta = new Vector2(600, 140);
                btnRect.anchoredPosition = new Vector2(0, 200 - i * 180);

                btnRect.gameObject.AddComponent<Image>().color = new Color(0.9f, 0.9f, 0.92f);
                var button = btnRect.gameObject.AddComponent<Button>();

                var labelRect = NewRect("Text", btnRect);
                Stretch(labelRect);
                var labelText = labelRect.gameObject.AddComponent<Text>();
                StyleText(labelText, label, 44, FontStyle.Normal);
                labelText.color = new Color(0.08f, 0.08f, 0.1f);

                // Serialized (persistent) listener so the wiring is visible and
                // editable in the Inspector, not created at runtime.
                UnityAction<int> call = view.Close;
                UnityEventTools.AddIntPersistentListener(button.onClick, call, reason);
            }

            var closedEvent = LoadAsset<GameEventWithInt>($"{EventsDir}/{closedEventName}.asset");
            if (closedEvent != null)
            {
                var so = new SerializedObject(view);
                so.FindProperty("ClosedEvent").objectReferenceValue = closedEvent;
                so.ApplyModifiedPropertiesWithoutUndo();
            }
            else
            {
                Debug.LogError($"[ViewPrefabBuilder] Missing {closedEventName}; {viewName} will not report dismissals.");
            }

            PrefabUtility.SaveAsPrefabAsset(root, $"{UIDir}/{viewName}.prefab");
            Object.DestroyImmediate(root);
            Debug.Log($"[ViewPrefabBuilder] Built {viewName}.prefab");
        }

        /// <summary>
        /// A HUD, not a menu: the root stays transparent so the game scene shows
        /// through, with a thin stat bar pinned to the top and small buttons in
        /// the bottom-right. A full-screen opaque panel here would hide the game.
        /// </summary>
        static void BuildHud(string viewName, string closedEventName, params (string Label, int Reason)[] buttons)
        {
            var root = new GameObject(viewName, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 0;

            var scaler = root.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 0.5f;

            var view = root.AddComponent<UIView>();

            // Top stat bar.
            var bar = NewRect("StatBar", root.transform);
            bar.anchorMin = new Vector2(0f, 1f);
            bar.anchorMax = new Vector2(1f, 1f);
            bar.pivot = new Vector2(0.5f, 1f);
            bar.offsetMin = new Vector2(0, -150);
            bar.offsetMax = new Vector2(0, 0);
            bar.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.45f);

            var score = NewRect("ScoreLabel", bar);
            score.anchorMin = new Vector2(0f, 0f);
            score.anchorMax = new Vector2(0.5f, 1f);
            score.offsetMin = new Vector2(40, 0);
            score.offsetMax = Vector2.zero;
            var scoreText = score.gameObject.AddComponent<Text>();
            StyleText(scoreText, "SCORE  0", 40, FontStyle.Bold);
            scoreText.alignment = TextAnchor.MiddleLeft;

            var coins = NewRect("CoinsLabel", bar);
            coins.anchorMin = new Vector2(0.5f, 0f);
            coins.anchorMax = new Vector2(1f, 1f);
            coins.offsetMin = Vector2.zero;
            coins.offsetMax = new Vector2(-40, 0);
            var coinsText = coins.gameObject.AddComponent<Text>();
            StyleText(coinsText, "COINS  0", 40, FontStyle.Bold);
            coinsText.alignment = TextAnchor.MiddleRight;

            // Compact buttons, bottom-right.
            for (int i = 0; i < buttons.Length; i++)
            {
                var (label, reason) = buttons[i];
                var btnRect = NewRect(label + "Button", root.transform);
                btnRect.anchorMin = btnRect.anchorMax = new Vector2(1f, 0f);
                btnRect.pivot = new Vector2(1f, 0f);
                btnRect.sizeDelta = new Vector2(320, 100);
                btnRect.anchoredPosition = new Vector2(-40, 40 + i * 120);

                btnRect.gameObject.AddComponent<Image>().color = new Color(0.9f, 0.9f, 0.92f, 0.9f);
                var button = btnRect.gameObject.AddComponent<Button>();

                var labelRect = NewRect("Text", btnRect);
                Stretch(labelRect);
                var labelText = labelRect.gameObject.AddComponent<Text>();
                StyleText(labelText, label, 36, FontStyle.Normal);
                labelText.color = new Color(0.08f, 0.08f, 0.1f);

                UnityAction<int> call = view.Close;
                UnityEventTools.AddIntPersistentListener(button.onClick, call, reason);
            }

            AssignClosedEvent(view, closedEventName, viewName);

            PrefabUtility.SaveAsPrefabAsset(root, $"{UIDir}/{viewName}.prefab");
            Object.DestroyImmediate(root);
            Debug.Log($"[ViewPrefabBuilder] Built {viewName}.prefab (HUD)");
        }

        static void AssignClosedEvent(UIView view, string closedEventName, string viewName)
        {
            var closedEvent = LoadAsset<GameEventWithInt>($"{EventsDir}/{closedEventName}.asset");
            if (closedEvent == null)
            {
                Debug.LogError($"[ViewPrefabBuilder] Missing {closedEventName}; {viewName} will not report dismissals.");
                return;
            }

            var so = new SerializedObject(view);
            so.FindProperty("ClosedEvent").objectReferenceValue = closedEvent;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static void ConvertStates()
        {
            var viewState = AssetDatabase.LoadAssetAtPath<MonoScript>("Assets/Runtime/UI/UIViewState.cs");
            var gameState = AssetDatabase.LoadAssetAtPath<MonoScript>("Assets/Runtime/UI/GameState.cs");
            if (viewState == null || gameState == null)
            {
                Debug.LogError("[ViewPrefabBuilder] UIViewState.cs / GameState.cs not found.");
                return;
            }

            foreach (var (stateName, viewName, isGameState) in new[]
            {
                ("MainMenuState", "MainMenuView", false), ("GameplayState", "GameHudView", true),
                ("SettingsState", "SettingsView", false), ("StoreState", "StoreView", false),
            })
            {
                var script = isGameState ? gameState : viewState;
                var path = $"{StatesDir}/{stateName}.asset";
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (asset == null) { Debug.LogError($"[ViewPrefabBuilder] Missing {path}"); continue; }

                // Retarget m_Script in place so the asset's GUID survives — every
                // Transition and the FSM's BootState already point at it.
                var wanted = isGameState ? typeof(GameState) : typeof(UIViewState);
                if (asset.GetType() != wanted)
                {
                    var so = new SerializedObject(asset);
                    so.FindProperty("m_Script").objectReferenceValue = script;
                    so.ApplyModifiedPropertiesWithoutUndo();
                }

                // Swapping m_Script destroys and re-creates the managed instance,
                // so `asset` is a dangling reference from here on — reload it.
                var typed = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{UIDir}/{viewName}.prefab");
                if (typed == null || prefab == null)
                {
                    Debug.LogError($"[ViewPrefabBuilder] Could not wire {stateName} to {viewName}.");
                    continue;
                }

                var reloaded = new SerializedObject(typed);
                var viewProp = reloaded.FindProperty("ViewPrefab");
                if (viewProp == null)
                {
                    Debug.LogError($"[ViewPrefabBuilder] {stateName} has no ViewPrefab field — script swap did not take.");
                    continue;
                }

                viewProp.objectReferenceValue = prefab.GetComponent<UIView>();
                var persist = reloaded.FindProperty("PersistAcrossScenes");
                if (persist != null) persist.boolValue = true;
                if (isGameState)
                {
                    var sceneProp = reloaded.FindProperty("SceneName");
                    if (sceneProp != null) sceneProp.stringValue = "GameScene";
                }
                reloaded.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(typed);
                Debug.Log($"[ViewPrefabBuilder] {stateName} -> {wanted.Name} -> {viewName}");
            }
        }

        /// <summary>
        /// GameState loads by name, and Application.CanStreamedLevelBeLoaded only
        /// returns true for scenes listed in Build Settings.
        /// </summary>
        static void EnsureGameSceneInBuild()
        {
            const string gameScene = "Assets/Scenes/GameScene.unity";

            var scenes = new System.Collections.Generic.List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            if (scenes.Exists(s => s.path == gameScene))
            {
                Debug.Log("[ViewPrefabBuilder] GameScene already in Build Settings.");
                return;
            }

            // Appended, not inserted: BootstrapScene must stay at index 0 so the
            // alwaysstartfromscenezero utility still boots the right scene.
            scenes.Add(new EditorBuildSettingsScene(gameScene, true));
            EditorBuildSettings.scenes = scenes.ToArray();
            Debug.Log("[ViewPrefabBuilder] Added GameScene to Build Settings (index 1).");
        }

        /// <summary>
        /// GameScene is loaded additively on top of the boot scene, which already
        /// has a camera and an AudioListener. A second pair would render twice and
        /// log "there are 2 audio listeners in the scene".
        /// </summary>
        static void StripGameSceneCamera()
        {
            const string gameScenePath = "Assets/Scenes/GameScene.unity";
            var scene = EditorSceneManager.OpenScene(gameScenePath, OpenSceneMode.Single);

            var removed = false;
            foreach (var root in scene.GetRootGameObjects())
            {
                if (root.GetComponent<Camera>() == null && root.GetComponent<AudioListener>() == null) continue;
                Debug.Log($"[ViewPrefabBuilder] Removing '{root.name}' from GameScene — the boot scene's camera is the persistent one.");
                Object.DestroyImmediate(root);
                removed = true;
            }

            if (removed)
            {
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
            }
            else
            {
                Debug.Log("[ViewPrefabBuilder] GameScene already has no camera.");
            }
        }

        static void EnsureEventSystem()
        {
            const string scenePath = "Assets/Scenes/BootstrapScene.unity";
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            foreach (var root in scene.GetRootGameObjects())
            {
                if (root.GetComponent<EventSystem>() != null)
                {
                    Debug.Log("[ViewPrefabBuilder] EventSystem already present.");
                    return;
                }
            }

            // InputSystemUIInputModule, not StandaloneInputModule: this project is
            // set to the new Input System only (activeInputHandler: 1), where the
            // legacy module does nothing and every button would be dead.
            var es = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            SceneManager.MoveGameObjectToScene(es, scene);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log("[ViewPrefabBuilder] Added EventSystem (InputSystemUIInputModule) to BootstrapScene.");
        }

        static RectTransform NewRect(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return (RectTransform)go.transform;
        }

        static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        static void StyleText(Text text, string content, int size, FontStyle style)
        {
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = size;
            text.fontStyle = style;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            text.raycastTarget = false;
        }

        static T LoadAsset<T>(string path) where T : Object => AssetDatabase.LoadAssetAtPath<T>(path);
    }
}
