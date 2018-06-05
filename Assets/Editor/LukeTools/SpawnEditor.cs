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
        Rect buttonRect = new Rect(x: 50, y: 50, width: 150, height: 100);
        private Object Prefab;
        private GameObject SpawnedPrefab;

        void Draw()
        {
            var content = new Rect(buttonRect);
            GUI.Box(buttonRect, "");
            GUILayout.BeginArea(buttonRect);
            GUILayout.Label(buttonRect.position.ToString());
            Prefab = EditorGUILayout.ObjectField(Prefab, typeof(GameObject), false);
            GUILayout.EndArea();
        }



        public bool isSelected = false;
        public void PollEvents()
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    if (Event.current.button == 0)
                    {
                        DestroyImmediate(SpawnedPrefab);
                        SpawnedPrefab = Instantiate(Prefab, buttonRect.position, Quaternion.identity) as GameObject;
                        if (buttonRect.Contains(Event.current.mousePosition))
                        {
                            isSelected = true;
                            GUI.changed = true;
                        }
                        else
                        {
                            isSelected = false;
                            GUI.changed = true;
                        }
                    }
                    break;
                case EventType.MouseDrag:
                    if (Event.current.button == 0)
                    {
                        if (isSelected)
                        {
                            buttonRect.position += Event.current.delta;
                            Event.current.Use();
                        }
                    }
                    break;
            }
        }

        void OnGUI()
        {
            Draw();
            PollEvents();
        }
    }
}
