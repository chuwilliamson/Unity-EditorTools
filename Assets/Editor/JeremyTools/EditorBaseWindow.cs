using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class JButton
{
    public Rect Rect;
    public int ID;
    public JButton(Vector3 position, Vector2 size)
    {
        Rect = new Rect(position, size);
        ID = 0;
    }

    public void Draw()
    {
        GUI.Box(Rect, GUIContent.none);
        GUILayout.BeginArea(Rect);//group an area so it stays when this button moves
        GUILayout.Label(ID.ToString());
        GUILayout.EndArea();
    }
    public void UpdateData()
    {

    }

    public void PollEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (Event.current.button == 0)
                {
                    ID = GetHashCode();
                    GUI.changed = true;
                    
                }

                if (Event.current.button == 1)
                {
                    if (Rect.Contains(Event.current.mousePosition))
                    {
                        var gm = new GenericMenu();
                        gm.AddItem(new GUIContent("click me"), false, () => { Debug.Log("clicked"); });
                        gm.ShowAsContext();
                        Event.current.Use();
                    }

                }
                break;
        }
    }
}
public class EditorBaseWindow : EditorWindow
{
    // fields
    private Rect background;
    public List<JButton> buttons = new List<JButton>();
    bool isDrag = false;
    Rect startRect, endRect;

    // methods
    [MenuItem(itemName: "JeremyTools/NodeWindow")]
    static void OpenWindow()
    {
        var w = CreateInstance<EditorBaseWindow>();
        w.Show();

    }
    void OnEnable()
    {
        buttons = new List<JButton>()
        {
            new JButton(new Vector3(Screen.width / 2, Screen.height / 2, 0), new Vector2(150, 50))
        };
    }

    void PollEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                mdcount++;
                if (Event.current.button == 0)
                {
                    isDrag = true;
                    startRect = new Rect(Event.current.mousePosition, Vector2.one);
                    endRect = startRect;
                    Event.current.Use();
                    
                }
                if (Event.current.button == 1)
                {
                    cccount++;
                    var gm = new GenericMenu();
                    gm.AddItem(new GUIContent("add button"), false, () => { CreateButton(e.mousePosition); });
                    gm.ShowAsContext();
                }
                break;
        }

       
    }
   
    public int mdcount, mucount, cccount, mdrcount;
    void OnGUI()
    {
        PollEvents(Event.current);
        UpdateData();
        Draw();
        buttons.ForEach(b => b.PollEvents(Event.current));
        buttons.ForEach(b => b.UpdateData());
        buttons.ForEach(b => b.Draw());
        Repaint();

    }

    void CreateContextMenu(Event e)
    {

    }

    void CreateButton(Vector2 pos)
    {
        buttons.Add(new JButton(pos, new Vector2(150, 50)));
    }

    void UpdateData()
    {
        background.size = new Vector2(40, 40);
        background.position = new Vector3(1, 1, 1);
    }

    void Draw()
    {

        if (GUILayout.Button("DoIt"))
            ClearConsole();
        EditorGUILayout.IntField("button count", buttons.Count);
        EditorGUILayout.LabelField("start timer", Time.realtimeSinceStartup.ToString());

        GUILayout.BeginHorizontal();
        EditorGUILayout.RectField("startRect", startRect);
        EditorGUILayout.RectField("endRect", endRect);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("mdcount: ", mdcount.ToString());
        EditorGUILayout.LabelField("mucount: ", mucount.ToString());
        EditorGUILayout.LabelField("cccount: ", cccount.ToString());
        EditorGUILayout.LabelField("mdrcount: ", mdrcount.ToString());
        GUILayout.EndHorizontal();
        GUI.Box(background, new GUIContent("new Box 1"), GUIStyle.none);
    }

    static void ClearConsole()
    {
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }
}
