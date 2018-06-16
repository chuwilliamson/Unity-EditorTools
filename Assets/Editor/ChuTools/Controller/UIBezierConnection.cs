using System;
using Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace ChuTools.Controller
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

        public Rect Rect => Out.Rect;

        public void Draw()
        {
            Chutilities.DrawNodeCurve(In.Rect.center, Out.Rect.center);
        }

        public IDrawable In { get; set; }
        public IDrawable Out { get; set; }
    }
}