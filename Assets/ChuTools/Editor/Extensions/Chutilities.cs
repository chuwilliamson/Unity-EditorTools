using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChuTools.Extensions
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

        public static List<Object> GetAssetsOfType(string typename)
        {
            //var assets_ = AssetDatabase.FindAssets("t:" + typename);
            //var guids = assets_.Select(AssetDatabase.GUIDToAssetPath);
            //var objs = guids.Select(guid => AssetDatabase.LoadAssetAtPath(guid, typeof(T)));
            //var ret =  objs.Where(gameevent => gameevent).ToList() as List<T>;
            var t = Type.GetType(typename);
            var guids = AssetDatabase.FindAssets("t:" + typename);
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            var objs = paths.Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object)));
            var gameevents = objs.Where(ge => ge).ToList();


            return gameevents;
        }

        public static void DrawNodeCurve(Vector3 startPos, Vector3 endPos)
        {
            var startTan = startPos + Vector3.right * 50;
            var endTan = endPos + Vector3.left * 50;
            var shadowCol = new Color(0, 0, 0, 0.06f);
            for (var i = 0; i < 3; i++) // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
        }

        public static void DrawNodeCurve(Rect start, Rect end)
        {
            var startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
            var endPos = new Vector3(end.x, end.y + end.height / 2, 0);
            var startTan = startPos + Vector3.right * 50;
            var endTan = endPos + Vector3.left * 50;
            var shadowCol = new Color(0, 0, 0, 0.06f);
            for (var i = 0; i < 3; i++) // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
        }
    }
}