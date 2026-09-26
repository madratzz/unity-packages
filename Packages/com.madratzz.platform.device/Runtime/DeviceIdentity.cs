using System;
using UnityEngine;

namespace Madratzz.Platform.Device
{
    /// <summary>
    /// Provides a stable, privacy-compliant install identifier per platform.
    ///
    /// The identifier is generated once (a random GUID), persisted, and returned
    /// unchanged for the lifetime of the install. It is NOT a hardware identifier:
    /// <c>SystemInfo.deviceUniqueIdentifier</c> is intentionally avoided because
    /// Apple deprecates hardware IDs and the value changes across reinstalls.
    ///
    /// Persistence per platform:
    ///   iOS     — Keychain (survives app reinstall; per-app keychain access group)
    ///   Android — PlayerPrefs (app-private storage; cleared on reinstall or app-data wipe)
    ///   Other   — PlayerPrefs
    ///
    /// Privacy: a self-generated UUID carries no personal data. If you declare an
    /// IDFV-free identity in the App Store privacy manifest, this qualifies as a
    /// "user-generated" identifier scoped to your app.
    /// </summary>
    public static class DeviceIdentity
    {
        private const string PlayerPrefsKey = "com.madratzz.platform.install_id";

        private static string _cached;

        /// <summary>
        /// Returns the stable install ID for this app install, creating and
        /// persisting it on first call.
        /// </summary>
        public static string GetInstallId()
        {
            if (!string.IsNullOrEmpty(_cached))
                return _cached;

            _cached = Load() ?? CreateAndPersist();
            return _cached;
        }

        /// <summary>
        /// True if an install ID has already been persisted for this install.
        /// </summary>
        public static bool HasPersistedId => Load() != null;

        private static string CreateAndPersist()
        {
            var id = Guid.NewGuid().ToString("N");
            Save(id);
            return id;
        }

        private static string Load()
        {
#if UNITY_IOS && !UNITY_EDITOR
            return IosKeychain.Get(PlayerPrefsKey);
#else
            var id = PlayerPrefs.GetString(PlayerPrefsKey, string.Empty);
            return string.IsNullOrEmpty(id) ? null : id;
#endif
        }

        private static void Save(string id)
        {
#if UNITY_IOS && !UNITY_EDITOR
            IosKeychain.Set(PlayerPrefsKey, id);
#else
            PlayerPrefs.SetString(PlayerPrefsKey, id);
            PlayerPrefs.Save();
#endif
        }
    }
}
