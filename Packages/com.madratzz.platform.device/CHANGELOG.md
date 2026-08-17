# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [0.0.1] - 2026-08-17

### Added
- Initial release
- `DeviceIdentity.GetInstallId()` — stable, privacy-compliant install identifier: a self-generated GUID persisted per platform (iOS Keychain, PlayerPrefs elsewhere); replaces hardware-ID-based identification
- `DeviceIdentity.HasPersistedId` — reports whether an ID was already persisted for this install
- iOS Keychain native plugin (`Runtime/Plugins/iOS/KeychainStorage.mm`) — `SecItem`-backed string storage using `kSecAttrAccessibleAfterFirstUnlockThisDeviceOnly`
