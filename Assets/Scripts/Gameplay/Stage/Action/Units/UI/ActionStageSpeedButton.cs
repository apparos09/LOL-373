using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // A button used to change the action stage's speed.
    public class ActionStageSpeedButton : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The unit remove button.
        public Button button;

        // The icon image.
        public Image iconImage;

        [Header("Sprites")]
        // Slow speed icon.
        public Sprite speedSlowIconSprite;

        // Normal speed icon.
        public Sprite speedNormalIconSprite;

        // Fast speed icon.
        public Sprite speedFastIconSprite;

        // Start is called before the first frame update
        void Start()
        {
            // Setting the action UI if it's null.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // Setting the button if it's null.
            if (button == null)
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
            // TODO: speed
        }

        // Refreshes the speed icon based on the time scale.
        public void RefreshSpeedIcon()
        {
            // TODO: implement.
        }

    }
}