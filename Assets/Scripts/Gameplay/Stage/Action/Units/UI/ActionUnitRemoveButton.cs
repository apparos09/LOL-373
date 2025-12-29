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

        // The icon im
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
                // Give the player the selected prefab.
                playerUser.SetToRemoveMode();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}