using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using util;

namespace RM_EDU
{
    // The title UI.
    public class TitleUI : MonoBehaviour
    {
        // The manager.
        public TitleManager manager;

        [Header("Buttons")]

        // The new game button, new game (with mode), and continue button.
        public Button newGameButton;
        public Button newGameModeButton;
        public Button continueButton;

        // The instructions, settings, and licenses.
        public Button instructionsButton;
        public Button settingsButton;
        public Button licensesButton;

        // The quit button.
        public Button quitButton;

        [Header("Windows")]
        // The title window.
        public GameObject titleWindow;

        // The mode window for the game.
        public GameObject modeWindow;

        // The instructions, settings, and credits windows.
        public GameObject instructionsWindow;
        public GameSettingsUI settingsWindow;
        public AudioCreditsInterface licensesWindow;

        [Header("Other")]
        // The save text for the game.
        public TMP_Text saveText;

        // Start is called before the first frame update
        void Start()
        {
            if (manager == null)
                manager = TitleManager.Instance;

            // If the platform is set to webGL, disable the quit button.
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                // The quit button doesn't do anything in WebGL, so turn it off.
                quitButton.interactable = false; // Disable               
            }

            // If the LOLSDK has been initialized.
            if (GameSettings.InitializedLOLSDK)
            {
                quitButton.gameObject.SetActive(false); // Turn-Off
            }
            // If the LOLSDK isn't initialized, make the continue button non-interactable.
            else
            {
                // Disable continue.
                continueButton.interactable = false;
            }

            // Save the save text as the save feedback text.
            if (LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
            {
                // Set the save text for the save system if it exists.
                if (LOLManager.Instance.saveSystem != null)
                {
                    LOLManager.Instance.saveSystem.feedbackText = saveText;
                }

                // Show the new game button and hide the mode button.
                newGameButton.gameObject.SetActive(true);
                newGameModeButton.gameObject.SetActive(false);
            }
            else
            {
                // Hide the new game button and show the mode button.
                newGameButton.gameObject.SetActive(false);
                newGameModeButton.gameObject.SetActive(true);
            }

            // Opens the title window at the start.
            OpenWindow(titleWindow);
        }

        // Starts the new game.
        public void StartNewGame()
        {
            manager.StartNewGame();
        }

        // Continues the game.
        public void ContinueGame()
        {
            manager.ContinueGame();
        }


        // OPENING/CLOSING WINDOWS

        // Opens the given window.
        public void OpenWindow(GameObject window)
        {
            CloseAllWindows();
            window.SetActive(true);
        }

        // Closes the given window.
        public void CloseWindow(GameObject window)
        {
            window.SetActive(false);
        }

        // Closes all windows.
        public void CloseAllWindows()
        {
            titleWindow.SetActive(false);
            instructionsWindow.SetActive(false);
            settingsWindow.gameObject.SetActive(false);
            licensesWindow.gameObject.SetActive(false);
            modeWindow.gameObject.SetActive(false);
        }

        // Other

        // Quits the game.
        public void QuitGame()
        {
            manager.QuitGame();
        }
    }
}