using ChuTools.Extensions;
using ChuTools.NodeEditor.Interfaces;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace ChuTools.NodeEditor.Controller
{
    [Serializable]
    public class UIBezierConnection : IDrawable
    {
        [JsonConstructor]
        public UIBezierConnection(IDrawable @in, IDrawable @out)
        {
            In = @in;
            Out = @out;
        }

        public IDrawable In { get; set; }
        public IDrawable Out { get; set; }

        public Rect Rect => Out.Rect;

        public void Draw()
        {
            Chutilities.DrawNodeCurve(In.Rect.center, Out.Rect.center);
        }
    }
}