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
        public void DrawAnts(Data.AntData antData)
        {
 
        }
        public void Draw(ScriptableObject data)
        {
            GUILayout.BeginArea(ContentRect);
            var so = new SerializedObject(data);
            var properties = data.GetType().GetFields();
            foreach (var prop in properties)
            {
                var sp = so.FindProperty(prop.Name);
                EditorGUILayout.PropertyField(sp);
            }            
            GUILayout.EndArea();
            Debug.Log(data.GetType());
        }

        void dofieldstuff(ScriptableObject data)
        {
            if (data == null)
                return;
            var fields = data.GetType().GetFields();
            int count = 0;
            foreach (var field in fields)
            {
                count++;
                //if(field.FieldType == typeof(string))
                //{                    
                //    var newRect = new Rect(ContentRect);
                //    newRect.position = ContentRect.position + new Vector2(Padding.x, Padding.y + ( 30 * count ));
                //    newRect.size = new Vector2(Parent.Rect.size.x - Padding.x, 25);
                //    UnityEditor.EditorGUILayout.TextField(field.Name, field.GetValue(field.Name).ToString());
                //}
                if (field.FieldType == typeof(Vector3))
                {
                    var newRect = new Rect(ContentRect);
                    newRect.position = ContentRect.position + new Vector2(Padding.x, Padding.y + ( 30 * count ));
                    newRect.size = new Vector2(Parent.Rect.size.x - Padding.x, 25);
                    var vecString = field.GetValue(field).ToString();
                    var vals = vecString.Split(',');
                    var vec = new Vector3(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2]));
                    UnityEditor.EditorGUILayout.Vector3Field(field.Name, vec);
                }
            }
        }
    }
}