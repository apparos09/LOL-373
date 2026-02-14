using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The world options dialog.
    public class WorldOptionsDialog : MonoBehaviour
    {
        // The world UI.
        public WorldUI worldUI;

        // Start is called before the first frame update
        void Start()
        {
            if (worldUI == null)
                worldUI = WorldUI.Instance;
        }

        // Saves and continues the game.
        public void SaveAndContinue()
        {
            worldUI.SaveAndContinue();
        }

        // Saves and quits the game.
        public void SaveAndQuit()
        {
            worldUI.SaveAndQuit();
        }

        // Quits the game without saving.
        public void QuitWithoutSaving()
        {
            worldUI.QuitWithoutSaving();
        }

        // Opens the settings dialog.
        public void OpenSettingsDialog()
        {
            worldUI.OpenSettingsDialog(true);
        }

        // Closes the options dialog.
        public void CloseOptionsDialog()
        {
            worldUI.CloseOptionsDialog();
        }


    }
}