using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action tile prefabs.
    public class ActionTilePrefabs : MonoBehaviour
    {
        // The singleton instance.
        private static ActionTilePrefabs instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The action tile prefabs.
        public List<ActionTile> tilePrefabs = new List<ActionTile>();

        // Constructor
        private ActionTilePrefabs()
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
        public static ActionTilePrefabs Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<ActionTilePrefabs>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Action Tile Prefabs (singleton)");
                        instance = go.AddComponent<ActionTilePrefabs>();
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

        // Gets a prefab. This does NOT instantiate the prefab.
        public ActionTile GetPrefab(int index)
        {
            // Index validity check.
            if(index >= 0 && index < tilePrefabs.Count)
            {
                return tilePrefabs[index];
            }
            else
            {
                return null;
            }
        }

        // Instantiates and returns a prefab.
        public ActionTile InstantiatePrefab(int index)
        {
            // The prefab and the new tile.
            ActionTile prefab = GetPrefab(index);
            ActionTile newTile = null;

            // If the prefab isn't equal to null, create an instance and return it.
            if (prefab != null)
            {
                newTile = Instantiate(prefab);
            }

            // Returns the new tile.
            return newTile;
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