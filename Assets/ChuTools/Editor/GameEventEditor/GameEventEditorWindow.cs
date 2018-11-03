using System;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChuTools.Extensions;
using ChuTools.Scripts;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = System.Object;

namespace ChuTools.GameEventEditor
{
    /// <inheritdoc />
    /// <summary>
    ///     The following Editor Window will display all GameEvent Objects
    ///     and allow the use to manually invoke those events
    ///     It also shows all active listeners
    /// </summary>
    public class GameEventEditorWindow : EditorWindow
    {
        private List<UnityEngine.Object> _gameEvents = new List<UnityEngine.Object>();

        [MenuItem("Tools/GameEventEditorWindow")]
        private static void Init()
        {
            var w = GetWindow(typeof(GameEventEditorWindow));
            w.Show();
        }

        private void OnEnable()
        {
            _gameEvents = Chutilities.GetAssetsOfType(_search);
            _searchField = new SearchField();

        }

        private void OnFocus()
        {
            _gameEvents = Chutilities.GetAssetsOfType(_search);
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private SearchField _searchField;
        private string _search = "GameEvent";
        private Vector2 _scrollPos;
        private void OnGUI()
        {

            var searchRect = new Rect(0, 0, Screen.width / 2.0f, 25);

            GUILayout.BeginArea(searchRect);
            
            _search = _searchField.OnToolbarGUI(searchRect, _search);
            
            GUILayout.EndArea();
            var contentRect = new Rect(0, searchRect.yMax, Screen.width, Screen.height - searchRect.height - 5);
            GUILayout.BeginArea(contentRect);
            EditorGUILayout.LabelField(_search);
            DrawObjectsView();
            GUILayout.EndArea();
            if (GUI.changed)
                Repaint();
        }

        private void DrawObjectsView()
        {
            EditorGUILayout.BeginVertical();

            foreach (var Event in _gameEvents)
            {
                EditorGUILayout.BeginHorizontal();
                var field = Event.GetType().GetField("listeners", BindingFlags.NonPublic | BindingFlags.Instance);

                var listeners = field?.GetValue(Event) as List<MonoBehaviour>;
                var gos = listeners?.Select(x => x.gameObject).ToList();
                EditorGUILayout.ObjectField(Event, typeof(Object), false, GUILayout.Width(250));
                var methodInfo = Event.GetType().GetMethod("Raise");
                if (methodInfo != null)
                {
                    if (GUILayout.Button("Raise", GUILayout.Width(250)))
                    {

                        methodInfo?.Invoke(Event, null);
                    }
                }
                
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;
                gos?.ForEach(go => EditorGUILayout.ObjectField(go, typeof(GameObject), false, GUILayout.Width(250)));
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
        }
    }
}