using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The user action player.
    public class ActionPlayerUser : ActionPlayer
    {
        // The unit prefab the action player user has selected.
        private ActionUnit selectedUnitPrefab;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the player user is null, set it to this.
            if(actionManager.playerUser == null)
            {
                actionManager.playerUser = this;
            }
        }

        // Returns 'true' if the player is selecting a unit.
        public bool IsSelectingUnitPrefab()
        {
            return selectedUnitPrefab != null;
        }

        // Gets the unit the player has selected.
        public ActionUnit GetSelectedUnitPrefab()
        {
            return selectedUnitPrefab;
        }

        // Sets the prefab the player has selected. This should be a prefab, not an actual object.
        public void SetSelectedUnitPrefab(ActionUnit unitPrefab)
        {
            selectedUnitPrefab = unitPrefab;
        }

        // Clears the prefab the player has selected.
        public void ClearSelectedUnitPrefab()
        {
            selectedUnitPrefab = null;
        }

        // Resets the player.
        public override void ResetPlayer()
        {
            ClearSelectedUnitPrefab();
            // TODO: reset energy amount.
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}
