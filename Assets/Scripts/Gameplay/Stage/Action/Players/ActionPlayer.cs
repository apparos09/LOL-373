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

            // If the unit parent is null, make this object the unit parent.
            if (unitParent == null)
                unitParent = gameObject;
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

        // Sets the action unit parent to the unit parent object.
        // If 'thisParentIfNull' is true, this is set as the unit's parent if the unit parent object is null.
        public virtual void SetActionUnitParentToUnitParent(ActionUnit actionUnit, bool thisParentIfNull = true)
        {
            // If there's a dedicated unit parent, give it that as the parent.
            if (unitParent != null)
            {
                actionUnit.transform.parent = unitParent.transform;
            }
            // No unit parent, so make this object the parent of the enemy unit.
            else
            {
                actionUnit.transform.parent = (thisParentIfNull) ? transform : null;
            }
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