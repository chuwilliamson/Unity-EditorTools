using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public class NodeEditorWindow : CustomEditorWindow
    {
        private List<IDrawable> _drawables = new List<IDrawable>();

        [MenuItem("Tools/ChuTools/NodeWindow")]
        private static void Init()
        {
            var window = GetWindow<NodeEditorWindow>();
         
            window.Show();
        }

        void OnEnable()
        {
            _drawables = new List<IDrawable>();
            wantsMouseMove = true;
            NodeEventSystem.Selected = this;
            NodeEventSystem.WillSelect = this;
            NodeEventSystem.OnContextClick += CreateContextMenu;
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            wantsMouseMove = EditorGUILayout.Toggle(wantsMouseMove);
            
            EditorGUILayout.LabelField("width", Screen.width.ToString());
            EditorGUILayout.LabelField("height", Screen.height.ToString());

            var value1 = NodeEventSystem.Selected == null ? "null" : NodeEventSystem.Selected.ToString();
            var value2 = NodeEventSystem.WillSelect == null? "null" : NodeEventSystem.WillSelect.ToString();
 
            EditorGUILayout.LabelField("EventSystem Selected", value1);
            EditorGUILayout.LabelField("EventSystem Will Selected   ", value2); 
            EditorGUILayout.EndVertical();

            _drawables.ForEach(n => n.Draw(Event.current));
            NodeEventSystem.PollEvents(e: Event.current);
            Repaint();
        }

        private void CreateContextMenu(Event e)
        {
            var pos = e.mousePosition;
            var gm = new GenericMenu();
            gm.AddItem(content: new GUIContent("Create Node"), on: false, func: () => { CreateNode(pos); });
            gm.AddItem(content: new GUIContent("Clear Nodes"), on: false, func: ClearNodes);
            gm.ShowAsContext();
            e.Use();
        }

        private void CreateNode(Vector2 pos)
        {
            _drawables.Add(item: new Node(position: pos, id: _drawables.Count, eventSystem: NodeEventSystem));
        }

        private void ClearNodes()
        {
            _drawables = new List<IDrawable>();
        }
    }
}