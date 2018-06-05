using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public void Draw(ScriptableObject data)
        {
            if (data == null)
                return;
            var properties = data.GetType().GetProperties();
            int count = 0;
            GUI.BeginGroup(ContentRect);
            foreach(var property in properties)
            {
                count++;
                if(property.GetType() == typeof(string))
                {                    
                    var newRect = new Rect(ContentRect);
                    newRect.position = ContentRect.position + new Vector2(Padding.x, Padding.y + ( 30 * count ));
                    newRect.size = new Vector2(Parent.Rect.size.x - Padding.x, 25);
                    UnityEditor.EditorGUILayout.TextField(property.Name, property.GetValue(property).ToString());
                }
            }
            GUI.EndGroup();
        }
    }
}