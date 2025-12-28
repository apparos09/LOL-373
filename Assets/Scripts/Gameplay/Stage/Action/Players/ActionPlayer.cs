using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace RM_EDU
{
    // An action player.
    public abstract class ActionPlayer : MonoBehaviour
    {
        // The action manager.
        public ActionManager actionManager;

        // The action unit prefabs.
        public ActionUnitPrefabs actionUnitPrefabs;

        // The amount of energy the player has.
        public float energy = 0;

        // The parent of the units created by the player.
        public GameObject unitParent;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the action manager is null, get the action manager.
            if (actionManager == null)
                actionManager = ActionManager.Instance;

            // The action unit prefabs.
            if (actionUnitPrefabs == null)
                actionUnitPrefabs = ActionUnitPrefabs.Instance;
        }

        // Returns 'true' if the action player can create a unit with the provided energy cost.
        public bool CanCreateActionUnit(float energyCreationCost)
        {
            return energyCreationCost <= energy;
        }

        // Returns 'true' if the action player can create the provided action unit.
        public bool CanCreateActionUnit(ActionUnit actionUnit)
        {
            return CanCreateActionUnit(actionUnit.energyCreationCost);
        }

        // Resets the player.
        public abstract void ResetPlayer();

        // Update is called once per frame
        protected virtual void Update()
        {
            // ...
        }
    }
}