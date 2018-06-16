using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ChuTools.View
{
    public partial class NodeEditorWindow
    {
        public void DrawCommandMenu()
        {
            var files = Directory.GetFiles(Application.streamingAssetsPath).ToList();

            if(GUILayout.Button("Open Command Prompt"))
                WorkerProcesses.Add(Process.Start("cmd.exe"));
            GUILayout.Label("StreamingAssets", EditorStyles.boldLabel);
            foreach (var f in files)
            {
                var nf = f.Remove(0, Application.streamingAssetsPath.Length + 1);
                if(nf.Contains(".meta"))
                    continue;

                if(GUILayout.Button(nf))
                    WorkerProcesses.Add(Process.Start(f));
            }

            WorkerProcesses.ForEach(p =>
            {
                EditorGUILayout.BeginHorizontal();

                if(GUILayout.Button(new GUIContent("Kill? " + p.ProcessName), EditorStyles.foldout))
                {
                    p.Kill();
                    WorkerProcesses.Remove(p);
                }
                EditorGUILayout.EndHorizontal();
            });
            EditorGUILayout.EndVertical();
        }

        public void DrawMenu()
        {
            GUILayout.BeginHorizontal();
            var value1 = CurrentSendingDrag?.ToString() ?? "null";
            var value2 = CurrentAcceptingDrag?.ToString() ?? "null";
            if(value2 != "null")
                Debug.Break();
            if(GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35))) Save();
            GUILayout.Space(5);
            if(GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35))) Load();
            GUILayout.EndHorizontal();

            var lastrect = GUILayoutUtility.GetLastRect();
            var pos = new Vector2(lastrect.xMin, lastrect.yMax);
            var menurect = new Rect(pos, new Vector2(550, 400));

            GUILayout.BeginArea(menurect);
            GUI.BeginGroup(menurect);
            GUI.Box(menurect, GUIContent.none);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.TextField("Path", _path, GUILayout.ExpandWidth(true));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Width", Screen.width.ToString());
            EditorGUILayout.LabelField("Height", Screen.height.ToString());
            EditorGUILayout.EndHorizontal();
            wantsMouseMove = EditorGUILayout.Toggle("WantsMouseMove", wantsMouseMove);
            EditorGUILayout.LabelField("HotControl: ", GUIUtility.hotControl.ToString());
            EditorGUILayout.LabelField("Current Sending Drag  ", value1);
            EditorGUILayout.LabelField("Current Requesting Drag  ", value2);
            EditorGUILayout.LabelField("Node Count: " + Nodes.Count);
            EditorGUILayout.LabelField("Connections Count: " + Connections.Count);

            GUI.EndGroup();
            GUILayout.EndArea();
        }

        public List<Process> WorkerProcesses = new List<Process>();
    }
}