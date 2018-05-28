using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Data", menuName = "Dialogue/Dialogue Root", order = 1)]
public class DialogueRootObject : ScriptableObject
{
    public Dialogue.DialogueRoot Conversation;
    private void OnEnable()
    {
        if(Conversation == null)
            Init();
    }

    public void Init()
    {

        Conversation = new Dialogue.DialogueRoot();
        Conversation.DialogueNodes = new List<Dialogue.DialogueNode>();
        var node = new Dialogue.DialogueNode();
        node.ConversationID = "CON::WORLD::NODE";
        node.ParticipantName = "Name";
        node.Line = "Thing to say";
        node.Side = "Left";
        node.EmoteType = "Reg";
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


