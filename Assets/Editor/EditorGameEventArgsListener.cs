using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(GameEventArgsListener))]
public class EditorGameEventArgsListener : Editor
{
    protected virtual void OnEnable()
    {
        List =
            new ReorderableList(serializedObject, serializedObject.FindProperty("Responses"), true, true, true, true)
            {
                drawElementCallback = DrawElementCallback,
                elementHeightCallback = ElementHeightCallback,
                drawHeaderCallback = DrawHeaderCallback
            };
    }

    private void DrawHeaderCallback(Rect rect)
    {
        EditorGUI.LabelField(rect, "Callbacks");
    }

    private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
    {
        var element = List.serializedProperty.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 50, EditorGUIUtility.singleLineHeight), element,
            GUIContent.none);
    }

    private float ElementHeightCallback(int index)
    {
        var element = List.serializedProperty.GetArrayElementAtIndex(index);
        var elementHeight = EditorGUI.GetPropertyHeight(element);
        // optional, depending on the situation in question and the defaults you like
        // you may want to subtract the margin out in the drawElementCallback before drawing
        var margin = EditorGUIUtility.standardVerticalSpacing + 15;
        return elementHeight + margin;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.Space();

        var prop = serializedObject.FindProperty("GameEvent");
        var objref = prop?.objectReferenceValue;
        // ReSharper disable once Unity.NoNullPropogation
        var method = objref?.GetType().GetMethod("Raise");

        var obj = EditorGUILayout.ObjectField(GameEventRef, typeof(GameEventArgs), false);
        Field.SetValue(target, obj);
        Label(Field?.GetValue(target)?.GetType().GetMethod("Raise")?.ToString());

        if(GUILayout.Button("Raise"))
        {
            //    Raisemethod.Invoke(gameevent, new object[] { null });
        }

        EditorGUILayout.LabelField("Field: " + Field?.Name);

        List.DoLayoutList();

        if(EditorGUI.EndChangeCheck())
        {
            Field.SetValue(target, GameEventRef as GameEventArgs);
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
            EditorUtility.SetDirty(serializedObject.targetObject);
        }
    }

    public static void Label(string value)
    {
        EditorGUILayout.LabelField(value);
    }

    public Object GameEventRef;
    public ReorderableList List;
    public FieldInfo Field => target.GetType().GetField("GameEvent");
    public static MethodInfo Raisemethod => typeof(GameEventArgs).GetMethod("Raise");//some function
}