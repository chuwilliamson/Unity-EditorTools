using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public static class Chutilities
    {
        public static Rect GetRectOffset(Rect r, Vector2 width, Vector2 height)
        {
            var offrect = new Rect(r);
            offrect.xMin += width.x;
            offrect.xMax -= width.y;
            offrect.yMin += height.x;
            offrect.yMax -= height.y;
            return offrect;
        }
        public static void DrawNodeCurve(Rect start, Rect end)
        {
            var startPos = new Vector3(x: start.x + start.width, y: start.y + start.height / 2, z: 0);
            var endPos = new Vector3(end.x, y: end.y + end.height / 2, z: 0);
            var startTan = startPos + Vector3.right * 50;
            var endTan = endPos + Vector3.left * 50;
            var shadowCol = new Color(0, 0, 0, 0.06f);
            for (var i = 0; i < 3; i++) // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, width: (i + 1) * 5);
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
        }
    }
}