using Dialogue;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
namespace ChuTools
{
    public partial class Node
    {
        public class EditorDialogueNode
        {
            public Object Data;

            public void Draw()
            {
                Data = EditorGUILayout.ObjectField(Data, objType: typeof(DialogueRootObject), allowSceneObjects: false);
                EditorGUILayout.RectField(value: GUILayoutUtility.GetLastRect());
                if (Data != null)
                {
                    var so = new SerializedObject(Data);
                    var sp = so.FindProperty("Conversation");
                    var rp = sp.FindPropertyRelative("DialogueNodes");

                    if (EditorGUILayout.PropertyField(rp, true))
                    {

                    }
                    EditorGUILayout.RectField(value: GUILayoutUtility.GetLastRect());

                }
            }
        } 
    }
}