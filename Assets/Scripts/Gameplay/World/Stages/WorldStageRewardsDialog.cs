using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The world stage rewards dialog.
    public class WorldStageRewardsDialog : MonoBehaviour
    {
        // The world UI.
        public WorldUI worldUI;

        // The selected reward unit.
        public TMP_LabeledValue selectedRewardUnit;

        // THe continue button.
        public Button continueButton;

        // Awake is called when the script instance is being loaded.
        private void Awake()
        {
            // Clears the selected unit info.
            ClearSelectedUnitInfo();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Gets the world UI.
            if (worldUI == null)
                worldUI = WorldUI.Instance;
        }

        // Applies the selected unit information from the reward button.
        public void ApplySelectedUnitInfo(ActionUnitRewardButton rewardButton)
        {
            selectedRewardUnit.headerText.text = rewardButton.unitPrefab.GetUnitNameTranslated();
            selectedRewardUnit.valueText.text = rewardButton.unitPrefab.GetUnitDescriptionTranslated();
        }

        // Clears the selected unit information.
        public void ClearSelectedUnitInfo()
        {
            selectedRewardUnit.headerText.text = "-";
            selectedRewardUnit.valueText.text = "-";
        }

        // Closes the dialog.
        public void CloseDialog()
        {
            WorldUI.Instance.CloseWorldStageRewardDialog();
        }
    }
}