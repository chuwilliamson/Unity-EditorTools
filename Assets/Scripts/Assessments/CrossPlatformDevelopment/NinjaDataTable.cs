namespace Assessments.CrossPlatformDevelopment
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(PlayerData))]
    public class PlayerDataPropertyDrawer : UnityEditor.PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + EditorStyles.objectField.CalcSize(label).y + EditorGUIUtility.standardVerticalSpacing;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var objectDimensions = EditorStyles.objectField.CalcSize(label);
            position.height -= objectDimensions.y + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(position, property, label);
            position.y += objectDimensions.y + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.BeginChangeCheck();
            var so = new SerializedObject(property.objectReferenceValue);
            EditorGUI.Slider(position, so.FindProperty("Value"), 0, 20);
            if (EditorGUI.EndChangeCheck())
            {
                so.ApplyModifiedProperties();
                so.Update();
            }
        }
    }
}