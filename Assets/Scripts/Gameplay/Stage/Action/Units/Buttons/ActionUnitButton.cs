using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // A button used for selecting an action unit by the player.
    public class ActionUnitButton : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The button script.
        public Button button;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the instance.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // If the button isn't set, try to get the component in the children.
            if (button == null)
                button = GetComponentInChildren<Button>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}