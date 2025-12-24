using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RM_EDU
{
    // A button used for selecting an action unit by the player.
    public class ActionUnitButton : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The button script.
        public Button button;

        // The name of the unit.
        public TMP_Text unitNameText;

        // The energy cost text. This displays the amount of energy needed to create the unit.
        public TMP_Text energyCostText;

        // The highlight on the top of the card, which indicates the card above is usable.
        [Tooltip("An image above the card that indicates the card above the current one is usable.")]
        public Image topHighlightImage;

        // The highlight on the bottom of the card, which indicates the card below is usable.
        [Tooltip("An image below the card that indicates the card below the current one is usable.")]
        public Image bottomHighlightImage;

        // The unit prefab.
        public ActionUnit unitPrefab;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Gets the instance.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // If the button isn't set, try to get the component in the children.
            if (button == null)
                button = GetComponentInChildren<Button>();


            // Adds a Select() call on the button.
            if (button != null)
            {
                // Listener for the tutorial toggle.
                button.onClick.AddListener(delegate
                {
                    Select();
                });
            }
        }

        // Called when the button has been pressed.
        public virtual void Select()
        {
            // Gets the player user.
            ActionPlayerUser playerUser = actionUI.actionManager.playerUser;

            // If the player exists, give it this unit prefab.
            if(playerUser != null)
            {
                // Give the player the selected prefab.
                playerUser.SetSelectedUnitPrefab(unitPrefab);
            }
        }

        // Gets the unit type.
        public virtual ActionUnit.unitType GetUnitType()
        {
            // The unit type.
            ActionUnit.unitType unitType;

            // Prefab exists.
            if(unitPrefab != null)
            {
                unitType = unitPrefab.GetUnitType();
            }
            else
            {
                // There's no prefab, so return unknown type.
                unitType = ActionUnit.unitType.unknown;
            }

            // Returns the unit type.
            return unitType;
        }

    }
}