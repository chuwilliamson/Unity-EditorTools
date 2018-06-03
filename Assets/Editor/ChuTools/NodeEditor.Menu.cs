﻿using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public partial class NodeEditorWindow
    {
        public void DrawMenu()
        {
            GUILayout.BeginHorizontal();
            var value1 = "null";
            var value2 = CurrentDrag?.ToString() ?? "null";
            if (GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35))) Save();
            GUILayout.Space(5);
            if (GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35))) Load();
            GUILayout.EndHorizontal();

            var lastrect = GUILayoutUtility.GetLastRect();
            var pos = new Vector2(lastrect.xMin, lastrect.yMax);
            var menurect = new Rect(pos, new Vector2(250, 200));


            GUI.BeginGroup(menurect);
            GUI.Box(menurect, GUIContent.none);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Width", Screen.width.ToString());
            wantsMouseMove = EditorGUILayout.Toggle("WantsMouseMove", wantsMouseMove);
            EditorGUILayout.LabelField("Height", Screen.height.ToString());
            EditorGUILayout.LabelField("HotControl: ", GUIUtility.hotControl.ToString());
            EditorGUILayout.LabelField("Control Name: ", GUI.GetNameOfFocusedControl());
            EditorGUILayout.LabelField("Path", _path); 
            EditorGUILayout.LabelField("Current Drag  ", value2);


            EditorGUILayout.EndVertical();
            GUI.EndGroup();
        }
    }
}