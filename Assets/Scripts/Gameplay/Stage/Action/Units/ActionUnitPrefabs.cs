using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action unit prefabs.
    public class ActionUnitPrefabs : MonoBehaviour
    {
        // The singleton instance.
        private static ActionUnitPrefabs instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // NOTE: the values at index 0 shouldn't be used, as those are just default versions or empty values.
        // The order in the list should match up with the id numbers.

        // The generator prefabs.
        public List<ActionUnitGenerator> generatorPrefabs = new List<ActionUnitGenerator>();

        // The defense prefabs.
        public List<ActionUnitDefense> defensePrefabs = new List<ActionUnitDefense>();

        // The enemy prefabs.
        public List<ActionUnitEnemy> enemyPrefabs = new List<ActionUnitEnemy>();

        // Constructor
        private ActionUnitPrefabs()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }

        // Gets the instance.
        public static ActionUnitPrefabs Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<ActionUnitPrefabs>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Action Unit Prefabs (singleton)");
                        instance = go.AddComponent<ActionUnitPrefabs>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Gets the prefab.
        protected ActionUnit GetPrefab(ActionUnit.unitType unitType, int index)
        {
            // The prefab.
            ActionUnit prefab;

            // Gets the prefab based on the provided tpye.
            switch (unitType)
            {
                case ActionUnit.unitType.unknown:
                default:
                    prefab = null;
                    break;

                case ActionUnit.unitType.generator:
                    prefab = GetGeneratorPrefab(index);
                    break;

                case ActionUnit.unitType.defense:
                    prefab = GetDefensePrefab(index);
                    break;

                case ActionUnit.unitType.enemy:
                    prefab = GetEnemyPrefab(index);
                    break;
            }

            return prefab;
        }

        // Instantiates and returns a prefab.
        // The unit id determines what kind of action unit to use.
        protected ActionUnit InstantiatePrefab(ActionUnit.unitType unitType, int index)
        {
            // The prefab and the new object.
            ActionUnit prefab = GetPrefab(unitType, index);
            ActionUnit actionUnit = null;

            // If the prefab isn't equal to null, instantiate it.
            if (prefab != null)
            {
                actionUnit = Instantiate(prefab);
            }

            // Returns the new UNIT.
            return actionUnit;
        }

        // Gets a generator prefab. This does NOT instantiate the prefab.
        public ActionUnitGenerator GetGeneratorPrefab(int index)
        {
            // Index validity check.
            if (index >= 0 && index < generatorPrefabs.Count)
            {
                return generatorPrefabs[index];
            }
            else
            {
                return null;
            }
        }

        // Instantiates and returns a prefab.
        public ActionUnitGenerator InstantiateGeneratorPrefab(int index)
        {
            // Gets the prefab.
            ActionUnitGenerator prefab = GetGeneratorPrefab(index);

            // If the prefab exists, instantiate it.
            // If it doesn't exist, return null.
            if (prefab != null)
                return Instantiate(prefab);
            else
                return null;
        }

        // Gets a defense prefab. This does NOT instantiate the prefab.
        public ActionUnitDefense GetDefensePrefab(int index)
        {
            // Index validity check.
            if (index >= 0 && index < defensePrefabs.Count)
            {
                return defensePrefabs[index];
            }
            else
            {
                return null;
            }
        }

        // Instantiates and returns a prefab.
        public ActionUnitDefense InstantiateDefensePrefab(int index)
        {
            // Gets the prefab.
            ActionUnitDefense prefab = GetDefensePrefab(index);

            // If the prefab exists, instantiate it.
            // If it doesn't exist, return null.
            if (prefab != null)
                return Instantiate(prefab);
            else
                return null;
        }

        // Gets a enemy prefab. This does NOT instantiate the prefab.
        public ActionUnitEnemy GetEnemyPrefab(int index)
        {
            // Index validity check.
            if (index >= 0 && index < enemyPrefabs.Count)
            {
                return enemyPrefabs[index];
            }
            else
            {
                return null;
            }
        }

        // Instantiates and returns a prefab.
        public ActionUnitEnemy InstantiateEnemyPrefab(int index)
        {
            // Gets the prefab.
            ActionUnitEnemy prefab = GetEnemyPrefab(index);

            // If the prefab exists, instantiate it.
            // If it doesn't exist, return null.
            if (prefab != null)
                return Instantiate(prefab);
            else
                return null;
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}