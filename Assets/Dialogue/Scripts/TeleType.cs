using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Assertions;
namespace Dialogue
{
    public class TeleType : MonoBehaviour
    {
        private string line = "Text still spans upto two Lines at a time. <color=red>Accentuated text</color> can be dropped to an alternate color like so. This will allow for many more characters per text 'chunk'. 96 characters to a row currently has nice spacing.";

        [SerializeField]
        private Text m_text;
        public float textSpeed = 1;
        private bool m_typing = false;

        public bool isTyping
        {
            get { return m_typing; }
        }
        
        void Awake()
        {
            // Get Reference to Text Component
            m_text = GetComponent<Text>();
          
        }
        string cache_line;
        public void StartTeleType()
        {
            line = m_text.text;
            cache_line = line;
            StartCoroutine(StartType());
        }

        public void EndTeleType()
        {
            m_text.text = cache_line;
            m_typing = false;
            StopAllCoroutines();         
      
        }

        private IEnumerator StartType()
        {
            int totalVisibleCharacters = line.Length; // Get # of Visible Character in text object
            int counter = 0;
            int visibleCount = 0;
            m_typing = true;
            while(true)
            {
                visibleCount = counter % (totalVisibleCharacters + 1);
                var visibleText = line.Substring(0, visibleCount);
                m_text.text = visibleText; // How many characters should TextMeshPro display?
            
                // Once the last character has been revealed, wait 1.0 second and start over.
                if(visibleCount >= totalVisibleCharacters)
                {
                    m_text.text = line;
                    m_typing = false;
                    yield break;
                }
                counter += 1;
                yield return new WaitForSeconds(0.05f / textSpeed);
            } 
        }
    }
}