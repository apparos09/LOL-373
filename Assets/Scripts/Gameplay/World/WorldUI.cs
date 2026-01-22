using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The world UI.
    public class WorldUI : GameplayUI
    {
        // The singleton instance.
        private static WorldUI instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("World")]

        // The world manager.
        public WorldManager worldManager;

        // The previous area button.
        public Button prevAreaButton;

        // The next area button.
        public Button nextAreaButton;

        [Header("World/Dialogs")]

        // The options dialog.
        public GameObject optionsDialog;

        // The game settings.
        public GameSettingsUI settingsDialog;

        // The stage prompt.
        // NOTE: this is different from the other dialogs since it's part of the world map...
        // And not a dedicated button on the header.
        public WorldStageDialog stageDialog;


        // Awake is called when the script is being loaded
        protected override void Awake()
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

            base.Awake();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Gets the world manager instance.
            if(worldManager == null)
            {
                worldManager = WorldManager.Instance;
            }

            // Closes all the dialogs.
            CloseAllDialogs();
        }

        // Gets the instance.
        public static WorldUI Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<WorldUI>(FindObjectsInactive.Include);
                    
                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("World UI (singleton)");
                        instance = go.AddComponent<WorldUI>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Goes to the previous world area.
        public void PreviousWorldArea(bool wrapAround)
        {
            worldManager.PreviousWorldArea(wrapAround);
        }

        // Goes to the pervious area, while not allowing wrap arounds.
        public void PreviousWorldArea()
        {
            worldManager.PreviousWorldArea();
        }

        // Goes to the next world area.
        public void NextWorldArea(bool wrapAround)
        {
            worldManager.NextWorldArea(wrapAround);
        }

        // Goes to the next area, while not allowing wrap arounds.
        public void NextWorldArea()
        {
            worldManager.NextWorldArea();
        }

        // Refreshes the buttons that're used to jump between areas.
        public void RefreshWorldAreaButtons()
        {
            // If the world manager isn't set, grab the instance.
            if (worldManager == null)
                worldManager = WorldManager.Instance;

            // Set the area buttons as interactable by default.
            prevAreaButton.interactable = true;
            nextAreaButton.interactable = true;

            // PREVIOUS
            // If this is the first area, disable the previous button by default.
            if (worldManager.IsCurrentWorldAreaFirstArea())
            {
                prevAreaButton.interactable = false;
            }
            else
            {
                // Not the first area, so the player can go back.
                prevAreaButton.interactable = true;
            }

            // NEXT
            // If this is the last area, disable the next button.
            if (worldManager.IsCurrentWorldAreaLastArea())
            {
                nextAreaButton.interactable = false;
            }
            else
            {
                // Not the last area, so check if the current area is complete.
                WorldArea currArea = worldManager.GetCurrentWorldArea();

                // If the area is cleared, allow going to the next area.
                nextAreaButton.interactable = currArea.IsWorldAreaCleared();
            }

        }

        // DIALOGS
        // Opens the given dialog.
        public virtual void OpenDialog(GameObject dialog, bool closeOtherDialogs)
        {
            // If 'true', close all the other dialogs.
            if (closeOtherDialogs)
            {
                CloseAllDialogs();
            }

            // Activates the dialog box.
            dialog.SetActive(true);

            // Pauses the game.
            worldManager.PauseGame();
        }

        // Closes the given dialog.
        public virtual void CloseDialog(GameObject dialog)
        {
            dialog.SetActive(false);

            // If no dialogs are open, unpause the game.
            if (!IsDialogOpen())
            {
                worldManager.UnpauseGame();
            }
        }

        // Closes all dialog boxes (windows).
        public virtual void CloseAllDialogs()
        {
            // Generates a dialog list.
            List<GameObject> dialogList = GenerateDialogList();

            // Goes through each dialog.
            foreach (GameObject dialog in dialogList)
            {
                // If the dialog exist, close it.
                if (dialog != null)
                {
                    // NOTE: this doesn't use the CloseDialog() function, because that function...
                    // Checks if any dialogs are open to know if the game should be unpaused.
                    // This function does the same thing after all dialogs are closed.

                    // CloseDialog(dialog);
                    dialog.gameObject.SetActive(false);
                }
            }

            // Unpause the game since all dialogs are closed.
            worldManager.UnpauseGame();
        }

        // Generates a list of dialogs.
        public virtual List<GameObject> GenerateDialogList()
        {
            // NOTE: the stage dialog isn't included since it's not a menu dialog.

            // The list to return, which is given the dialogs in this script.
            List<GameObject> dialogList = new List<GameObject>
            {
                optionsDialog,
                settingsDialog.gameObject
            };

            return dialogList;
        }

        // Returns true if a dialog is open.
        public virtual bool IsDialogOpen()
        {
            // Set to see if there's a dialog open.
            bool result = false;

            // Generates a list of dialogs.
            List<GameObject> dialogList = GenerateDialogList();

            // Goes through all dialogs.
            foreach (GameObject dialog in dialogList)
            {
                // Dialog exists.
                if (dialog != null)
                {
                    // If an active dialog has been found.
                    if (dialog.activeSelf)
                    {
                        result = true;
                        break;
                    }
                }
            }

            // Return result.
            return result;
        }

        // Options
        // Returns 'true' if the options dialog is open.
        public bool IsOptionsDialogOpen()
        {
            return optionsDialog.gameObject.activeSelf;
        }

        // Opens the options dialog.
        public void OpenOptionsDialog(bool closeOtherDialogs)
        {
            OpenDialog(optionsDialog.gameObject, closeOtherDialogs);
        }

        // Closes the options dialog.
        public void CloseOptionsDialog()
        {
            CloseDialog(optionsDialog.gameObject);
        }

        // Returns 'true' if the settigns dialog is open.
        public bool IsSettingsDialogOpen()
        {
            return settingsDialog.gameObject.activeSelf;
        }

        // Opens the settings dialog.
        public void OpenSettingsDialog(bool closeOtherDialogs)
        {
            OpenDialog(settingsDialog.gameObject, closeOtherDialogs);
        }

        // Closes the settings dialog.
        public void CloseSettingsDialog()
        {
            settingsDialog.gameObject.SetActive(false);
        }

        // Returns 'true' if the stage dialog is open.
        public bool IsWorldStageDialogOpen()
        {
            return stageDialog.gameObject.activeSelf;
        }

        // Opens the stage prompt.
        public void OpenWorldStageDialog(WorldStage worldStage)
        {
            stageDialog.SetWorldStage(worldStage);
            stageDialog.gameObject.SetActive(true);
        }

        // Closes the stage prompt.
        public void CloseWorldStageDialog()
        {
            stageDialog.gameObject.SetActive(false);
        }

        // SAVE
        // Saves and continues the game.
        public void SaveAndContinue()
        {
            worldManager.SaveAndContinue();
        }

        // Saves and quits the game.
        public void SaveAndQuit()
        {
            worldManager.SaveAndQuit();
        }

        // Quit without saving.
        public void QuitWithoutSaving()
        {
            worldManager.QuitWithoutSaving();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}