using Newtonsoft.Json;
using System;
using UnityEngine;

namespace ChuTools.Controller
{
    [Serializable]
    public class UIConnectionPoint : UIElement
    {
        [JsonConstructor]
        public UIConnectionPoint(string name, string normalStyleName, string selectedStyleName, Rect rect)
        {
            Base(name: name, normalStyleName: normalStyleName, selectedStyleName: selectedStyleName, rect: rect);
        }
    }
}