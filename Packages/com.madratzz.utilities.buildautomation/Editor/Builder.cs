using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using System;
using System.Collections.Generic;

namespace CustomEditorUtilities
{
    public class Builder : MonoBehaviour
    {
        [MenuItem("Build/Build iOS")]
        public static void BuildiOS()
        {
            ConfigurePlayerSettingsForBuild(BuildTarget.iOS);
            Build(BuildTarget.iOS, BuildOptions.None, "iOSBuild");
        }

        [MenuItem("Build/Build Android APK")]
        public static void BuildAndroidAPK()
        {
            ConfigurePlayerSettingsForBuild(BuildTarget.Android);
            BuildAPK();
        }

        [MenuItem("Build/Build Android AAB")]
        public static void BuildAndroidAAB()
        {
            ConfigurePlayerSettingsForBuild(BuildTarget.Android);
            BuildAAB();
        }

        [MenuItem("Build/Build Android Development APK")]
        public static void BuildAndroidDevelopmentAPK()
        {
            ConfigurePlayerSettingsForBuild(BuildTarget.Android);
            BuildDevelopmentAPK();
        }

        private static void ConfigurePlayerSettingsForBuild(BuildTarget target)
        {
            var buildVersion = GetArgument("-buildversion");
            var config = BuilderConfig.Current;

            if (string.IsNullOrEmpty(buildVersion))
            {
                // CLI arg absent — the config's override wins over PlayerSettings;
                // fall back to PlayerSettings when the override is empty too.
                buildVersion = !string.IsNullOrEmpty(config.BuildVersionOverride)
                    ? config.BuildVersionOverride
                    : PlayerSettings.bundleVersion;
            }

            PlayerSettings.bundleVersion = buildVersion;

            int versionCode = GenerateVersionCode(buildVersion);

            if (target == BuildTarget.Android)
            {
                Debug.Log($"Setting Version Code to {versionCode}");
                PlayerSettings.Android.bundleVersionCode = versionCode;

                // Keystore credentials apply to Android only — never to iOS.
                if (!string.IsNullOrEmpty(config.KeystorePassword))
                {
                    PlayerSettings.Android.keystorePass = config.KeystorePassword;
                    PlayerSettings.Android.keyaliasPass = config.KeyAliasPassword;
                }
            }
            else if (target == BuildTarget.iOS)
            {
                PlayerSettings.iOS.buildNumber = versionCode.ToString();
            }
        }

        private static int GenerateVersionCode(string version)
        {
            if (TryGenerateVersionCode(version, out int versionCode))
                return versionCode;

            Debug.LogError("Failed to parse version code from version string: " + version);
            return 1; // Default to 1 if parsing fails
        }

        /// <summary>
        /// Strips dots from a semantic version string and parses the continuous
        /// number (e.g. "1.2.3" → 123). Returns false when the result is not a
        /// valid integer. Extracted as an internal seam for EditMode tests.
        /// </summary>
        internal static bool TryGenerateVersionCode(string version, out int versionCode)
        {
            versionCode = 0;
            if (string.IsNullOrEmpty(version))
                return false;

            string numericVersion = version.Replace(".", string.Empty);
            return int.TryParse(numericVersion, out versionCode);
        }

        private static void BuildAPK()
        {
            EditorUserBuildSettings.buildAppBundle = false;
            Build(BuildTarget.Android, BuildOptions.None,
                $"AndroidBuilds/{PlayerSettings.productName}_{PlayerSettings.bundleVersion}-{PlayerSettings.Android.bundleVersionCode}.apk");
        }

        private static void BuildAAB()
        {
            EditorUserBuildSettings.buildAppBundle = true;
            Build(BuildTarget.Android, BuildOptions.None,
                $"AndroidBuilds/{PlayerSettings.productName}_{PlayerSettings.bundleVersion}-{PlayerSettings.Android.bundleVersionCode}.aab");
        }

        private static void BuildDevelopmentAPK()
        {
            EditorUserBuildSettings.buildAppBundle = false;
            Build(BuildTarget.Android, BuildOptions.Development,
                $"AndroidBuilds/{PlayerSettings.productName}_{PlayerSettings.bundleVersion}-{PlayerSettings.Android.bundleVersionCode}_Dev.apk");
        }

        private static void Build(BuildTarget target, BuildOptions options, string pathName)
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = GetActiveScenes(),
                locationPathName = $"{BuilderConfig.Current.OutputDirectory}/{pathName}",
                target = target,
                options = options
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            HandleBuildReport(report);
        }

        private static void HandleBuildReport(BuildReport report)
        {
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
                Debug.Log($"Build succeeded: {summary.totalSize} bytes");
            else if (summary.result == BuildResult.Failed)
                Debug.Log("Build failed");
        }

        private static string GetArgument(string argName)
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == argName && args[i + 1] != "empty" && args[i + 1] != "null")
                    return args[i + 1];
            }

            return null;
        }

        private static string[] GetActiveScenes()
        {
            return GetEnabledScenePaths(EditorBuildSettings.scenes);
        }

        /// <summary>
        /// Returns the paths of build-settings scenes that are enabled, in order.
        /// Extracted as an internal seam for EditMode tests.
        /// </summary>
        internal static string[] GetEnabledScenePaths(IEnumerable<EditorBuildSettingsScene> scenes)
        {
            List<string> result = new List<string>();
            foreach (var scene in scenes)
            {
                if (scene.enabled)
                    result.Add(scene.path);
            }
            return result.ToArray();
        }
    }
}
