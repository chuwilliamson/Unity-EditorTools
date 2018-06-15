using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DylanTools
{
    public interface IContent
    {
        Rect Rect { get; }
        Vector2 ContentPadding { get; }
        Node Parent { get; }
        void Draw(ScriptableObject data);
    }
}
