#if LOBODESTROYO
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
namespace Dialogue
{
    [CustomEditor(typeof(Dialogue.DialogueTrigger))]
    public class DialogueTriggerEditor : Editor
    {
        protected static bool showLine = true; //declare outside of function
        private AnimBool showFields;

        private GUIStyle style;


        private void OnEnable()
        {
            showFields = new AnimBool(true);
            showFields.valueChanged.AddListener(Repaint);
        }

        public override void OnInspectorGUI()
        {
            EditorUtility.SetDirty(target);

            style = new GUIStyle("HelpBox") { richText = true };
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            var mytarget = target as Dialogue.DialogueTrigger;

            if (mytarget == null)
                return;

            var currentDialogue = mytarget._dialogueConfig;

            if (currentDialogue == null)
                return;


            showFields.target = EditorGUILayout.Foldout(showFields.target, "Show Dialogue");
            if (EditorGUILayout.BeginFadeGroup(showFields.faded))
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField("Conversation Exhausted", currentDialogue.Conversation.Exhausted.ToString(),
                    style);
                EditorGUILayout.LabelField("Participant Name", currentDialogue.Conversation.Current.ParticipantName, style);
                EditorGUILayout.LabelField("Side", currentDialogue.Conversation.Current.Side, style);
                GUILayout.BeginVertical();
                EditorGUILayout.LabelField("Line", currentDialogue.Conversation.Current.Line, style);
                GUILayout.EndVertical();
                EditorGUILayout.LabelField("Emote", currentDialogue.Conversation.Current.EmoteType, style);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }
    }
}
#endif