using Interfaces;
using UnityEngine;

namespace ChuTools
{
    [System.Serializable]
    public class UIBezierConnection : IDrawable
    {
        public IDrawable @in;
        public IDrawable @out;

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
    }
}