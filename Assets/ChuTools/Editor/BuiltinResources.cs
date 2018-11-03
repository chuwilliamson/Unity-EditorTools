using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class BuiltInResourcesWindow : EditorWindow
{
    private float _maxY;

    private List<Object> _objects;
    private Rect _oldPosition;
    private float _scrollPos;

    private string _search = "";
    private bool _showingIcons;

    private bool _showingStyles = true;

    private List<Drawing> Drawings;

    [MenuItem("Tools/Built-in styles and icons")]
    public static void ShowWindow()
    {
        var w = GetWindow<BuiltInResourcesWindow>();
        w.Show();
    }

    private void OnGUI()
    {
        if (position.width != _oldPosition.width && Event.current.type == EventType.Layout)
        {
            Drawings = null;
            _oldPosition = position;
        }

        GUILayout.BeginHorizontal();

        if (GUILayout.Toggle(_showingStyles, "Styles", EditorStyles.toolbarButton) != _showingStyles)
        {
            _showingStyles = !_showingStyles;
            _showingIcons = !_showingStyles;
            Drawings = null;
        }

        if (GUILayout.Toggle(_showingIcons, "Icons", EditorStyles.toolbarButton) != _showingIcons)
        {
            _showingIcons = !_showingIcons;
            _showingStyles = !_showingIcons;
            Drawings = null;
        }

        GUILayout.EndHorizontal();

        var newSearch = GUILayout.TextField(_search);
        if (newSearch != _search)
        {
            _search = newSearch;
            Drawings = null;
        }

        float top = 36;

        if (Drawings == null)
        {
            var lowerSearch = _search.ToLower();

            Drawings = new List<Drawing>();

            var inactiveText = new GUIContent("inactive");
            var activeText = new GUIContent("active");

            var x = 5.0f;
            var y = 5.0f;

            if (_showingStyles)
            {
                foreach (var ss in GUI.skin.customStyles)
                {
                    if (lowerSearch != "" && !ss.name.ToLower().Contains(lowerSearch))
                        continue;

                    var thisStyle = ss;

                    var draw = new Drawing();

                    var width = Mathf.Max(100.0f, GUI.skin.button.CalcSize(new GUIContent(ss.name)).x,
                                    ss.CalcSize(inactiveText).x + ss.CalcSize(activeText).x) + 16.0f;

                    var height = 60.0f;

                    if (x + width > position.width - 32 && x > 5.0f)
                    {
                        x = 5.0f;
                        y += height + 10.0f;
                    }

                    draw.Rect = new Rect(x, y, width, height);

                    width -= 8.0f;

                    draw.Draw = () =>
                    {
                        if (GUILayout.Button(thisStyle.name, GUILayout.Width(width)))
                            CopyText("(GUIStyle)\"" + thisStyle.name + "\"");

                        GUILayout.BeginHorizontal();
                        GUILayout.Toggle(false, inactiveText, thisStyle, GUILayout.Width(width / 2));
                        GUILayout.Toggle(false, activeText, thisStyle, GUILayout.Width(width / 2));
                        GUILayout.EndHorizontal();
                    };

                    x += width + 18.0f;

                    Drawings.Add(draw);
                }
            }
            else if (_showingIcons)
            {
                if (_objects == null)
                {
                    _objects = new List<Object>(Resources.FindObjectsOfTypeAll(typeof(Texture)));
                    _objects.Sort((pA, pB) => string.Compare(pA.name, pB.name, StringComparison.OrdinalIgnoreCase));
                }

                var rowHeight = 0.0f;

                foreach (var oo in _objects)
                {
                    var texture = (Texture)oo;

                    if (texture.name == "")
                        continue;

                    if (lowerSearch != "" && !texture.name.ToLower().Contains(lowerSearch))
                        continue;

                    var draw = new Drawing();

                    var width = Mathf.Max(GUI.skin.button.CalcSize(new GUIContent(texture.name)).x, texture.width) +
                                8.0f;

                    var height = texture.height + GUI.skin.button.CalcSize(new GUIContent(texture.name)).y + 8.0f;

                    if (x + width > position.width - 32.0f)
                    {
                        x = 5.0f;
                        y += rowHeight + 8.0f;
                        rowHeight = 0.0f;
                    }

                    draw.Rect = new Rect(x, y, width, height);

                    rowHeight = Mathf.Max(rowHeight, height);

                    width -= 8.0f;

                    draw.Draw = () =>
                    {
                        if (GUILayout.Button(texture.name, GUILayout.Width(width)))
                            CopyText("EditorGUIUtility.FindTexture( \"" + texture.name + "\" )");

                        var textureRect = GUILayoutUtility.GetRect(texture.width, texture.width, texture.height,
                            texture.height, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
                        EditorGUI.DrawTextureTransparent(textureRect, texture);
                    };

                    x += width + 8.0f;

                    Drawings.Add(draw);
                }
            }

            _maxY = y;
        }

        var r = position;
        r.y = top;
        r.height -= r.y;
        r.x = r.width - 16;
        r.width = 16;

        var areaHeight = position.height - top;
        _scrollPos = GUI.VerticalScrollbar(r, _scrollPos, areaHeight, 0.0f, _maxY);

        var area = new Rect(0, top, position.width - 16.0f, areaHeight);
        GUILayout.BeginArea(area);

        var count = 0;
        foreach (var draw in Drawings)
        {
            var newRect = draw.Rect;
            newRect.y -= _scrollPos;

            if (newRect.y + newRect.height > 0 && newRect.y < areaHeight)
            {
                GUILayout.BeginArea(newRect, GUI.skin.textField);
                draw.Draw();
                GUILayout.EndArea();

                count++;
            }
        }

        GUILayout.EndArea();
    }

    private void CopyText(string pText)
    {
        var editor = new TextEditor();

        //editor.content = new GUIContent(pText); // Unity 4.x code
        editor.text = pText; // Unity 5.x code

        editor.SelectAll();
        editor.Copy();
    }

    private struct Drawing
    {
        public Rect Rect;
        public Action Draw;
    }
}