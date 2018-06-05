using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BrettTools
{
    public class PrefabEditorWindow : EditorWindow
    {
        [MenuItem(itemName: "Tools/BrettTools/PrefabSpawner")]

        public
        static void Init()
        {
            Debug.Log("Make Window");
            var window = GetWindow<PrefabEditorWindow>();
            window.Show();
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(0, 0, 100, 100), "Prefab Spawner");
        }


    }
}