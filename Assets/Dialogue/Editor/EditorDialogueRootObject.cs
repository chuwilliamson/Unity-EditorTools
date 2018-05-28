using UnityEngine;
using UnityEditor;
using System;
using Dialogue;
[CustomEditor(typeof(DialogueRootObject))]
public class EditorDialogueRootObject : Editor
{

    public override void OnInspectorGUI()
    {
        var mytarget = target as DialogueRootObject;
        if(mytarget.Conversation.Count > 0)
        {
            for(int i = 0; i < mytarget.Conversation.Count; i++)
            {
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("Line {0}", i));
                if(GUILayout.Button("Remove", GUILayout.ExpandWidth(false)))
                    mytarget.Conversation.Remove(mytarget.Conversation[i]);
                GUILayout.EndHorizontal();
                mytarget.Conversation[i].ParticipantName = EditorGUILayout.TextField("Who is talking?", mytarget.Conversation[i].ParticipantName as string);
                mytarget.Conversation[i].Side = EditorGUILayout.EnumPopup("Which Side?", (Side)Enum.Parse(typeof(Side), mytarget.Conversation[i].Side)).ToString();
                mytarget.Conversation[i].Line = EditorGUILayout.TextField("What to say..", mytarget.Conversation[i].Line as string);
                mytarget.Conversation[i].EmoteType = EditorGUILayout.EnumPopup("How are you feeling?", (EmoteType)Enum.Parse(typeof(EmoteType), mytarget.Conversation[i].EmoteType)).ToString();
            }

            if(GUILayout.Button("Add Line", GUILayout.Width(Screen.width / 2)))
            {
                var node = new DialogueNode();
                node.ConversationID = "";
                node.ParticipantName = "Name...";
                node.Side = "Right";
                node.EmoteType = "Reg";
                node.Line = "Thing to Say";                
                mytarget.Conversation.Add(node);
            }

            EditorUtility.SetDirty(mytarget);

        }
    }
}


