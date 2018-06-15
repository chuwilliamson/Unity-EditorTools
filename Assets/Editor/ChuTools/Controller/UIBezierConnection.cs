using Interfaces;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace ChuTools.Controller
{
    [Serializable]
    public class UIBezierConnection : IDrawable
    {
        [JsonConstructor]
        public UIBezierConnection(IDrawable @in, IDrawable @out)
        {
            this.@in = @in;
            this.@out = @out;
        }

        public Rect Rect => @out.Rect;

        public void Draw()
        {
            Chutilities.DrawNodeCurve(@in.Rect.center, @out.Rect.center);
        }

        public IDrawable @in;
        public IDrawable @out;
    }
}