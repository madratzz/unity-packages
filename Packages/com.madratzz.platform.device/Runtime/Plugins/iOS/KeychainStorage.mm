#import <Foundation/Foundation.h>
#import <Security/Security.h>

// Minimal Keychain read/write for string values, used by Madratzz.Platform.Device.
// kSecAttrAccessibleAfterFirstUnlock keeps the value readable when the device is
// unlocked; kSecAttrAccessible defaults keep it out of iCloud backups (per-install).

static NSString * const kMadratzzService = @"com.madratzz.platform.device";

FOUNDATION_EXPORT NSString * MadratzzKeychain_GetString(const char * key)
{
    if (key == NULL) return nil;
    NSString * nsKey = @(key);

    NSDictionary * query = @{
        (__bridge id)kSecClass:            (__bridge id)kSecClassGenericPassword,
        (__bridge id)kSecAttrService:      kMadratzzService,
        (__bridge id)kSecAttrAccount:      nsKey,
        (__bridge id)kSecReturnData:       @YES,
        (__bridge id)kSecMatchLimit:       (__bridge id)kSecMatchLimitOne
    };

    CFTypeRef result = NULL;
    OSStatus status = SecItemCopyMatching((__bridge CFDictionaryRef)query, &result);
    if (status != errSecSuccess || result == NULL) return nil;

    NSData * data = (__bridge_transfer NSData *)result;
    return [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
}

FOUNDATION_EXPORT void MadratzzKeychain_SetString(const char * key, const char * value)
{
    if (key == NULL || value == NULL) return;
    NSString * nsKey   = @(key);
    NSData   * nsValue = [@(value) dataUsingEncoding:NSUTF8StringEncoding];

    NSDictionary * query = @{
        (__bridge id)kSecClass:       (__bridge id)kSecClassGenericPassword,
        (__bridge id)kSecAttrService: kMadratzzService,
        (__bridge id)kSecAttrAccount: nsKey
    };

    NSDictionary * attributes = @{
        (__bridge id)kSecValueData:     nsValue,
        (__bridge id)kSecAttrAccessible: (__bridge id)kSecAttrAccessibleAfterFirstUnlockThisDeviceOnly
    };

    OSStatus status = SecItemUpdate((__bridge CFDictionaryRef)query, (__bridge CFDictionaryRef)attributes);
    if (status == errSecItemNotFound)
    {
        NSMutableDictionary * add = [query mutableCopy];
        [add addEntriesFromDictionary:attributes];
        SecItemAdd((__bridge CFDictionaryRef)add, NULL);
    }
}
