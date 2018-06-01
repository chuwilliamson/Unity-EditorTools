
using UnityEngine;

namespace Interfaces
{
    public interface IDrawable
    {
        Rect Rect {get;}
        void Draw();
    }
}