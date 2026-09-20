# Platform - Device

← [[Home]] · [[Architecture Overview]]

**Package:** `com.madratzz.platform.device` · [Full README & API](../../Packages/com.madratzz.platform.device/README.md)

Privacy-compliant, per-platform install identity: a self-generated GUID persisted in iOS Keychain (survives reinstall) or PlayerPrefs elsewhere. Replaces hardware `deviceUniqueIdentifier` usage, which is deprecated/restricted on both major app stores.

## Depends on

None — zero-dependency leaf package.

## Used by

Nothing in this repo yet. (Replaces `Utilities.GetDeviceId`, which was removed from [[Utilities - Core]] for being broken on iOS.)

## Key types

| Type | Description |
|---|---|
| `DeviceIdentity.GetInstallId()` | Stable per-install identifier. |
| `DeviceIdentity.HasPersistedId` | `false` on first-ever run. |

## Per-platform persistence

| Platform | Store | Survives reinstall |
|---|---|---|
| iOS | Keychain (`kSecAttrAccessibleAfterFirstUnlockThisDeviceOnly`) | Yes |
| Android | PlayerPrefs | No |
| Editor/other | PlayerPrefs | No |

Namespace: `Madratzz.Platform.Device`.

---
← [[Home]]
