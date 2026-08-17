# Platform: Device

Privacy-compliant, per-platform install identity.

## Why not `SystemInfo.deviceUniqueIdentifier`

- Apple deprecates hardware identifiers; the value changes across reinstalls on iOS.
- Google's ad-ID and Android-ID policies restrict hardware-ID use.
- A self-generated UUID is the correct answer on both stores — it carries no personal
  data and qualifies as an app-scoped, user-generated identifier in App Store privacy
  manifests.

## API

```csharp
using Madratzz.Platform.Device;

string installId = DeviceIdentity.GetInstallId();   // stable per install
bool   existed   = DeviceIdentity.HasPersistedId;   // false on first-ever run
```

## Per-platform persistence

| Platform | Store | Survives reinstall | Notes |
|----------|-------|--------------------|-------|
| iOS      | Keychain (`kSecAttrAccessibleAfterFirstUnlockThisDeviceOnly`) | Yes | Native plugin `Runtime/Plugins/iOS/KeychainStorage.mm`; per-device only (not iCloud-synced) |
| Android  | PlayerPrefs (app-private storage) | No (cleared on reinstall / data wipe) | No extra permissions required |
| Editor / other | PlayerPrefs | No | Editor never touches the simulator keychain |

## iOS native plugin

`KeychainStorage.mm` exposes two C functions (`MadratzzKeychain_GetString` /
`MadratzzKeychain_SetString`) called via `[DllImport("__Internal")]`. It uses the
Security framework's `SecItem*` API with `kSecClassGenericPassword` scoped to service
`com.madratzz.platform.device`. No capabilities or entitlements are required for
per-app keychain access.

Namespace: `Madratzz.Platform.Device`. Zero package dependencies.
