using Dialogue;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public partial class Node
    {
        public class EditorDialogueNode
        {
            public Object Data;

            public void Draw()
            {
                Data = EditorGUILayout.ObjectField(Data, typeof(DialogueRootObject), false);
                EditorGUILayout.RectField(GUILayoutUtility.GetLastRect());
                if(Data != null)
                {
                    var so = new SerializedObject(Data);
                    var sp = so.FindProperty("Conversation");
                    var rp = sp.FindPropertyRelative("DialogueNodes");

                    if(EditorGUILayout.PropertyField(rp, true))
                    {

                    }
                    EditorGUILayout.RectField(GUILayoutUtility.GetLastRect());

                }
            }
        }
    }
}