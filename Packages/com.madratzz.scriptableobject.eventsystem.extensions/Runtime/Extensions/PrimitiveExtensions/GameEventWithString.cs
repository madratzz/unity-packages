using UnityEngine;

namespace ProjectCore.Events
{
    [CreateAssetMenu(fileName = "e_", menuName = "ProjectCore/Events/Game Event With String")]
    public class GameEventWithString : GameEventWithParam<string>
    {
        public override void Invoke(string t)
        {
            base.Invoke(t);
        }
    }
}

