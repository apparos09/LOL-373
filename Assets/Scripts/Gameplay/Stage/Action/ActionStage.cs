using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action stage.
    public class ActionStage : MonoBehaviour
    {
        // The action manager.
        public ActionManager actionManager;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the action manager.
            if (actionManager == null)
                actionManager = ActionManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}