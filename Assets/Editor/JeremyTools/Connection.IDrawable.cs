using Interfaces;
using UnityEditor;
using UnityEngine;

namespace JeremyTools
{
    public partial class Connection : IDrawable
    {
        public Rect Rect => new Rect(In.Rect.position - Out.Rect.position, new Vector2(25, 25));

        public void Draw()
        {
            Handles.DrawLine(In.Rect.center, Out.Rect.center);
        }
    }
}