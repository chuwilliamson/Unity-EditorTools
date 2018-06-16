using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEditor.EventSystems;
using UnityEngine;

[CustomEditor(typeof(CallbackBehaviour))]
public class EditorCallbackBehaviour : Editor
{
    private void OnEnable()
    {
        Type = target.GetType();
        Fields = Type.GetFields();
        Interfaces = Type.GetInterfaces();
        Properties = Type.GetProperties();
        _foldouts = new bool[GetType().GetFields().Length];
        _fields = GetType().GetFields();
    }

    public static void DrawArray(ICollection array)
    {
        EditorGUILayout.Space();
        
        if (array == null || array.Count < 1)
        {
            EditorGUILayout.LabelField(new GUIContent("no members"), EditorStyles.helpBox);
            return;
        }

        foreach (var a in array)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField(a.ToString(), EditorStyles.miniLabel);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
    }

    public void DrawLine()
    {
        EditorGUILayout.Space();
        var lr = GUILayoutUtility.GetLastRect();
        lr.position = new Vector2(lr.position.x, lr.position.y + lr.height);
        
        Handles.DrawLine(lr.position, new Vector3(lr.position.x + Screen.width, lr.position.y, 0));
        EditorGUILayout.Space();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField(GetType().ToString(), EditorStyles.boldLabel);
        DrawArray(_fields);
        DrawLine();
        EditorGUILayout.LabelField(Type.Name, EditorStyles.boldLabel);
        for (var count = 0; count < _fields.Length; count++)
        {
            var array = _fields[count];
            _foldouts[count] = EditorGUILayout.Foldout(_foldouts[count], array.Name);
            if (!_foldouts[count])
                continue;

            EditorGUI.indentLevel++;
            DrawArray(array.GetValue(this) as ICollection);
            EditorGUI.indentLevel--;
        }

        DrawLine();
    }

    public Type Type;
    public FieldInfo[] Fields;
    private FieldInfo[] _fields;
    public PropertyInfo[] Properties;
    public Type[] Interfaces;
    public MethodInfo[] MethodInfos;
    private bool[] _foldouts;
}