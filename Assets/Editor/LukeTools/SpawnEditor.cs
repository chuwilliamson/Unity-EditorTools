using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LukeTools
{

    public class SpawnEditor : UnityEditor.EditorWindow
    {
        [MenuItem("Tools/LukeTools/SpawnEditor")]

        public static void Init()
        {
            Debug.Log("");
            var w = ScriptableObject.CreateInstance<SpawnEditor>();
            w.Show();
        }
        void Draw()
        {
            Rect buttonRect = new UnityEngine.Rect(x: 50, y: 50, width: 150, height: 150);
            var content = new Rect(buttonRect);
            GUI.Box(buttonRect, "object");
        }


        void OnGUI()
        {
            Draw();
        }
    }
}
