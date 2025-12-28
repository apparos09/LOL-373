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

        // Start is called before the first frame update
        void Start()
        {
            // Setting the action UI if it's null.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // Setting the button if it's null.
            if(button == null)
                button = GetComponent<Button>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}