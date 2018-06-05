using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DylanTools
{
    
    public class Node : Interfaces.IDrawable
    {
        protected Rect VisualRect;
        private Rect ScaleRect;
        private Rect DeleteRect;
        protected Vector2 Position;
        protected Vector2 Scale;
        private Vector2 MinScale;
        protected string Name;        
        public System.Action<Node> _onDelete;        
        public ScriptableObject Scriptable;
        public delegate void OnScriptableChanged(ScriptableObject obj);
        public OnScriptableChanged scriptableChangedEvent;
        public Rect Rect
        {
            get
            {
                return VisualRect;
            }
        }

        public Node(string name, Vector2 position, Vector2 scale, System.Action<Node> onDelete) : this(name, position, scale)
        {
            _onDelete = onDelete;
        }

        protected Node(string name, Vector2 position, Vector2 scale)
        {
            Name = name;
            Position = position;
            Scale = scale;
            MinScale = scale;
            VisualRect = new Rect(position, Scale);
            EditorGlobals.mouseDragEvent += ScaleVisual;
        }

        public virtual void Draw()
        {            
            GUI.Box(VisualRect, Name);
            ScaleRect = new Rect(VisualRect.position, new Vector2(10, 10));
            ScaleRect.position += new Vector2(VisualRect.width - 10, VisualRect.height - 10);
            GUI.Box(ScaleRect, "");
            var objectRect = new Rect(VisualRect.position + new Vector2(0, 25), new Vector2(VisualRect.size.x, 20));                        
            Scriptable = EditorGUI.ObjectField(objectRect,Scriptable, typeof(ScriptableObject), false) as ScriptableObject;
            DeleteRect = new Rect(VisualRect.position, new Vector2(20, 20));
            if (GUI.Button(DeleteRect, "X"))
            {
                var gm = new UnityEditor.GenericMenu();
                gm.AddItem(new GUIContent("Remove Node"), false, RemoveNode, this);
                gm.ShowAsContext();
            }
            if (GUI.changed)
            {
                if(scriptableChangedEvent != null)
                    scriptableChangedEvent.Invoke(Scriptable);
            }
        }
        private void RemoveNode(object userdata)
        {
            _onDelete.Invoke(this);
        }

        private void ScaleVisual()
        {
            var current = Event.current;
            if (ScaleRect.Contains(current.mousePosition))
            {
                var newScale = VisualRect.size + current.delta;
                VisualRect.size += current.delta;
                if (VisualRect.size.x <= MinScale.x)
                    VisualRect.size = new Vector2(MinScale.x, VisualRect.size.y);
                if (VisualRect.size.y <= MinScale.y)
                    VisualRect.size = new Vector2(VisualRect.size.x, MinScale.y);
                current.Use();
            }
        }
    }
}