using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The action stage optiosn dialog.
    public class ActionStageOptionsDialog : StageOptionsDialog
    {
        [Header("Action")]

        // The action UI.
        public ActionUI actionUI;

        // The generator info button.
        public Button generatorInfoButton;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Gets the action UI instance.
            if (actionUI == null)
                actionUI = ActionUI.Instance;
        }

        // Opens the generator info dialog.
        public void OpenGeneratorInfoDialog()
        {
            actionUI.OpenGeneratorInfoDialog(true);
        }
    }
}