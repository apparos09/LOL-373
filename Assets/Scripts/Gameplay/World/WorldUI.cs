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

        // The game settings.
        public GameSettingsUI settingsUI;

        // The stage prompt.
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

            // Turn off the settings UI.
            if(settingsUI != null)
            {
                settingsUI.gameObject.SetActive(false);
            }

            // Turn off the stage prompt.
            if(stageDialog != null)
            {
                stageDialog.gameObject.SetActive(false);
            }
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

        // DIALOGS //

        // Returns 'true' if the settigns dialog is open.
        public bool IsSettingsDialogOpen()
        {
            return settingsUI.gameObject.activeSelf;
        }

        // Opens the settings dialog.
        public void OpenSettingsDialog()
        {
            settingsUI.gameObject.SetActive(true);
        }

        // Closes the settings dialog.
        public void CloseSettingsDialog()
        {
            settingsUI.gameObject.SetActive(false);
        }

        // TODO: expand on these functions.
        // Opens the stage prompt.
        public void OpenStageDialog(WorldStage worldStage)
        {
            stageDialog.SetWorldStage(worldStage);
            stageDialog.gameObject.SetActive(true);
        }

        // Closes the stage prompt.
        public void CloseStageDialog()
        {
            stageDialog.gameObject.SetActive(false);
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