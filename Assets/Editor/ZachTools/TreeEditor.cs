using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TreeEditor : UnityEditor.EditorWindow
{
    public Rect buttonRect = new UnityEngine.Rect(75,75,150,150);
    private List<Object> objects = new List<Object>();
    // Use this for initialization
    [UnityEditor.MenuItem("Tools/ZachTools/TreeEditor")]
    public static void Init()
    {
        var window = ScriptableObject.CreateInstance<TreeEditor>();
        window.Show();
    }
    private void OnGUI()
    {
        var contentRect = new Rect(buttonRect) { position = new Vector2(buttonRect.position.x, buttonRect.position.y + 15) };
        GUILayout.BeginArea(contentRect);
        GUILayout.Button("Make Tree");
        GUILayout.EndArea();
        var current = Event.current;
        
    }
}
