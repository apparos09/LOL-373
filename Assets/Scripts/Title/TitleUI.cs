using LoLSDK;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_EDU
{
    // The title UI.
    public class TitleUI : MonoBehaviour
    {
        // The singleton instance.
        private static TitleUI instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The manager.
        public TitleManager titleManager;

        [Header("Main Buttons")]

        // The new game button, new game (with mode), and continue button.
        public Button newGameButton;
        public Button continueButton;

        // The instructions, settings, and licenses.
        public Button instructionsButton;
        public Button settingsButton;
        public Button licensesButton;

        // The quit button.
        public Button quitButton;

        [Header("Windows")]
        // The title window.
        public GameObject titleDialog;

        // The instructions, settings, and credits windows.
        public GameObject instructionsDialog;
        public GameSettingsUI settingsDialog;
        public Licenses licensesDialog;

        [Header("Other")]
        // The save text for the game.
        public TMP_Text saveText;

        // Constructor
        private TitleUI()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            // The manager.
            if (titleManager == null)
                titleManager = TitleManager.Instance;

            // If the platform is set to webGL, disable the quit button.
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                // The quit button doesn't do anything in WebGL, so turn it off.
                quitButton.interactable = false; // Disable               
            }

            // Makes the save text empty by default.
            saveText.text = string.Empty;

            // Save the save text as the save feedback text.
            if (LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
            {
                // Set the save text for the save system if it exists.
                if (LOLManager.Instance.saveSystem != null)
                {
                    // Gets the save system.
                    SaveSystem saveSystem = LOLManager.Instance.saveSystem;

                    // If there is loaded data, make the continue button interactable.
                    // If there is no loaded data, make the continue button non-interactable.
                    continueButton.interactable = saveSystem.HasLoadedData();

                    // Sets the feedback text.
                    saveSystem.feedbackText = saveText;
                }
                else
                {
                    // The save system doesn't exist, so there's no save data to pull.
                    // As such, the continue button is turned not interactable.
                    continueButton.interactable = false;
                }

                // Hide the quit button.
                quitButton.gameObject.SetActive(false);
            }
            else
            {
                // Enable the continue button and show the quit button.
                continueButton.interactable = false;
                quitButton.gameObject.SetActive(true);
            }

            // Opens the title window at the start.
            OpenTitleDialog();
        }

        // Gets the instance.
        public static TitleUI Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<TitleUI>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Title UI (singleton)");
                        instance = go.AddComponent<TitleUI>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Starts the new game.
        public void StartNewGame()
        {
            titleManager.StartNewGame();
        }

        // Continues the game.
        public void ContinueGame()
        {
            titleManager.ContinueGame();
        }


        // OPENING/CLOSING DIALOGS/WINDOWS

        // Opens the given window.
        public void OpenDialog(GameObject dialog)
        {
            CloseAllDialogs();
            dialog.SetActive(true);
        }

        // Closes the given window.
        public void CloseDialog(GameObject dialog)
        {
            dialog.SetActive(false);
        }

        // Closes all dialog boxes (windows).
        public void CloseAllDialogs()
        {
            titleDialog.SetActive(false);
            instructionsDialog.SetActive(false);
            settingsDialog.gameObject.SetActive(false);
            licensesDialog.gameObject.SetActive(false);
        }

        // Opens the title dialog, which closes all the other dialogs.
        public void OpenTitleDialog()
        {
            CloseAllDialogs();
            titleDialog.SetActive(true);
        }

        // Opens the instructions dialog.
        public void OpenInstructionsDialog()
        {
            CloseAllDialogs();
            instructionsDialog.SetActive(true);
        }

        // Opens the settings dialog.
        public void OpenSettingsDialog()
        {
            CloseAllDialogs();
            settingsDialog.gameObject.SetActive(true);
        }

        // Opens the licenses dialog.
        public void OpenLicensesDialog()
        {
            CloseAllDialogs();
            licensesDialog.gameObject.SetActive(true);
        }

        // Other

        // Quits the game.
        public void QuitGame()
        {
            titleManager.QuitGame();
        }

        // This function is called when the MonoBehaviour will be destroyed.
        private void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}