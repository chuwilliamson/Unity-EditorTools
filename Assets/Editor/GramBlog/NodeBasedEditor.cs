using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Editor.GramBlog
{
    public class NodeBasedEditor : EditorWindow
    {
        private List<Connection> connections;
        private Vector2 drag;
        private GUIStyle inPointStyle;
        private Rect menuBar;
        private const float menuBarHeight = 20f;
        private List<Node> nodes;
        private GUIStyle nodeStyle;
        private Vector2 offset;
        private GUIStyle outPointStyle;
        private ConnectionPoint selectedInPoint;
        private GUIStyle selectedNodeStyle;
        private ConnectionPoint selectedOutPoint;

        [MenuItem("Window/Node Based Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<NodeBasedEditor>();
            window.titleContent = new GUIContent("Node Based Editor");
        }

        private void OnEnable()
        {
            nodeStyle = new GUIStyle
            {
                normal = {background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D},
                border = new RectOffset(12, 12, 12, 12)
            };

            selectedNodeStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D
                },
                border = new RectOffset(12, 12, 12, 12)
            };

            inPointStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D
                },
                active =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D
                },
                border = new RectOffset(4, 4, 12, 12)
            };

            outPointStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D
                },
                active =
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D
                },
                border = new RectOffset(4, 4, 12, 12)
            };
        }

        private void OnGUI()
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

        private void DrawMenuBar()
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

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
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

        private void DrawNodes()
        {
            if (nodes == null) return;
            foreach (var t in nodes)
                t.Draw();
        }

        private void DrawConnections()
        {
            if (connections == null) return;
            foreach (var t in connections)
                t.Draw();
        }

        private void ProcessEvents(Event e)
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
                case EventType.DragExited:
                    if (e.button == 0)
                    {
                        
                    }
                    break;
            }
        }

        private void ProcessNodeEvents(Event e)
        {
            if (nodes == null) return;
            for (var i = nodes.Count - 1; i >= 0; i--)
            {
                var guiChanged = nodes[index: i].ProcessEvents(e: e);

                if (guiChanged)
                    GUI.changed = true;
            }
        }

        private void DrawConnectionLine(Event e)
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

            if (selectedOutPoint == null || selectedInPoint != null) return;
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

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            var genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition: mousePosition));
            genericMenu.ShowAsContext();
        }

        private void OnDrag(Vector2 delta)
        {
            drag = delta;

            if (nodes != null)
                foreach (var t in nodes)
                    t.Drag(delta: delta);

            GUI.changed = true;
        }

        private void OnClickAddNode(Vector2 mousePosition)
        {
            if (nodes == null)
                nodes = new List<Node>();

            nodes.Add(new Node(position: mousePosition, width: 200, height: 50, nodeStyle: nodeStyle,
                selectedStyle: selectedNodeStyle, inPointStyle: inPointStyle, outPointStyle: outPointStyle,
                onClickInPoint: OnClickInPoint, onClickOutPoint: OnClickOutPoint,
                onClickRemoveNode: OnClickRemoveNode));
        }

        private void OnClickInPoint(ConnectionPoint inPoint)
        {
            selectedInPoint = inPoint;

            if (selectedOutPoint == null) return;
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

        private void OnClickOutPoint(ConnectionPoint outPoint)
        {
            selectedOutPoint = outPoint;

            if (selectedInPoint == null) return;
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

        private void OnClickRemoveNode(Node node)
        {
            if (connections != null)
            {
                var connectionsToRemove = new List<Connection>();

                foreach (Connection t in connections)
                    if (t.inPoint == node.inPoint ||
                        t.outPoint == node.outPoint)
                        connectionsToRemove.Add(t);

                foreach (Connection t in connectionsToRemove)
                    connections.Remove(t);
            }

            nodes.Remove(item: node);
        }

        private void OnClickRemoveConnection(Connection connection)
        {
            connections.Remove(item: connection);
        }

        private void CreateConnection()
        {
            if (connections == null)
                connections = new List<Connection>();

            connections.Add(new Connection(inPoint: selectedInPoint, outPoint: selectedOutPoint,
                onClickRemoveConnection: OnClickRemoveConnection));
        }

        private void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }

        private void Save()
        {
            XMLOp.Serialize(item: nodes, path: "Assets/Resources/nodes.xml");
            XMLOp.Serialize(item: connections, path: "Assets/Resources/connections.xml");
        }

        private void Load()
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
                        onClickInPoint: OnClickInPoint,
                        onClickOutPoint: OnClickOutPoint,
                        onClickRemoveNode: OnClickRemoveNode,
                        inPointId: nodeDeserialized.inPoint.id,
                        outPointId: nodeDeserialized.outPoint.id
                    )
                );

            foreach (var connectionDeserialized in connectionsDeserialized)
            {
                var inPoint = nodes.First(n => n.inPoint.id == connectionDeserialized.inPoint.id).inPoint;
                var outPoint = nodes.First(n => n.outPoint.id == connectionDeserialized.outPoint.id).outPoint;
                connections.Add(new Connection(inPoint: inPoint, outPoint: outPoint,
                    onClickRemoveConnection: OnClickRemoveConnection));
            }
        }
    }
}