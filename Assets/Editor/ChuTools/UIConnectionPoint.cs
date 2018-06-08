using System;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public class UIConnectionPoint : UIElement
    {
        public UIConnectionPoint(string name, string normalStyleName, string selectedStyleName, Rect rect)
        {
            Base(name: name, normalStyleName: normalStyleName, selectedStyleName: selectedStyleName, rect: rect);
        }
    }
}