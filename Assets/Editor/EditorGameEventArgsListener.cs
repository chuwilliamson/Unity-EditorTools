using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(GameEventArgsListener))]
public class EditorGameEventArgsListener : Editor
{
    public FieldInfo Field;
    public Object GameEventRef;
    public ReorderableList list;
    public static  MethodInfo raisemethod => typeof(GameEventArgs).GetMethod("Raise");//some function
    protected virtual void OnEnable()
    {
        Field = target.GetType().GetField("GameEvent");
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyPath: "Responses"), true,
            true, true, true)
        {
            drawElementCallback = DrawElementCallback,
            elementHeightCallback = ElementHeightCallback,
            drawHeaderCallback = DrawHeaderCallback
        };
    }

    private void DrawHeaderCallback(Rect rect)
    {
        EditorGUI.LabelField(rect, label: "Callbacks");

    }

    private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
    {
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 50,
                EditorGUIUtility.singleLineHeight),
            element,
            GUIContent.none);
    }

    private float ElementHeightCallback(int index)
    {
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        var elementHeight = EditorGUI.GetPropertyHeight(element);
        // optional, depending on the situation in question and the defaults you like
        // you may want to subtract the margin out in the drawElementCallback before drawing
        var margin = EditorGUIUtility.standardVerticalSpacing + 15;
        return elementHeight + margin;
    }

    public override void OnInspectorGUI()
    {
        Field = target.GetType().GetField("GameEvent");
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("GameEvent"), true);
        var method = serializedObject.FindProperty("GameEvent").objectReferenceValue.GetType().GetMethod("Raise");

        Label(serializedObject.FindProperty("GameEvent").objectReferenceValue.GetType().GetMethod("Raise").ToString());       
        
        var gameevent = serializedObject.FindProperty("GameEvent").objectReferenceValue; //the gameevent
        
        if (GUILayout.Button("Raise"))
        {
           raisemethod.Invoke(gameevent, new object[] { null });
        }

        EditorGUILayout.LabelField("Field: " + Field.Name ?? "null");

        list.DoLayoutList();

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }


    }
    public static void Label(string value)
    {
        EditorGUILayout.LabelField(value);
    }

    public System.Collections.Generic.List<string> FizzBuzz(int n)
    {
        var listofstrings = new List<string> { "1", "2", "Fizz", "3", "4", "Buzz" };        

        for(int i = 0; i < n; i++)
        {
            if (i % 3 == 0)
                listofstrings.Add("fizz");
        }

        return listofstrings;
    }
}
 