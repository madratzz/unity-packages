using System.Collections;
using System.Collections.Generic;
using ProjectCore.Events;
using UnityEngine;
[CreateAssetMenu(fileName = "e_", menuName = "ProjectCore/Events/Game Event With Sprite")]

public class GameEventWithSprite : GameEventWithParam<Sprite>
{
    public override void Invoke(Sprite t)
    {
        base.Invoke(t);
    }
}
