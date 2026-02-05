using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // An element used for the knowledge stage.
    public abstract class KnowledgeElement : MonoBehaviour
    {
        // The knowledge UI.
        public KnowledgeUI knowledgeUI;

        // The button.
        public Button button;

        // The button image.
        public Image buttonImage;

        // A copy of the button's color block on start.
        [HideInInspector]
        public ColorBlock buttonColorsCopy;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the knowledge UI is null, get the instance.
            if (knowledgeUI == null)
                knowledgeUI = KnowledgeUI.Instance;

            // Adds a Select() call on the button.
            if (button != null)
            {
                // If the button image isn't set, get it from the button.
                if (buttonImage == null)
                    buttonImage = button.gameObject.GetComponent<Image>();

                // Listener for the tutorial toggle.
                button.onClick.AddListener(delegate
                {
                    Select();
                });

                // Saves the color block.
                SetButtonColorsCopyToCurrent();
            }
        }

        // The select function for the button.
        public abstract void Select();

        // Sets the button's color copy to the current values.
        public void SetButtonColorsCopyToCurrent()
        {
            buttonColorsCopy = button.colors;
        }

        // Sets the button to the normal color.
        public void SetButtonToNormalColor()
        {
            ColorBlock cb = button.colors;
            cb.normalColor = buttonColorsCopy.normalColor;
            button.colors = cb;
        }

        // Sets the button to the selected color.
        public void SetButtonToSelectedColor()
        {
            ColorBlock cb = button.colors;
            cb.normalColor = buttonColorsCopy.selectedColor;
            button.colors = cb;
        }

        // Returns 'true' if text-to-speech is enabled.
        public bool IsTextToSpeechEnabled()
        {
            // Checks if tts is available and enabled.
            bool ttsAvail;
            bool ttsEnabled;

            // If the LOL Manager is initialized and text-to-speech is instantiated...
            // The TTS is available.
            if(LOLManager.IsLOLSDKInitialized() && TextToSpeech.Instantiated)
            {
                ttsAvail = true;
            }
            else
            {
                ttsAvail = false;
            }

            // If the game settings is instantiated, check if TTS is enabled.
            if (GameSettings.Instantiated)
            {
                ttsEnabled = GameSettings.Instance.UseTextToSpeech;
            }
            else
            {
                ttsEnabled = false;
            }

            // The result to return.
            bool result = ttsAvail && ttsEnabled;

            // Returns 'true' if TTS is enabled.
            return result;
        }

        // Read the text for TTS. Only does so if text to speech is enabled.
        public abstract void SpeakText();

        // Update is called once per frame
        protected virtual void Update()
        {
            // ...
        }
    }
}