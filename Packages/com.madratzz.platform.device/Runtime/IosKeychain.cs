using System.Runtime.InteropServices;

namespace Madratzz.Platform.Device
{
    /// <summary>
    /// iOS Keychain-backed storage for the install ID. Only compiled on device
    /// iOS builds (<c>UNITY_IOS &amp;&amp; !UNITY_EDITOR</c>); the editor path in
    /// <see cref="DeviceIdentity"/> uses PlayerPrefs so iteration never touches
    /// the simulator keychain.
    ///
    /// Requires the native plugin at
    /// <c>Runtime/Plugins/iOS/KeychainStorage.mm</c> to be present in the build —
    /// it is included in this package and imported automatically.
    /// </summary>
    internal static class IosKeychain
    {
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string MadratzzKeychain_GetString(string key);

        [DllImport("__Internal")]
        private static extern void MadratzzKeychain_SetString(string key, string value);
#endif

        public static string Get(string key)
        {
#if UNITY_IOS && !UNITY_EDITOR
            return MadratzzKeychain_GetString(key);
#else
            return null;
#endif
        }

        public static void Set(string key, string value)
        {
#if UNITY_IOS && !UNITY_EDITOR
            MadratzzKeychain_SetString(key, value);
#endif
        }
    }
}
