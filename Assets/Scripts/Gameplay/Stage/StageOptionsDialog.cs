using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The stage options dialog.
    public class StageOptionsDialog : MonoBehaviour
    {
        // The stage UI.
        public StageUI stageUI;

        // Start is called before the first frame update
        void Start()
        {
            // If the stage UI isn't set, try to get it.
            if (stageUI == null)
                stageUI = FindObjectOfType<StageUI>();
        }

        // Resets the stage.
        public void ResetStage()
        {
            stageUI.ResetStage();
        }

        // Quits the stage, which loads the world scene.
        public void QuitStage()
        {
            stageUI.QuitStage();
        }


        // Opens the settings dialog.
        public void OpenSettingsDialog()
        {
            stageUI.OpenSettingsDialog(true);
        }

        // Closes the options dialog.
        public void CloseOptionsDialog()
        {
            stageUI.CloseOptionsDialog();
        }
    }
}