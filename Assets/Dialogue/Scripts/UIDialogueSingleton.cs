#if LOBODESTROYO
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/UIDialogueSingleton")]
    public class UIDialogueSingleton : ScriptableObject
    {
        [HideInInspector]
        public DialogueRootObject CurrentDialogue;
        public string CurrentDialogueName;

        [Header("Events")]
        public GameEvent DialogueSetEvent;
        public GameEvent BeginDialogueEvent;
        public GameEvent EndDialogueEvent;
        public GameEvent DialogueReleasedEvent;
        public GameEvent DialogueExecutedEvent;
        public GameEvent DialogueExhaustedEvent;

        private void OnEnable()
        { 
            CurrentDialogue = null;
            CurrentDialogueName = "";
        }

        public bool DialogueExhausted
        {
            get { return CurrentDialogue.Conversation.Exhausted; }
        }

        public void Use()
        {            
            CurrentDialogue.ExecuteCurrent();
            if (CurrentDialogue.Conversation.Exhausted)
                DialogueExhaustedEvent.Raise();
            DialogueExecutedEvent.Raise();
        }

        public void Set(DialogueRootObject dro)
        {
            CurrentDialogue = dro;
            CurrentDialogueName = CurrentDialogue.name;
            DialogueSetEvent.Raise();
        }

        public void Begin()
        {
            BeginDialogueEvent.Raise();
        }

        public void End()
        {
            EndDialogueEvent.Raise();
        }

        public void Release()
        {
            CurrentDialogue = null;
            CurrentDialogueName = "none";
            DialogueReleasedEvent.Raise();
        }

        public void PrintInfo(string info)
        {
            Debug.Log(info);
        }
    }
}
#endif