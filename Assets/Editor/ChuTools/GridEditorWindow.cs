using ChuTools;
using Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GridEditorWindow : CustomEditorWindow
{
    private void OnEnable()
    {
        EventSystem = new GridWindowEventSystem();
    }

    [MenuItem(itemName: "Tools/ChuTools/GridEditor")]
    public static void Init()
    {
        var w = CreateInstance<GridEditorWindow>();
        w.Show();
    }

    [System.Serializable]
    public class Cell
    {
        public int Index;
        public Vector3 Position;
    }

    private Vector2Int _dimensions;
    private List<Cell> _cells;
    public Rect MenuRect => new Rect(5, 5, 300, 300);
    public Rect SideRect => new Rect(305, 5, 300, 300);
    public Rect BottomRect => new Rect(5, 315, 600, Screen.height - 345);

    private int _selected;

    public GUIStyle NormalStyle => new GUIStyle("CN Box") { fontSize = 15, alignment = TextAnchor.UpperCenter, padding = new RectOffset(15, 15, 15, 15) };

    private void OnGUI()
    {
        EventSystem.PollEvents(Event.current);
        DrawMainMenu();
        DrawSideMenu();
        DrawBottomMenu();

        if (GUI.changed)
            Repaint();
    }

    public void DrawBottomMenu()
    {
        GUILayout.BeginArea(BottomRect, new GUIContent("GridView"), NormalStyle);

        _cells?.ForEach(cell =>
        {
            var x = cell.Position.x * 25 + 5;
            var z = cell.Position.z * 25 + 5;
            GUI.Box(new Rect(x * 2, z * 2, 50, 50), new GUIContent(cell.Index.ToString()));
        });

        GUILayout.EndArea();
    }

    public void DrawMainMenu()
    {
        GUILayout.BeginArea(MenuRect, new GUIContent("Menu"), NormalStyle);

        GUILayout.Space(20);
        EditorGUILayout.LabelField("Dimensions");
        EditorGUI.indentLevel++;
        EditorGUI.BeginChangeCheck();
        _dimensions.x = EditorGUILayout.IntSlider(new GUIContent("Width"), _dimensions.x, 1, 10);
        _dimensions.y = EditorGUILayout.IntSlider(new GUIContent("Height"), _dimensions.y, 1, 10);
        EditorGUI.indentLevel--;

        if (EditorGUI.EndChangeCheck())
        {
            _selected = -1;
            _cells = CreateGrid(_dimensions);
        }

        if (_cells?.Count > 0)
        {
            var names = _cells.Select(cell => cell.Index.ToString()).ToArray();
            _selected = GUILayout.SelectionGrid(_selected, names, _dimensions.x);
        }

        GUILayout.EndArea();
    }

    public void DrawSideMenu()
    {
        GUILayout.BeginArea(SideRect, new GUIContent("Cell Info"), NormalStyle);

        GUILayout.Space(20);

        var obj = _selected > -1 ? _cells?[_selected] : null;
        EditorGUILayout.LabelField(new GUIContent("Index: " + obj?.Index));
        EditorGUILayout.LabelField(new GUIContent("Position: " + obj?.Position));

        GUILayout.EndArea();
    }

    private static List<Cell> CreateGrid(Vector2 dims)
    {
        var result = new List<Cell>();
        var count = 0;
        for (var i = 0; i < dims.x; i++)
        {
            for (var j = 0; j < dims.y; j++)
            {
                result.Add(new Cell { Index = count, Position = new Vector3(i, 0, j) });
                count++;
            }
        }

        return result;
    }

    public override IEventSystem EventSystem { get; set; }
}