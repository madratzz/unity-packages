# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
The initial release collects all pre-release changes into a single entry; versions will increment once the package is first published to the Verdaccio registry.

## [0.0.1] - Unreleased

### Added
- Initial release
- `DeviceIdentity.GetInstallId()` — stable, privacy-compliant install identifier: a self-generated GUID persisted per platform (iOS Keychain, PlayerPrefs elsewhere); replaces hardware-ID-based identification
- `DeviceIdentity.HasPersistedId` — reports whether an ID was already persisted for this install
- iOS Keychain native plugin (`Runtime/Plugins/iOS/KeychainStorage.mm`) — `SecItem`-backed string storage using `kSecAttrAccessibleAfterFirstUnlockThisDeviceOnly`
