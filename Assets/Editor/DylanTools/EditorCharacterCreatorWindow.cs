using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.UIElements;

public class EditorCharacterCreatorWindow : UnityEditor.EditorWindow
{
    private Stat stat;
    private Rect ButtonRect = new UnityEngine.Rect(75, 75, 150, 150);
    private Rect ContentRect;
    private Rect SelectedRect;
    private static List<Rect> GUIRects;
    private bool Draggable;
    private bool Selected;
    [UnityEditor.MenuItem("Tools/Node Editor")]
    public static void Init()
    {
        var window = UnityEngine.ScriptableObject.CreateInstance<EditorCharacterCreatorWindow>();
        window.Show();
    }

    private void OnGUI()
    {
        if (stat == null)
        {
            stat = new Stat("Strength", 10, "How strong are ya");
        }
        ContentRect = new Rect(ButtonRect);
        ContentRect.position = new Vector2(ButtonRect.position.x, ButtonRect.position.y + 25);
        ContentRect.size = new Vector2(ButtonRect.width, ButtonRect.height - 25);
        Draggable = GUI.Toggle(new Rect(ButtonRect.position.x + ButtonRect.width, ButtonRect.position.y, 20, 20), Draggable, "");
        GUI.Box(ButtonRect, "Stat");
        EditorGUILayout.RectField(ButtonRect);
        GUILayout.BeginArea(ContentRect);
        stat.Name = GUILayout.TextField(stat.Name, GUILayout.Width(ButtonRect.width - 5));
        GUILayout.Label("Value:" + stat.Value);
        ButtonRect.width = GUILayout.HorizontalSlider(ButtonRect.width, 150, 200, GUILayout.Width(75));
        ButtonRect.height = GUILayout.HorizontalSlider(ButtonRect.height, 150, 200, GUILayout.Width(75));
        var current = Event.current;
        GUILayout.EndArea();
        if (GUI.changed)
            Repaint();
        switch (current.type)
        {
            case EventType.MouseDown:
                Selected = ButtonRect.Contains(Event.current.mousePosition);
                current.Use();
                break;
            case EventType.MouseDrag:
                if (Draggable && Selected)
                {
                    ButtonRect.position += Event.current.delta;
                    current.Use();
                }
                break;
        }
    }
}

public class Stat
{
    public string Name { get; set; }
    public string Description { get; set; }
    public float Value { get; set; }

    public Stat(string name, float val, string des)
    {
        Name = name;
        Value = val;
        Description = des;
    }
}
