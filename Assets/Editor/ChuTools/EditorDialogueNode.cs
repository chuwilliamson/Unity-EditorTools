using System;
using UnityEditor;
using UnityEngine;
namespace Dialogue
{
    public class EditorDialogueNode
    {
        private DialogueRootObject Target { get; set; }

        public EditorDialogueNode()
        {
            Target = ScriptableObject.CreateInstance<DialogueRootObject>();
        }
 
        public void Draw()
        {
            if (Target == null)
                return;
            if (Target.Conversation.Count <= 0)
            {
                if (!GUILayout.Button("Add Line", GUILayout.Width(Screen.width / 2.0f))) return;

                var node = new DialogueNode
                {
                    ConversationID = "",
                    ParticipantName = "Name...",
                    Side = "Right",
                    EmoteType = "Reg",
                    Line = "Thing to Say"
                };

                Target.Conversation.Add(node);
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                for (var i = 0; i < Target.Conversation.Count; i++)
                {
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(string.Format("Line {0}", i));
                    if(Target.Conversation.Count > 1)
                        if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false)))
                            Target.Conversation.Remove(Target.Conversation[i]);
                    GUILayout.EndHorizontal();
                    Target.Conversation[i].ParticipantName = EditorGUILayout.TextField("Who is talking?", Target.Conversation[i].ParticipantName);
                    Target.Conversation[i].Side = EditorGUILayout.EnumPopup("Which Side?", (Side)Enum.Parse(typeof(Side), Target.Conversation[i].Side)).ToString();
                    Target.Conversation[i].Line = EditorGUILayout.TextField("What to say..", Target.Conversation[i].Line);
                    Target.Conversation[i].EmoteType = EditorGUILayout.EnumPopup("How are you feeling?", (EmoteType)Enum.Parse(typeof(EmoteType), Target.Conversation[i].EmoteType)).ToString();
                }
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(Target);
                }
            }
        }
    }

}