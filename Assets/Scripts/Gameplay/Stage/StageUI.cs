using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The stage UI.
    public abstract class StageUI : GameplayUI
    {
        [Header("Stage UI")]
        // The game will skip these overwritten functions in StageUI and go to GameplayUI instead.
        // Uncomment these functions if actual functionaliy is added.

        // The stage manager.
        public StageManager stageManager;

        // The game settings UI.
        public GameSettingsUI settingsDialog;

        // // Awake is called when the script is being loaded
        // protected override void Awake()
        // {
        //     base.Awake();
        // }
        // 
        // // Update is called once per frame
        // protected override void Update()
        // {
        //     base.Update();
        // }
        // 
        // protected override void OnDestroy()
        // {
        //     base.OnDestroy();
        // }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Finds the stage manager if it isn't set.
            if (stageManager == null)
                stageManager = FindObjectOfType<StageManager>();

            // Makes sure the settings UI is off.
            if (settingsDialog != null)
                settingsDialog.gameObject.SetActive(false);
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
            stageManager.PauseGame();
        }

        // Closes the given window.
        public virtual void CloseDialog(GameObject dialog)
        {
            dialog.SetActive(false);

            // If no dialogs are open, unpause the game.
            if (!IsDialogOpen())
            {
                stageManager.UnpauseGame();
            }
        }

        // Closes all dialog boxes (windows).
        public virtual void CloseAllDialogs()
        {
            settingsDialog.gameObject.SetActive(false);

            // Unpause the game since all dialogs are closed.
            stageManager.UnpauseGame();
        }

        // Returns true if a dialog is open.
        public virtual bool IsDialogOpen()
        {
            // The dialog
            bool dialogOpen = false;

            if (settingsDialog.gameObject.activeSelf)
            {
                dialogOpen = true;
            }
            else
            {
                dialogOpen = false;
            }

            return dialogOpen;
        }

        // Opens the settings dialog.
        public void OpenSettingsDialog()
        {
            OpenDialog(settingsDialog.gameObject, true);
        }

        // Called to finish the stage.
        // This just calls the appropriate function in the manager.
        public virtual void FinishStage()
        {
            stageManager.FinishStage();
        }

    }
}