using UnityEngine;

namespace CustomEditorUtilities
{
    /// <summary>
    /// ScriptableObject holding Android keystore credentials for builds.
    /// Create via Assets → Create → Build Automation → Builder Config, place it
    /// under a Resources folder named <c>BuilderConfig</c>, and fill credentials
    /// in the Inspector. Defaults are intentionally EMPTY — never commit real
    /// keystore passwords to source control.
    /// </summary>
    [CreateAssetMenu(fileName = "BuilderConfig", menuName = "Build Automation/Builder Config")]
    public class BuilderConfig : ScriptableObject
    {
        public string KeystorePassword = string.Empty;
        public string KeyAliasPassword = string.Empty;

        private static BuilderConfig _cached;

        /// <summary>
        /// Loads the default config from <c>Resources/BuilderConfig</c>. Returns a
        /// blank instance (empty credentials) when no asset exists, so build code
        /// never throws on an unconfigured project.
        /// </summary>
        public static BuilderConfig LoadDefault()
        {
            if (_cached != null)
                return _cached;

            _cached = Resources.Load<BuilderConfig>("BuilderConfig");
            if (_cached == null)
            {
                Debug.LogWarning("[BuildAutomation] No Resources/BuilderConfig asset found; using empty keystore credentials. Android release signing will fall back to the values set in Player Settings.");
                _cached = CreateInstance<BuilderConfig>();
            }
            return _cached;
        }
    }
}
