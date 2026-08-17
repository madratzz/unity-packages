using UnityEngine;

namespace ProjectCore.Events
{
    [CreateAssetMenu(fileName = "e_", menuName = "ProjectCore/Events/Game Event With Float")]
    public class GameEventWithFloat : GameEventWithParam<float>
    {
        public override void Invoke(float t)
        {
            base.Invoke(t);
        }
    }
}
