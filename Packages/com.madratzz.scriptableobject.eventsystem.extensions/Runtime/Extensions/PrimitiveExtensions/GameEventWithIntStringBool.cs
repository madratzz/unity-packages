using UnityEngine;

namespace ProjectCore.Events
{
    [CreateAssetMenu(fileName = "e_", menuName = "ProjectCore/Events/Game Event With Int, String, Bool")]
    public class GameEventWithIntStringBool : GameEventWithParam<int, string,bool>
    {
        public override void Invoke(int t, string u,bool v)
        {
            base.Invoke(t, u,v);
        }
    }
}