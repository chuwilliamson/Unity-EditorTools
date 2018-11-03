using ChuTools.Attributes;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ChuTools.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ScriptVariableAttribute))]
    public class ScriptVariableDrawer : PropertyDrawer
    {
        public ScriptVariableAttribute ScriptVariableAttribute => (ScriptVariableAttribute)attribute;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + GUI.skin.button.CalcSize(label).y +
                   EditorGUIUtility.standardVerticalSpacing;
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var vars = ScriptVariableAttribute.Vars;
            var selected = vars.ToList().FindIndex(stat => property.objectReferenceValue == stat);
            var contents = vars.Select(n => new GUIContent(n.name)).ToArray();
            var bottomContent = new GUIContent("Field: " + property.name);

            position.height -= GUI.skin.button.CalcSize(bottomContent).y + EditorGUIUtility.standardVerticalSpacing;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, new GUIContent("Type: " + label.text));
            GUI.enabled = true;
            EditorGUI.BeginChangeCheck();
            position.y += EditorStyles.objectField.CalcSize(label).y + EditorGUIUtility.standardVerticalSpacing;

            selected = EditorGUI.Popup(position, bottomContent, selected, contents);

            if (EditorGUI.EndChangeCheck())
            {
                property.objectReferenceValue = vars[selected];
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
            }
        }
    }
}