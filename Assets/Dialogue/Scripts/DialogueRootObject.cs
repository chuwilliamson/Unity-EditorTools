using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Data", menuName = "Dialogue/Dialogue Root", order = 1)]
    public class DialogueRootObject : ScriptableObject
    {
        public DialogueRoot Conversation;

        private void OnEnable()
        {
            if (Conversation == null)
                Init();
        }

        public void Init()
        {
            Conversation = new DialogueRoot {DialogueNodes = new List<DialogueNode>()};
            var node = new DialogueNode
            {
                ConversationID = "CON::WORLD::NODE",
                ParticipantName = "Name",
                Line = "Thing to say",
                Side = "Left",
                EmoteType = "Reg"
            };
            Conversation.DialogueNodes.Add(node);
        }

        [ContextMenu("Reset Index")]
        public void ResetIndex()
        {
            Conversation.Reset();
        }

        public void ExecuteCurrent()
        {
            Conversation.NextNode();
        }
    }
}