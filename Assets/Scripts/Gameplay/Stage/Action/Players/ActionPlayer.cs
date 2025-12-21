using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // An action player.
    public abstract class ActionPlayer : MonoBehaviour
    {
        // The action manager.
        public ActionManager actionManager;

        // The amount of energy the player has.
        public float energy = 0;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the action manager is null, get the action manager.
            if (actionManager == null)
                actionManager = ActionManager.Instance;
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }
}