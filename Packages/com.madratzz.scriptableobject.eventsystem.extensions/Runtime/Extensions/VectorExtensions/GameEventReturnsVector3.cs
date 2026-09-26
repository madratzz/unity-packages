using UnityEngine;

namespace ProjectCore.Events
{
    [CreateAssetMenu(fileName = "e_", menuName = "ProjectCore/Events/Game Event Returns Vector3")]
    public class GameEventReturnsVector3 : GameEventWithReturn<Vector3?>
    {
        // No override needed: the base returns default (null) when unsubscribed.
    }
}
