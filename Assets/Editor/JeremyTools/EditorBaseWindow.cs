using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Interfaces;

namespace JeremyTools
{
    public class EditorBaseWindow : EditorWindow
    {
        // fields
        public List<JNode> nodes;
        public List<Connection> connections;
        bool isDrag = false;
        private Rect startRect, endRect;
        private JNode startNode, endNode;
        private IEventSystem EventSystem = new ChuTools.NodeWindowEventSystem();

        // methods
        void OnEnable()
        {
            startRect = new Rect(Vector2.zero, Vector2.zero);
            endRect = new Rect(Vector2.zero, Vector2.zero);
            wantsMouseMove = true;
            nodes = new List<JNode>();
            connections = new List<Connection>();
            EventSystem = new ChuTools.NodeWindowEventSystem();
            EventSystem.OnMouseDown += OnMouseDown;
            EventSystem.OnMouseUp += OnMouseUp;
            EventSystem.OnMouseDrag += OnMouseDrag;
            EventSystem.OnContextClick += onContextClick;
        }

        void Draw()
        {
            nodes.ForEach(n => n.Draw());
            connections.ForEach(c => c.Draw());

            EditorGUILayout.IntField("nodes", nodes.Count);
            EditorGUILayout.IntField("connections", connections.Count);
            EditorGUILayout.RectField("start", startRect);
            EditorGUILayout.RectField("end", endRect);
            if (GUILayout.Button("Reopen Window"))
            {
                ClearWindow();
            }
            if (GUILayout.Button("Clear Console"))
            {
                ClearConsole();
            }

            if (isDrag)
            {
                DrawLine();
            }
            GUI.changed = true;
        }

        void OnGUI()
        {
            EventSystem.PollEvents(Event.current);
            Draw();
            if (GUI.changed)
                Repaint();
        }

        void OnMouseDown(Event e)
        {
            if (e.button == 0)
            {
                foreach (var n in nodes)
                {
                    if (n.outPoint.rect.Contains(e.mousePosition))
                    {
                        startNode = n;
                        startRect.position = e.mousePosition;
                        endRect = startRect;
                        isDrag = true;
                        Debug.Log("Left Down Connection Point");
                    }
                    if (n.rect.Contains(e.mousePosition))
                    {
                        n.isSelected = true;
                        Debug.Log("Left Down Node");
                    }
                    else
                    {
                        n.isSelected = false;
                        Debug.Log("Left Down Node");
                    }
                }
                GUI.changed = true;
            }
        }

        void OnMouseUp(Event e)
        {
            if (e.button == 0)
            {
                foreach (JNode n in nodes)
                {
                    if (isDrag)
                    {
                        if (n.inPoint.rect.Contains(e.mousePosition))
                        {
                            endNode = n;
                            connections.Add(new Connection(endNode, startNode));
                        }
                    }
                    if (n.outPoint.rect.Contains(e.mousePosition))
                    {
                        endRect.position = e.mousePosition;
                    }
                    if (n.rect.Contains(e.mousePosition))
                    {
                        n.isSelected = false;
                    }
                }
                isDrag = false;
                GUI.changed = true;
            }
        }

        void OnMouseDrag(Event e)
        {
            endRect.position = e.mousePosition;
            Handles.DrawLine(startRect.position, endRect.position);
        }

        public void onContextClick(Event e)
        {
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("Create Node"), false, () => { CreateNode(e); });
            gm.ShowAsContext();
            GUI.changed = true;
        }

        void CreateNode(Event e)
        {
            var rect = new Rect(e.mousePosition, new Vector2(100, 100));
            var content = new GUIContent(Resources.Load("white-square") as Texture2D, ("Node" + nodes.Count));
            nodes.Add(new JNode(rect, content, new GUIStyle(), EventSystem, RemoveNode));
        }

        void RemoveNode(JNode node)
        {
            if (!nodes.Contains(node))
                return;
            nodes.Remove(node);
        }

        [MenuItem(itemName: "Tools/JeremyTools/NodeWindow")]
        static void OpenWindow()
        {
            var w = CreateInstance<EditorBaseWindow>();
            w.Show();
        }

        static void ClearConsole()
        {
            var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            clearMethod.Invoke(null, null);
            GUI.changed = true;
            Debug.Log("Console Cleared.");
        }

        void ClearWindow()
        {
            OpenWindow();
            Close();
        }

        void DrawLine()
        {
            Handles.DrawLine(startRect.position, endRect.position);
        }
    }
}