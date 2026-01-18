using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_EDU
{
    // The licenses dialog.
    public class Licenses : MonoBehaviour
    {
        // The title UI.
        public TitleUI titleUI;

        // The credits interface.
        public AudioCreditsInterface creditsInterface;

        // The BGM audio credits for the game.
        public AudioCredits bgmCredits;

        // The BGM credits page.
        public int bgmCreditsIndex = 0;

        // The SFX audio credits for the game.
        public AudioCredits sfxCredits;

        // The sound effects credit page.
        public int sfxCreditsIndex = 0;

        [Header("UI")]

        // BGM Button
        public Button bgmButton;

        // SFX Button
        public Button sfxButton;

        // Start is called before the first frame update
        void Start()
        {
            // Loads all the credits.
            LoadBackgroundMusicCredits();
            LoadSoundEffectCredits();

            // Sets this if it has not been set already.
            if (creditsInterface == null)
                creditsInterface = GetComponent<AudioCreditsInterface>();

            // Show the background credits.
            ShowBackgroundMusicCredits();
        }

        // Loads the BGM credits.
        private void LoadBackgroundMusicCredits()
        {
            AudioCredits.AudioCredit credit;

            // Title
            // TODO: add credit.
            credit = new AudioCredits.AudioCredit();
            credit.title = "-";
            credit.artist = "-";
            credit.collection = "-";
            credit.source = "-";
            credit.link1 = "";
            credit.link2 = "";
            credit.copyright = "-";

            bgmCredits.audioCredits.Add(credit);
        }

        // Loads the SFX and JNG credits.
        private void LoadSoundEffectCredits()
        {
            AudioCredits.AudioCredit credit;

            // Loads sound effects and jingles.

            // Sound Effects
            // TODO: add credit.
            credit = new AudioCredits.AudioCredit();
            credit.title = "-";
            credit.artist = "-";
            credit.collection = "-";
            credit.source = "-";
            credit.link1 = "";
            credit.link2 = "";
            credit.copyright = "-";

            sfxCredits.audioCredits.Add(credit);
        }

        // Enables all the credit buttons.
        public void EnableAllCreditButtons()
        {
            // Disables all the credit buttons.
            bgmButton.interactable = true;
            sfxButton.interactable = true;
        }

        // Disables all the credit buttons.
        public void DisableAllCreditButtons()
        {
            // Disables all the credit buttons.
            bgmButton.interactable = false;
            sfxButton.interactable = false;
        }

        // Shows the BGM credits.
        public void ShowBackgroundMusicCredits()
        {
            // Saves the current credit index, switches over, and then sets the new credit index.
            SaveCurrentCreditIndex();
            creditsInterface.audioCredits = bgmCredits;
            creditsInterface.SetCreditIndex(bgmCreditsIndex);

            // Change button settings.
            EnableAllCreditButtons();
            bgmButton.interactable = false;
        }

        // Shows the SFX credits.
        public void ShowSoundEffectCredits()
        {
            // Saves the old credits to know what button to enable.
            AudioCredits oldCredits = creditsInterface.audioCredits;

            // Saves the current credit index, switches over, and then sets the new credit index.
            SaveCurrentCreditIndex();
            creditsInterface.audioCredits = sfxCredits;
            creditsInterface.SetCreditIndex(sfxCreditsIndex);

            // Change button settings.
            EnableAllCreditButtons();
            sfxButton.interactable = false;
        }

        // Gets the current credits.
        public AudioCredits GetCurrentCredits()
        {
            return creditsInterface.audioCredits;
        }

        // Returns 'true' if the BGM credits are the current credits.
        public bool IsCurrentCreditsBgmCredits()
        {
            return GetCurrentCredits() == bgmCredits;
        }

        // Returns 'true' if the SFX credits are the current credits.
        public bool IsCurrentCreditsSfxCredits()
        {
            return GetCurrentCredits() == sfxCredits;
        }

        // Saves the current credit index of the applicable window.
        public void SaveCurrentCreditIndex()
        {
            // Only one of these should be active at a time.
            if (IsCurrentCreditsBgmCredits())
            {
                bgmCreditsIndex = creditsInterface.GetCurrentCreditIndex();
            }
            else if (IsCurrentCreditsSfxCredits())
            {
                sfxCreditsIndex = creditsInterface.GetCurrentCreditIndex();
            }
        }
    }
}