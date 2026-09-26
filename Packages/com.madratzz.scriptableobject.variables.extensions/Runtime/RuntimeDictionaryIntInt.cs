#if ODIN_INSPECTOR
using UnityEngine;

namespace ProjectCore.Variables
{
    [CreateAssetMenu(fileName = "v_", menuName = "ProjectCore/Variables/RuntimeDictionaryIntInt")]
    public class RuntimeDictionaryIntInt : RuntimeDictionary<int, int>
    {
    }
}
#endif
