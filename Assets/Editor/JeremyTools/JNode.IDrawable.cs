using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace JeremyTools
{
    public partial class JNode : IDrawable
    {
        public Rect Rect => rect;

        public void Draw()
        {
            GUI.Box(rect, content, style);
            inPoint.Draw();
            outPoint.Draw();
        }

    }
}
