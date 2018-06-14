using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

//use this to draw all the things
public interface IFunction
{
    void Call();
}

public class FunctionObject : IFunction
{
    public void Call()
    {
        Debug.Log("call func");
    }
}

public class CallbackBehaviour : MonoBehaviour
{
    public IFunction Function { get; set; }
}

[CustomEditor(typeof(CallbackBehaviour))]
public class EditorCallbackBehaviour : Editor
{
    private void OnEnable()
    {
        Type = target.GetType();
        Fields = Type.GetFields();
        Interfaces = Type.GetInterfaces();
        Properties = Type.GetProperties();
        Foldouts = new bool[GetType().GetFields().Length];
    }

    public static void DrawArray(ICollection array)
    {
        if (array == null)
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
    }

    public override void OnInspectorGUI()
    {

        EditorGUILayout.LabelField(GetType().ToString());
        DrawArray(GetType().GetFields());

        var lr = GUILayoutUtility.GetLastRect();
        lr.position = new Vector2(lr.position.x, lr.position.y + lr.height);
        Handles.DrawLine(lr.position, new Vector3(lr.position.x + Screen.width, lr.position.y, 0));
        EditorGUILayout.LabelField(Type.Name);
        var count = 0;
        foreach (var array in GetType().GetFields())
        {
            Foldouts[count] = EditorGUILayout.Foldout(Foldouts[count], array.Name);

            EditorGUI.indentLevel++;
            if (Foldouts[count])
            {
                var obj = array.GetValue(this);
                DrawArray(obj as ICollection);
            }
            EditorGUI.indentLevel--;
            count++;
        }
    }

    

    public Type Type;
    public FieldInfo[] Fields;
    public PropertyInfo[] Properties;
    public Type[] Interfaces;
    public MethodInfo[] MethodInfos;
    
    public bool[] Foldouts;
}