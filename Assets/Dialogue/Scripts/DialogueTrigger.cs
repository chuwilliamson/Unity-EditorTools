#if LOBODESTROYO
using UnityEngine;

namespace Dialogue
{
    /// <summary>
    ///     put this trigger as a child of an npc to initialize dialogue
    /// </summary>
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject _dialogueCamera;

        /// <summary>
        ///assigned in inspector for this conversation
        /// </summary>        
        [SerializeField]
        public DialogueRootObject _dialogueConfig;

        [SerializeField]
        private UIDialogueSingleton _UIDialogue;

        private bool _uiActive = true;
        public GameObject _convoBubble;

        /// <summary>
        ///     create the resource from the dialogue configuration.
        /// </summary>
        /// 
        private void Start()
        {
            _dialogueConfig = Instantiate(_dialogueConfig);
            _dialogueCamera.gameObject.SetActive(false);
            _convoBubble.SetActive(false);
            _uiActive = false;
        }

        public void OnEnterSensor()
        {
            if (_uiActive)
                return;
            _uiActive = true;
            _convoBubble.SetActive(true);

            //Debug.Log(@"<color=green>makecurrent</color>");
            _UIDialogue.Set(_dialogueConfig);
        }

        public void OnExitSensor()
        {
            if (!_uiActive)
                return;
            _convoBubble.SetActive(false);
            //_UIDialogue.HideInteractButton();
            // _UIDialogue.End();
            _uiActive = false;
            _UIDialogue.Release();
        }

        public void OnDialogueBegin()
        {
            if (_UIDialogue.CurrentDialogue != _dialogueConfig)
                return;
            _dialogueCamera.SetActive(true);
        }

        public void OnDialogueEnd()
        {
            if (_UIDialogue.CurrentDialogue != _dialogueConfig)
                return;
            _dialogueCamera.SetActive(false);
        }
    }
} 
#endif