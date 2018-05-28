using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChuTools
{
    public class RemoveComponentsWindow : EditorWindow
    {
        [MenuItem("Tools/ChuTools/Remove Components Editor")]
        private static void Init()
        {
            var w = (RemoveComponentsWindow) GetWindow(t: typeof(RemoveComponentsWindow));
            w.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Remove Components On Selected", GUILayout.ExpandWidth(false)))
                RemoveComponents(Selection.activeGameObject);
        }

        private static void RemoveComponents(GameObject go)
        {
            var children = new List<MeshCollider>();
            go.GetComponentsInChildren(children);
            children.ForEach(child => DestroyImmediate(child, false));
        }
    }
}