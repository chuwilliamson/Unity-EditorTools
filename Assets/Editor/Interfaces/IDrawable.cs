using UnityEngine;

namespace Interfaces
{
    public interface IDrawable
    {
        void Draw();
        Rect Rect { get; }
    }
}