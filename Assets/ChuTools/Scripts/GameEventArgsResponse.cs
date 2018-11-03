using System;
using UnityEngine.Events;

namespace ChuTools.Scripts
{
    [Serializable]
    public class GameEventArgsResponse : UnityEvent<object[]>, IResponder
    {
    }
}