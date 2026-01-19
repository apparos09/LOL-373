using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RM_EDU
{
    // The UI for the selected action unit.
    public class ActionUnitSelectedUI : MonoBehaviour
    {
        // The aciton UI.
        public ActionUI actionUI;

        // The selected unit.
        public ActionUnitCard selectedUnit;

        // The selected unit name text.
        public TMP_Text selectedUnitNameText;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the action unit UI.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // Gets the component in the children if it isn't set.
            if (selectedUnit == null)
                selectedUnit = GetComponentInChildren<ActionUnitCard>();
        }

        // Applies information from action unit.
        public void ApplyActionUnitInfo(ActionUnit actionUnit)
        {
            selectedUnit.ApplyActionUnitInfo(actionUnit);

            selectedUnitNameText.text = actionUnit.name;
        }

        // Applies information from an action unit button.
        public void ApplyActionUnitInfo(ActionUnitButton unitButton)
        {
            selectedUnit.ApplyActionUnitInfo(unitButton);

            selectedUnitNameText.text = unitButton.name;
        }

        // Applies info from the remove button.
        public void ApplyRemoveActionUnitInfo(ActionUnitRemoveButton removeButton)
        {
            selectedUnit.ApplyRemoveActionUnitInfo(removeButton);

            selectedUnitNameText.text = removeButton.name;
        }

        // Clears the information displayed for the icon.
        public void ClearActionUnitInfo()
        {
            selectedUnit.ClearActionUnitInfo();

            selectedUnitNameText.text = "-";
        }

    }
}