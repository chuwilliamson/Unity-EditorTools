using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DylanTools
{
    public class ScriptableVisual : IContent
    {
        private Rect ContentRect;
        private Node ParentNode;
        private Vector2 Padding;

        public Rect Rect
        {
            get
            {
                return ContentRect;
            }
        }

        public Vector2 ContentPadding
        {
            get
            {
                return Padding;
            }
        }

        public Node Parent
        {
            get
            {
                return ParentNode;
            }
        }

        public ScriptableVisual(Node parent)
        {
            ParentNode = parent;
            ContentRect = new Rect(ParentNode.Rect);
            Padding = new Vector2(0, 30);
            ParentNode.scriptableChangedEvent += Draw;
        }
   
        public void Draw(ScriptableObject data)
        {
            if(data == null)
                return;
            GUILayout.BeginArea(ContentRect);
            var so = new SerializedObject(data);
            var properties = data.GetType().GetFields();
            foreach (var prop in properties)
            {
                if(prop.IsNotSerialized)
                    continue;
                var sp = so.FindProperty(prop.Name);
                EditorGUILayout.PropertyField(sp);
            }            
            GUILayout.EndArea();                 
        }
    }
}