using UnityEngine;

namespace Madratzz.Tests.UtilitiesAttributes
{
    // Test fixture for SearchableAssetFinderTests, kept in its own assembly on purpose.
    //
    // Unity's `t:<TypeName>` asset search does not index types declared in test assemblies,
    // so a fixture type declared in com.madratzz.utilities.attributes.tests would make every
    // lookup return nothing regardless of which assets exist. This assembly is a plain Editor
    // assembly — no TestRunner references, no UNITY_INCLUDE_TESTS constraint — so its types
    // are indexed normally, while still living under Tests/ and never shipping in a build.
    public class SearchableAssetFinderTestAsset : ScriptableObject
    {
    }
}