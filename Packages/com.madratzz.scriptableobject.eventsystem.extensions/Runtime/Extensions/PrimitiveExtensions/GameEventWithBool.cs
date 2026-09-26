using UnityEngine;

namespace ProjectCore.Events
{
    [CreateAssetMenu(fileName = "e_", menuName = "ProjectCore/Events/Game Event With Bool")]
    public class GameEventWithBool : GameEventWithParam<bool>
    {
        public override void Invoke(bool t)
        {
            base.Invoke(t);
        }
    }
}
