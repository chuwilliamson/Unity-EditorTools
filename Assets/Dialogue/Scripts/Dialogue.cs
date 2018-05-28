using System.Collections.Generic;
using System.Xml.Serialization;

/*
 *NAMING CONVENTIONS
 *Conversations are tagged with the prefix CON. Cutscenes/cinematics are given CUT prefixes.
 *The number following the prefix is the world in which this conversation takes place.  
 *Example: CON01-000A takes place in WikiTiki Woods (World 1).
 *The three digit number following the prefix and world indicator is the ID number for the 
 *conversation. The letter following it is used if there are multiple interconnected
 *conversations. A is the default and B,C,D... are added as needed. 
 */
namespace Dialogue
{
    [XmlRoot(ElementName = "DialogueTree")]
    [System.Serializable]
    public class DialogueTree
    {
        [XmlElement(ElementName = "DialogueRoot")]
        public List<DialogueRoot> DialogueRoots
        {
            get;
            set;
        }
    }

    [XmlRoot(ElementName = "DialogueRoot")]
    [System.Serializable]
    public class DialogueRoot
    {
        private int _index = 0;

        public DialogueNode this[int key]
        {
            get { return DialogueNodes[key]; }    
        }

        public int Count
        {
            get { return DialogueNodes.Count; }
        }

        public DialogueRoot()
        {
            _index = 0;
            DialogueNodes = new List<DialogueNode>();
        }

        public DialogueRoot(List<DialogueNode> dialogueNodes)
        {
            _index = 0;
            DialogueNodes = dialogueNodes;
        }

        [XmlElement(ElementName = "DialogueNode")]
        public List<DialogueNode> DialogueNodes;
        
             
        public void Reset()
        {
            _index = 0;
        }
        
        public DialogueNode Current
        {
            get { return DialogueNodes[_index]; }
            
        }

        public bool Exhausted
        {
            get { return _index == DialogueNodes.Count - 1; }
        }

        public void NextNode()
        {
            int newindex = _index + 1;
            if(newindex >= DialogueNodes.Count - 1)
                newindex = DialogueNodes.Count - 1;

            _index = newindex;
        }

        public DialogueNode Next
        {
            get
            {
                int newindex = _index + 1;
                if(newindex >= DialogueNodes.Count - 1)
                    newindex = DialogueNodes.Count - 1;
                
                return DialogueNodes[newindex];
            }
        }

        public void RemoveAt(int index)
        {
            DialogueNodes.RemoveAt(index);
        }

        public void Remove(DialogueNode node)
        {
            DialogueNodes.Remove(node);
        }

        public void Add(DialogueNode node)
        {
            DialogueNodes.Add(node);
        }


    }

    [XmlRoot(ElementName = "DialogueNode")]
    [System.Serializable]
    public class DialogueNode
    {
       
        public override string ToString()
        {
             return string.Format("{0} :: {1}", ParticipantName, Line); 
        }

        public DialogueNode()
        {
            _current = this;
            ConversationID = "";
            ParticipantName = "";
            EmoteType = "";
            Side = "";
            Line = "";
            SpecialityAnimation = "";
            SpecialtyCamera = "";
            Participants = "";
            ConversationSummary = "";
        }

        DialogueNode(string id, string p, string e, string side, string line) : this()
        {
            _current = this;
            ConversationID = id;
            ParticipantName = p;
            EmoteType = e;
            Side = side;
            Line = line;
            SpecialityAnimation = "";
            SpecialtyCamera = "";
            Participants = "0";
            ConversationSummary = "";
        }
        private DialogueNode _current;
        public DialogueNode Next
        {
            get;
            set;
        }

        [XmlElement(ElementName = "ConversationID")]
        public string ConversationID;
        [XmlElement(ElementName = "ParticipantName")]
        public string ParticipantName;
        [XmlElement(ElementName = "EmoteType")]
        public string EmoteType;
        [XmlElement(ElementName = "Side")]
        public string Side;
        [XmlElement(ElementName = "Line")]
        public string Line;
        [XmlElement(ElementName = "SpecialityAnimation")]
        public string SpecialityAnimation;
        [XmlElement(ElementName = "SpecialtyCamera")]
        public string SpecialtyCamera;
        [XmlElement(ElementName = "Participants")]
        public string Participants;
        [XmlElement(ElementName = "ConversationSummary")]
        public string ConversationSummary;
    }
}
