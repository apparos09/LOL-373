using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // A button used to remove an action unit on the map.
    public class ActionUnitRemoveButton : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The unit remove button.
        public Button button;

        // The card background sprite.
        public Sprite cardBackgroundSprite;

        // The icon for the card.
        public Sprite cardIconSprite;

        // Start is called before the first frame update
        void Start()
        {
            // Setting the action UI if it's null.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // Setting the button if it's null.
            if(button == null)
                button = GetComponent<Button>();

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
            ActionPlayerUser playerUser = ActionManager.Instance.playerUser;

            // If the player exists, give it this unit prefab.
            if (playerUser != null)
            {
                // If the player is in remove mode, turn it off.
                if(playerUser.InRemoveMode())
                {
                    playerUser.DisableRemoveMode();
                }
                // If the player isn't in remove mode, turn it on.
                else
                {
                    // Give the player the selected prefab.
                    playerUser.EnableRemoveMode();
                }
            }
        }

        // Gets the remove card's name.
        public string GetRemoveCardName()
        {
            // The name to return.
            string cardName;

            // Checks if the language file is available.
            if (LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
            {
                string key = GetRemoveCardNameKey();
                cardName = LOLManager.Instance.GetLanguageText(key);
            }
            else
            {
                cardName = "Remove";
            }

            return cardName;
        }

        // Gets the remove card's name key.
        public string GetRemoveCardNameKey()
        {
            return "kwd_remove";
        }

        // Gets the display name for the unit's card.
        public string GetRemoveCardDisplayName()
        {
            // The name to return.
            string displayName;

            // Checks if the language file is available.
            if(LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
            {
                string key = GetRemoveCardDisplayNameKey();
                displayName = LOLManager.Instance.GetLanguageText(key);
            }
            else
            {
                displayName = "RMV";
            }

            return displayName;
        }

        // Gets the remove card display name key.
        public string GetRemoveCardDisplayNameKey()
        {
            return "kwd_remove_abv";
        }
    }
}