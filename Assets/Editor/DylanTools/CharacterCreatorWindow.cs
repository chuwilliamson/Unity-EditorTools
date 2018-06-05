using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DylanTools
{
    public class CharacterCreatorWindow : EditorWindow
    {
        private List<Node> Nodes = new List<Node>();

        [UnityEditor.MenuItem("Tools/Character Creator Window")]
        public static void Init()
        {
            var window = ScriptableObject.CreateInstance<CharacterCreatorWindow>();
            window.Show();
        }

        private void OnEnable()
        {
            EditorGlobals.mouseDownEvent += DisplayMenu;
        }

        void DisplayMenu()
        {
            var current = Event.current;

            if (current.button == 1)
            {
                GenericMenu CreateNodeMenu = new GenericMenu();
                CreateNodeMenu.AddItem(new GUIContent("CreateDraggable"), true, CreateNode);
                CreateNodeMenu.ShowAsContext();
            }
        }

        private void OnGUI()
        {
            EditorGlobals.GUIEvents();
            Nodes?.ForEach(n => n.Draw());
            Repaint();
        }

        void CreateNode()
        {
            var newNode = new DraggableNode("Sample", new Vector2(75, 75), new Vector2(100, 100), DeleteNode);
            Nodes.Add(newNode);
        }

        void DeleteNode(Node node)
        {
            Nodes.Remove(node);
        }
    }
}