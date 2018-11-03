using UnityEngine;

namespace ChuTools.NodeEditor.Interfaces
{
    public interface IDrawable
    {
        Rect Rect { get; }

        void Draw();
    }
}