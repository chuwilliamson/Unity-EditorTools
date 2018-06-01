using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace JeremyTools
{
    //ToDo:: put this in a seperate file
    public class ConnectionPoint
    {
        public Rect rect;
        public string name;

        public ConnectionPoint(Rect r, string n)
        {
            rect = r;
            name = n;
        }

        public void Draw()
        {
            GUI.Box(rect, new GUIContent(name, name));
        }
    }
    //ToDo:: put this in a seperate file
    public class Connection
    {
        INode inNode;
        INode outNode;

        public Connection(INode inN, INode outN)
        {
            inNode = inN;
            outNode = outN;
        }

        public void Draw()
        {
            Handles.DrawLine(inNode.InCenter, outNode.OutCenter);
        }
    }
    //ToDo:: put this in a seperate file
    public class JNode : INode
    {
        // fields
        public ChuTools.IEventSystem EventSystem { get; set; }

        Rect OutRect
        {
            get
            {
                return new Rect(new Vector2(rect.xMax, (rect.center.y - (25 / 2))), new Vector3(25, 25));
            }
        }

        Rect InRect
        {
            get { return new Rect(new Vector2(rect.xMin - 25, (rect.center.y - (25 / 2))), new Vector3(25, 25)); }
        }

        public Vector2 OutCenter
        {
            get
            {
                return OutRect.center;
            }
        }

        public Vector2 InCenter
        {
            get
            {
                return InRect.center;
            }
        }

        public Rect rect;
        public ConnectionPoint outPoint, inPoint;
        public GUIContent content;
        public GUIStyle style;
        private System.Action<JNode> _onNodeDelete;
        public bool isSelected;

        //methods
        public JNode(Rect r, GUIContent c, GUIStyle s, ChuTools.IEventSystem eventSystem, System.Action<JNode> onNodeDelete) : this(r, c, s)
        {
            EventSystem = eventSystem;
            EventSystem.OnMouseDown += OnMouseDown;
            _onNodeDelete = onNodeDelete;
            EventSystem.OnContextClick += onContextClick;
            EventSystem.OnMouseDrag += OnMouseDrag;

            outPoint = new ConnectionPoint(OutRect, "out");
            inPoint = new ConnectionPoint(InRect, "in");
        }

        public JNode(Rect r, GUIContent c, GUIStyle s)
        {
            rect = r;
            content = c;
            style = s;
        }

        ~JNode()
        {
            EventSystem.OnMouseDown -= OnMouseDown;
            EventSystem.OnContextClick -= onContextClick;
            EventSystem = null;
        }

        public void onContextClick(Event e)
        {
            if (!rect.Contains(e.mousePosition)) return;
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("Delete Node"), false, () =>
            {
                EventSystem.OnMouseDown -= OnMouseDown;
                EventSystem.OnContextClick -= onContextClick;
                _onNodeDelete(this);
            });
            gm.ShowAsContext();
            GUI.changed = true;
        }

        public void OnMouseDown(Event e)
        {
            if (e.button == 0)
            {
                if (rect.Contains(e.mousePosition))
                {
                    Debug.Log("Left Down Node");
                    GUI.changed = true;
                }
            }
        }

        public void OnMouseUp(Event e)
        {
            if (e.button == 0)
            {
                Debug.Log("Left Up Node");
                GUI.changed = true;
            }
        }

        public void OnMouseDrag(Event e)
        {
            if (isSelected)
            {
                rect.position += e.delta;
                inPoint.rect.position += e.delta;
                outPoint.rect.position += e.delta;
            }
        }

        public void Draw()
        {
            GUI.Box(rect, content, style);
            inPoint.Draw();
            outPoint.Draw();
        }
    }
    //ToDo:: put this in a seperate file
    public class EditorBaseWindow : EditorWindow
    {
        // fields
        public List<JNode> nodes;
        public List<Connection> connections;
        bool isDrag = false;
        private Rect startRect, endRect;
        private JNode startNode, endNode;
        ChuTools.IEventSystem EventSystem = new ChuTools.NodeWindowEventSystem();

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