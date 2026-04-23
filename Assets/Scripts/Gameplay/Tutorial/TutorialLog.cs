using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

namespace RM_EDU
{
    // The tutorial log, which allows the player to view previous tutorials.
    public class TutorialLog : MonoBehaviour
    {
        // The gameplay UI.
        public GameplayUI gameUI;

        // The current tutorial index.
        protected int currTutorialInfoIndex = 0;

        // The tutorial infos.
        protected List<Tutorials.TutorialInfo> tutorialInfos = new List<Tutorials.TutorialInfo>();

        // The current tutorial info.
        protected Tutorials.TutorialInfo currTutorialInfo = null;

        [Header("Tutorials")]

        // The tutorial log entry buttons.
        public List<TutorialLogEntryButton> tutorialLogEntryButtons = new List<TutorialLogEntryButton>();

        // The previous tutorials page button.
        public Button prevTutorialsPageButton;

        // The next tutorial pages button.
        public Button nextTutorialsPageButton;

        [Header("Selected Tutorial Section")]

        // The tutorial title.
        public TMP_Text tutorialTitle;

        // The tutorial image display.
        public GameObject tutorialImageDisplay;

        // The tutorial image.
        public Image tutorialImage;

        // The tutorial text.
        public TMP_Text tutorialText;

        // The tutorial page index.
        protected int currTutorialInfoPageIndex = 0;

        // The previous tutorial text page.
        public Button prevTutorialTextPageButton;

        // The next tutorial text page.
        public Button nextTutorialTextPageButton;

        // The tutorial page text.
        public TMP_Text tutorialPageText;


        // Start is called before the first frame update
        void Start()
        {
            // If the game UI isn't set, set it.
            if (gameUI == null)
                gameUI = FindObjectOfType<GameplayUI>();
        }

        // CURRENT TUTORIAL INFO
        // Returns 'true' if the tutorial info is set.
        public bool IsCurrentTutorialInfoSet()
        {
            return currTutorialInfo != null;
        }

        // Sets the tutorial info.
        public void SetCurrentTutorialInfo(int newTutorialInfoIndex)
        {
            // Clamp the tutorial info index.
            currTutorialInfoIndex = Mathf.Clamp(newTutorialInfoIndex, 0, tutorialInfos.Count - 1);

            // Index is valid, so get the tutorials info.
            if(currTutorialInfoIndex >= 0 && currTutorialInfoIndex < tutorialInfos.Count)
            {
                currTutorialInfo = tutorialInfos[currTutorialInfoIndex];
            }
            // Index is invalid, so set to null.
            else
            {
                currTutorialInfo = null;
            }

            // Sets the page index to 0.
            currTutorialInfoPageIndex = 0;

            // Updates the current tutorial info.
            UpdateCurrentTutorialInfo();
        }

        // Set the current tutorial info using info.
        public void SetCurrentTutorialInfo(Tutorials.TutorialInfo newInfo)
        {
            // Sets the info.
            currTutorialInfo = newInfo;

            // If the current info isn't null, update the info.
            if (currTutorialInfo != null)
            {
                UpdateCurrentTutorialInfo();
            }
            // Info is now null, so clear the current info.
            else
            {
                ClearCurrentTutorialInfo();
            }
        }

        // Set the current tutorial info using an entry button.
        public void SetCurrentTutorialInfo(TutorialLogEntryButton entryButton)
        {
            SetCurrentTutorialInfo(entryButton.entryInfo);
        }

        // Goes to the previous tutorial info.
        public void PreviousTutorialInfo()
        {
            // Reduce index.
            currTutorialInfoIndex--;

            // Bounds check.
            if (currTutorialInfoIndex < 0)
            {
                currTutorialInfoIndex = tutorialInfos.Count - 1;
            }

            // Sets the tutorial.
            SetCurrentTutorialInfo(currTutorialInfoIndex);
        }

        // Goes to the next tutorial info.
        public void NextTutorialInfo()
        {
            // Increase index.
            currTutorialInfoIndex++;

            // Bounds check.
            if(currTutorialInfoIndex >= tutorialInfos.Count)
            {
                currTutorialInfoIndex = 0;
            }

            // Sets the tutorial.
            SetCurrentTutorialInfo(currTutorialInfoIndex);
        }

        // CURRENT TUTORIAL INFO PAGE
        // Sets the tutorial info page.
        public void SetCurrentTutorialInfoPage(int newPageIndex)
        {
            // Sets the current page index if the current tutorial info is set.
            if(currTutorialInfo != null)
            {
                currTutorialInfoPageIndex = Mathf.Clamp(newPageIndex, 0, currTutorialInfo.pages.Count - 1);
            }
            // Set to 0 since the value is null.
            else
            {
                currTutorialInfoPageIndex = 0;
            }

            // Updates the current tutorial info page.
            UpdateCurrentTutorialInfo();
        }

