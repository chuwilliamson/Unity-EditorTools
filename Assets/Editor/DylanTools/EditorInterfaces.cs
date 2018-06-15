using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorInterfaces
{
    interface IDrawable
    {
        Rect _Rect { get; }
        void Draw();
    }

    interface IDraggable
    {
        bool IsDraggable { get; }
        Vector2 Positon { get; set; }
        void EnableDragging();
        void Drag();
        void DisableDragging();
    }

    interface IResizable
    {
        Rect DragRect { get; }
        bool IsResizable { get; }
        Vector2 Scale { get; }
        void EnableResize();
        void Resize();
        void DisableResize();
    }
}