using System;
using System.IO;
using UnityEngine;

namespace CustomEditorUtilities
{
    /// <summary>
    /// ScriptableObject holding build configuration: Android keystore credentials,
    /// optional bundle-version override, and output directory.
    ///
    /// Resolution order for every build:
    ///   1. <c>-buildversion</c> command-line argument (CI override — wins over everything)
    ///   2. This asset, populated from the project-root <c>buildsettings.json</c> when
    ///      <see cref="ReadFromResourcesFile"/> is enabled (falls back to inspector
    ///      values with a warning when the file is missing or invalid)
    ///   3. Inspector values on this asset
    ///
    /// Create via Assets → Create → Build Automation → Builder Config and place it
    /// under a Resources folder named <c>BuilderConfig</c>. Never commit real keystore
    /// passwords to source control — neither in this asset nor in the JSON file.
    /// </summary>
    [CreateAssetMenu(fileName = "BuilderConfig", menuName = "Build Automation/Builder Config")]
    public class BuilderConfig : ScriptableObject
    {
        [Tooltip("Android keystore password. Empty = keep the value set in Player Settings.")]
        public string KeystorePassword = string.Empty;

        [Tooltip("Android key alias password. Empty = keep the value set in Player Settings.")]
        public string KeyAliasPassword = string.Empty;

        [Tooltip("Optional bundle-version override (e.g. 1.2.3). Empty = keep Player Settings' bundle version. The -buildversion CLI argument always wins over this.")]
        public string BuildVersionOverride = string.Empty;

        [Tooltip("Root output directory for builds, relative to the project folder.")]
        public string OutputDirectory = "Builds";

        [Tooltip("When enabled, fields are populated from the project-root buildsettings.json file before each build. Falls back to the values above with a warning if the file is missing or invalid.")]
        public bool ReadFromResourcesFile;

        /// <summary>Project-root JSON file name, relative to the project folder.</summary>
        public const string JsonFileName = "buildsettings.json";

        private static BuilderConfig _cached;

        /// <summary>
        /// The active configuration: the <c>Resources/BuilderConfig</c> asset when one
        /// exists, otherwise a blank instance (empty credentials, default output dir).
        /// When <see cref="ReadFromResourcesFile"/> is enabled, the instance's fields
        /// are refreshed from the project-root JSON file. Never returns null.
        /// </summary>
        public static BuilderConfig Current
        {
            get
            {
                if (_cached == null)
                {
                    _cached = Resources.Load<BuilderConfig>("BuilderConfig");
                    if (_cached == null)
                        _cached = CreateInstance<BuilderConfig>();
                }

                if (_cached.ReadFromResourcesFile)
                    _cached.PopulateFromJsonFile();

                return _cached;
            }
        }

        /// <summary>
        /// Clears the cached instance (for tests and domain-reload-free iteration).
        /// </summary>
        internal static void ResetCache()
        {
            _cached = null;
        }

        /// <summary>
        /// Reads the project-root JSON file and applies its values to this instance's
        /// fields. Missing keys leave the corresponding field untouched, so partial
        /// files merge over inspector values. Returns true when the file was read and
        /// parsed successfully; false (with a warning) on missing/invalid input —
        /// in which case the inspector values remain in effect.
        /// </summary>
        internal bool PopulateFromJsonFile()
        {
            if (!TryReadJsonFile(JsonFileName, out BuildSettingsJson data))
            {
                Debug.LogWarning($"[BuildAutomation] ReadFromResourcesFile is enabled but {JsonFileName} is missing or invalid at the project root — using inspector values.");
                return false;
            }

            ApplyJson(data);
            return true;
        }

        /// <summary>
        /// Parses JSON text into the settings DTO. Extracted for EditMode tests.
        /// </summary>
        internal static bool TryParseJson(string json, out BuildSettingsJson data)
        {
            data = null;
            if (string.IsNullOrEmpty(json))
                return false;

            try
            {
                data = JsonUtility.FromJson<BuildSettingsJson>(json);
                return data != null;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        /// <summary>
        /// Reads and parses the JSON file from disk. Extracted for EditMode tests.
        /// </summary>
        internal static bool TryReadJsonFile(string fileName, out BuildSettingsJson data)
        {
            data = null;
            // Application.dataPath is <project>/Assets — the project root is its parent.
            string path = Path.Combine(Directory.GetParent(Application.dataPath).FullName, fileName);

            if (!File.Exists(path))
                return false;

            string json;
            try
            {
                json = File.ReadAllText(path);
            }
            catch (IOException)
            {
                return false;
            }

            return TryParseJson(json, out data);
        }

        /// <summary>
        /// Applies parsed JSON values to this instance. Only fields the file
        /// actually specifies are overwritten — a partial JSON merges over
        /// inspector values instead of blanking them. Extracted for EditMode tests.
        /// </summary>
        internal void ApplyJson(BuildSettingsJson data)
        {
            // Only overwrite fields the file actually specifies — a partial JSON
            // merges over inspector values instead of blanking them.
            if (!string.IsNullOrEmpty(data.keystorePassword))
                KeystorePassword = data.keystorePassword;
            if (!string.IsNullOrEmpty(data.keyAliasPassword))
                KeyAliasPassword = data.keyAliasPassword;
            if (!string.IsNullOrEmpty(data.buildVersion))
                BuildVersionOverride = data.buildVersion;
            if (!string.IsNullOrEmpty(data.outputDirectory))
                OutputDirectory = data.outputDirectory;
        }

        /// <summary>
        /// JSON DTO for the project-root settings file. Field names match the JSON
        /// keys exactly (JsonUtility maps by name).
        /// </summary>
        [Serializable]
        internal class BuildSettingsJson
        {
            public string keystorePassword;
            public string keyAliasPassword;
            public string buildVersion;
            public string outputDirectory;
        }
    }
}