        // Goes to the previous tutorial info page.
        public void PreviousCurrentTutorialInfoPage()
        {
            // Reduce page index.
            currTutorialInfoPageIndex--;

            // If the current tutorial info is set.
            if (currTutorialInfo != null)
            {
                // Bounds check for going below 0.
                if (currTutorialInfoPageIndex < 0)
                {
                    currTutorialInfoPageIndex = currTutorialInfo.pages.Count - 1;
                }
            }
            // Not set.
            else
            {
                currTutorialInfoPageIndex = 0;
            }

            // Sets the tutorial page.
            SetCurrentTutorialInfoPage(currTutorialInfoPageIndex);
        }

        // Goes to the next tutorial info page.
        public void NextCurrentTutorialInfoPage()
        {
            // Increase page index.
            currTutorialInfoPageIndex++;

            // If the current tutorial info is set,
            if(currTutorialInfo != null)
            {
                // If the page is at or above the count, set it to null.
                if(currTutorialInfoPageIndex >= currTutorialInfo.pages.Count)
                {
                    currTutorialInfoPageIndex = 0;
                }
            }
            // Not set.
            else
            {
                currTutorialInfoPageIndex = 0;
            }

            // Sets the tutorial page.
            SetCurrentTutorialInfoPage(currTutorialInfoPageIndex);
        }

        // TUTORIAL INFO DISPLAY
        // Updates the tutorial info.
        public void UpdateCurrentTutorialInfo()
        {
            // No current info, so clear display and return.
            if(currTutorialInfo == null)
            {
                ClearCurrentTutorialInfo();
                return;
            }

            // If the page index is negative or above the page count, clear display and return. 
            if(currTutorialInfoPageIndex < 0 || currTutorialInfoPageIndex >= currTutorialInfo.pages.Count)
            {
                ClearCurrentTutorialInfo();
                return;
            }

            // Gets the page.
            EDU_Page page = (EDU_Page)currTutorialInfo.pages[currTutorialInfoPageIndex];

            // Set the text.
            tutorialTitle.text = page.title;
            tutorialText.text = page.text;
            tutorialPageText.text = (currTutorialInfoPageIndex + 1).ToString() + "/" + currTutorialInfo.pages.Count.ToString();

            // Set the display image.
            tutorialImageDisplay.gameObject.SetActive(true);
            tutorialImage.sprite = page.displaySprite;

            // If the tutorial image sprite is null, turn the object off.
            if(tutorialImage.sprite == null)
                tutorialImage.gameObject.SetActive(false);

            // If text-to-speech is usable and enabled, read the page text.
            if (page.textSpeakKey != "" && LOLManager.IsTextToSpeechUsableAndEnabled())
            {
                // Read the text.
                SpeakText(page.textSpeakKey, false);
            }

            // Set the text page buttons interactable if there's more than 1 page.
            bool multPages = currTutorialInfo.pages.Count > 1;
            prevTutorialTextPageButton.interactable = multPages;
            nextTutorialTextPageButton.interactable = multPages;
        }

        // Clears the tutorial info.
        public void ClearCurrentTutorialInfo()
        {
            // Set the current tutorial info to null and reset the indexes.
            currTutorialInfo = null;
            currTutorialInfoIndex = 0;
            currTutorialInfoPageIndex = 0;

            // Clear the text.
            tutorialTitle.text = "-";
            tutorialText.text = "-";
            tutorialPageText.text = "-";

            // Clear the info display image.
            tutorialImageDisplay.gameObject.SetActive(true);
            tutorialImage.sprite = null;
            tutorialImage.gameObject.SetActive(false);

            // Disable page buttons.
            prevTutorialTextPageButton.interactable = false;
            nextTutorialTextPageButton.interactable = false;

        }

        // TEXT-TO-SPEECH
        // Speaks text if Text-to-Speech is usable and enabled.
        // checkUsable: if 'true', this function checks if the TTS is usable.
        public void SpeakText(string key, bool checkTTSUsable = true)
        {
            // If TTS should be checked for it being usable.
            if(checkTTSUsable)
            {
                // If text-to-speech is usable and enabled, speak the text.
                if (key != "" && LOLManager.IsTextToSpeechUsableAndEnabled())
                {
                    LOLManager.Instance.SpeakText(key);
                }
            }
            // Don't check usable.
            else
            {
                LOLManager.Instance.SpeakText(key);
            }

        }

    }
}