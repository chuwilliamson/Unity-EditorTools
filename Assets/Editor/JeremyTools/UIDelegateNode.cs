using System.Collections;
using System.Collections.Generic;
using ChuTools;
using UnityEngine;

namespace JeremyTools
{
    public delegate void CustomCallback();

    public class UIDelegateNode : UIElement
    {
        public UIDelegateNode(Rect rect)
        {
            Base(rect: rect, name: "UIMethodNode");
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}