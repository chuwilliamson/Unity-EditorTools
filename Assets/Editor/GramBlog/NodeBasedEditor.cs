using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.GramBlog
{
    public class NodeBasedEditor : EditorWindow
    {
        List<Connection> connections;
        Vector2 drag;
        GUIStyle inPointStyle;
        Rect menuBar;

        readonly float menuBarHeight = 20f;
        List<Node> nodes;

        GUIStyle nodeStyle;

        Vector2 offset;
        GUIStyle outPointStyle;

        ConnectionPoint selectedInPoint;
        GUIStyle selectedNodeStyle;
        ConnectionPoint selectedOutPoint;

        [MenuItem("Window/Node Based Editor")]
        static void OpenWindow()
        {
            var window = GetWindow<NodeBasedEditor>();
            window.titleContent = new GUIContent("Node Based Editor");
        }

        void OnEnable()
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);

            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background =
                EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
            selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

            inPointStyle = new GUIStyle();
            inPointStyle.normal.background =
                EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            inPointStyle.active.background =
                EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            inPointStyle.border = new RectOffset(4, 4, 12, 12);

            outPointStyle = new GUIStyle();
            outPointStyle.normal.background =
                EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
            outPointStyle.active.background =
                EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
            outPointStyle.border = new RectOffset(4, 4, 12, 12);
        }

        void OnGUI()
        {
            DrawGrid(20, 0.2f, gridColor: Color.gray);
            DrawGrid(100, 0.4f, gridColor: Color.gray);
            DrawMenuBar();

            DrawNodes();
            DrawConnections();

            DrawConnectionLine(e: Event.current);

            ProcessNodeEvents(e: Event.current);
            ProcessEvents(e: Event.current);

            if (GUI.changed) Repaint();
        }

        void DrawMenuBar()
        {
            menuBar = new Rect(0, 0, width: position.width, height: menuBarHeight);

            GUILayout.BeginArea(screenRect: menuBar, style: EditorStyles.toolbar);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35)))
                Save();

            GUILayout.Space(5);

            if (GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35)))
                Load();

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            var widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            var heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(r: gridColor.r, g: gridColor.g, b: gridColor.b, a: gridOpacity);

            offset += drag * 0.5f;
            var newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (var i = 0; i < widthDivs; i++)
                Handles.DrawLine(new Vector3(gridSpacing * i, y: -gridSpacing, z: 0) + newOffset,
                    new Vector3(gridSpacing * i, y: position.height, z: 0f) + newOffset);

            for (var j = 0; j < heightDivs; j++)
                Handles.DrawLine(new Vector3(x: -gridSpacing, y: gridSpacing * j, z: 0) + newOffset,
                    new Vector3(x: position.width, y: gridSpacing * j, z: 0f) + newOffset);

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        void DrawNodes()
        {
            if (nodes != null)
                for (var i = 0; i < nodes.Count; i++)
                    nodes[index: i].Draw();
        }

        void DrawConnections()
        {
            if (connections != null)
                for (var i = 0; i < connections.Count; i++)
                    connections[index: i].Draw();
        }

        void ProcessEvents(Event e)
        {
            drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                        ClearConnectionSelection();

                    if (e.button == 1)
                        ProcessContextMenu(mousePosition: e.mousePosition);
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0)
                        OnDrag(delta: e.delta);
                    break;
            }
        }

        void ProcessNodeEvents(Event e)
        {
            if (nodes != null)
                for (var i = nodes.Count - 1; i >= 0; i--)
                {
                    var guiChanged = nodes[index: i].ProcessEvents(e: e);

                    if (guiChanged)
                        GUI.changed = true;
                }
        }

        void DrawConnectionLine(Event e)
        {
            if (selectedInPoint != null && selectedOutPoint == null)
            {
                Handles.DrawBezier(
                    startPosition: selectedInPoint.rect.center,
                    endPosition: e.mousePosition,
                    startTangent: selectedInPoint.rect.center + Vector2.left * 50f,
                    endTangent: e.mousePosition - Vector2.left * 50f,
                    color: Color.white,
                    texture: null,
                    width: 2f
                );

                GUI.changed = true;
            }

            if (selectedOutPoint != null && selectedInPoint == null)
            {
                Handles.DrawBezier(
                    startPosition: selectedOutPoint.rect.center,
                    endPosition: e.mousePosition,
                    startTangent: selectedOutPoint.rect.center - Vector2.left * 50f,
                    endTangent: e.mousePosition + Vector2.left * 50f,
                    color: Color.white,
                    texture: null,
                    width: 2f
                );

                GUI.changed = true;
            }
        }

        void ProcessContextMenu(Vector2 mousePosition)
        {
            var genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition: mousePosition));
            genericMenu.ShowAsContext();
        }

        void OnDrag(Vector2 delta)
        {
            drag = delta;

            if (nodes != null)
                for (var i = 0; i < nodes.Count; i++)
                    nodes[index: i].Drag(delta: delta);

            GUI.changed = true;
        }

        void OnClickAddNode(Vector2 mousePosition)
        {
            if (nodes == null)
                nodes = new List<Node>();

            nodes.Add(new Node(position: mousePosition, width: 200, height: 50, nodeStyle: nodeStyle,
                selectedStyle: selectedNodeStyle, inPointStyle: inPointStyle, outPointStyle: outPointStyle,
                OnClickInPoint: OnClickInPoint, OnClickOutPoint: OnClickOutPoint, OnClickRemoveNode: OnClickRemoveNode));
        }

        void OnClickInPoint(ConnectionPoint inPoint)
        {
            selectedInPoint = inPoint;

            if (selectedOutPoint != null)
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
        }

        void OnClickOutPoint(ConnectionPoint outPoint)
        {
            selectedOutPoint = outPoint;

            if (selectedInPoint != null)
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
        }

        void OnClickRemoveNode(Node node)
        {
            if (connections != null)
            {
                var connectionsToRemove = new List<Connection>();

                for (var i = 0; i < connections.Count; i++)
                    if (connections[index: i].inPoint == node.inPoint || connections[index: i].outPoint == node.outPoint)
                        connectionsToRemove.Add(connections[index: i]);

                for (var i = 0; i < connectionsToRemove.Count; i++)
                    connections.Remove(connectionsToRemove[index: i]);

                connectionsToRemove = null;
            }

            nodes.Remove(item: node);
        }

        void OnClickRemoveConnection(Connection connection)
        {
            connections.Remove(item: connection);
        }

        void CreateConnection()
        {
            if (connections == null)
                connections = new List<Connection>();

            connections.Add(new Connection(inPoint: selectedInPoint, outPoint: selectedOutPoint,
                OnClickRemoveConnection: OnClickRemoveConnection));
        }

        void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }

        void Save()
        {
            XMLOp.Serialize(item: nodes, path: "Assets/Resources/nodes.xml");
            XMLOp.Serialize(item: connections, path: "Assets/Resources/connections.xml");
        }

        void Load()
        {
            var nodesDeserialized = XMLOp.Deserialize<List<Node>>("Assets/Resources/nodes.xml");
            var connectionsDeserialized = XMLOp.Deserialize<List<Connection>>("Assets/Resources/connections.xml");

            nodes = new List<Node>();
            connections = new List<Connection>();

            foreach (var nodeDeserialized in nodesDeserialized)
                nodes.Add(new Node(
                        position: nodeDeserialized.rect.position,
                        width: nodeDeserialized.rect.width,
                        height: nodeDeserialized.rect.height,
                        nodeStyle: nodeStyle,
                        selectedStyle: selectedNodeStyle,
                        inPointStyle: inPointStyle,
                        outPointStyle: outPointStyle,
                        OnClickInPoint: OnClickInPoint,
                        OnClickOutPoint: OnClickOutPoint,
                        OnClickRemoveNode: OnClickRemoveNode,
                        inPointID: nodeDeserialized.inPoint.id,
                        outPointID: nodeDeserialized.outPoint.id
                    )
                );

            foreach (var connectionDeserialized in connectionsDeserialized)
            {
                var inPoint = nodes.First(n => n.inPoint.id == connectionDeserialized.inPoint.id).inPoint;
                var outPoint = nodes.First(n => n.outPoint.id == connectionDeserialized.outPoint.id).outPoint;
                connections.Add(new Connection(inPoint: inPoint, outPoint: outPoint,
                    OnClickRemoveConnection: OnClickRemoveConnection));
            }
        }
    }
}