#if LOBODESTROYO

using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dialogue
{
    public class UIDialogue : MonoBehaviour
    {
        #region fields
        [SerializeField]
        private UIDialogueSingleton _uiDialogue;

        [Header("Center")]
        public Image centerImage;
        public Text centerTextBot;
        public Text centerTextTop;


        [Header("Buttons")]
        public Button interactButton;
        public Button nextButton;
        [Header("Left")]
        public Image leftImage;
        public Text leftText;

        [Header("Right GUI")]
        public Image rightImage;
        public Text rightText;

        [Header("Sprites")]
        public Sprite[] textBubbleSprites;
        public Sprite[] conversationSprites;

        [Space]
        [SerializeField]
        private TeleType teleType;
        #endregion

        private void Start()
        {
            HideInteractButton();
            HideDialogueElements();
        }

        /// <summary>
        /// Show the Interactbutton and add click listener to begin the dialogue
        /// </summary>
        public void ShowInteractButton()
        {
            interactButton.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(interactButton.gameObject);
            interactButton.onClick.AddListener(_uiDialogue.Begin);
        }

        /// <summary>
        /// hide the interactbutton and remove the listener for beginning dialogue
        /// </summary>
        public void HideInteractButton()
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.RemoveListener(_uiDialogue.Begin);
        }

        #region Callbacks
        /// <summary>
        /// show the interactbutton and add the begin dialogue event to the onclick
        /// </summary>
        public void OnDialogueSet()
        {
            ShowInteractButton();
        }

        public void OnDialogueBegin()
        {
            HideInteractButton();
            ShowDialogueElements();
        }

        public void OnDialogueEnd()
        {
            ShowInteractButton();
            HideDialogueElements();
        }

        public void OnDialogueReleased()
        {
            HideInteractButton();
        }
        #endregion

        /// <summary>
        /// Go to the next line of dialogue then update
        /// </summary>
        private void NextDialogue()
        {
            //if the typer is typing then we just return to mock some input
            if (teleType.isTyping)
            {
                teleType.EndTeleType();
                return;
            }

            //if the dialogue is exhausted then end this conversation
            //dialogue will be exhausted when we reach the last element 
            if (_uiDialogue.DialogueExhausted)
            {
                _uiDialogue.End();
                return;
            }

            _uiDialogue.Use();
            UpdateDialogueElements(_uiDialogue.CurrentDialogue);
        }

        /// <summary>
        /// show the primary dialogue elements and add the listener for clicking the nextbutton
        /// once we show then update
        /// </summary>
        private void ShowDialogueElements()
        {
            leftImage.gameObject.SetActive(true);
            rightImage.gameObject.SetActive(true);

            centerTextTop.gameObject.SetActive(true);
            centerImage.gameObject.SetActive(true);

            nextButton.gameObject.SetActive(true);
            nextButton.onClick.AddListener(NextDialogue);
            EventSystem.current.SetSelectedGameObject(nextButton.gameObject);

            UpdateDialogueElements(_uiDialogue.CurrentDialogue);
        }

        /// <summary>
        /// hide the primary dialogue elements and remove listeners
        /// </summary>
        private void HideDialogueElements()
        {
            leftImage.gameObject.SetActive(false);
            rightImage.gameObject.SetActive(false);

            nextButton.onClick.RemoveListener(NextDialogue);
            nextButton.gameObject.SetActive(false);

            centerImage.gameObject.SetActive(false);
            centerTextTop.gameObject.SetActive(false);
        }

        /// <summary>
        /// update the hud
        /// </summary>
        /// <param name="d"></param>
        private void UpdateDialogueElements(DialogueRootObject d)
        {
            var talker = d.Conversation.Current.ParticipantName;
            var emote = d.Conversation.Current.EmoteType;
            var side = d.Conversation.Current.Side;

            // Please note that Enpi Seeki and all of the Nantooki Tribe NPCs will all use the same icon.    
            talker = talker.Contains("Enpi") ? "Nantooki" : talker;
            talker = talker.Contains(".") ? talker.Replace(".", string.Empty) : talker; // T.I.M.B.U.R.
            talker = talker.Replace(" ", string.Empty);


            if (side == "Left")
                SetSide(leftImage, leftText, talker, emote, side);
            else if (side == "Right")
                SetSide(rightImage, rightText, talker, emote, side);

            centerTextTop.text = d.Conversation.Current.Line;

            teleType.StartTeleType();
        }

        /// <summary>
        /// Do the setup for the participants talking, this function is gross but w/e
        /// </summary>
        /// <param name="img"></param>
        /// <param name="txt"></param>
        /// <param name="talker"></param>
        /// <param name="emote"></param>
        /// <param name="side"></param>
        private void SetSide(Image img, Text txt, string talker, string emote, string side)
        {
            leftImage.gameObject.SetActive(false);
            rightImage.gameObject.SetActive(false);
            leftText.gameObject.SetActive(false);
            rightText.gameObject.SetActive(false);
            img.gameObject.SetActive(true);
            txt.gameObject.SetActive(true);
            txt.text = _uiDialogue.CurrentDialogue.Conversation.Current.ParticipantName;
            img.sprite = conversationSprites.FirstOrDefault(
                x => x.name.Contains(talker) && x.name.Contains(emote));
            centerImage.sprite = textBubbleSprites.FirstOrDefault(x => x.name.Contains(side));
        }
    }
} 
#endif