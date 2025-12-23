using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Holds all the data for the action stage.
    public class ActionStageList : MonoBehaviour
    {
        // The singleton instance.
        private static ActionStageList instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // Constructor
        private ActionStageList()
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

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // ...
        }

        // Gets the instance.
        public static ActionStageList Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<ActionStageList>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Action Stage List (singleton)");
                        instance = go.AddComponent<ActionStageList>();
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

        // Stage maps are 16x7.
        // The lane blasters are all in the first column, so the usable space for the player is 15x7.
        // For the tile set, the two digit number determines the tile type, and the letter on the end determines the version used (e.g., "00A").
        /*
         * Tileset:
         *  0: nothing
         *  1: land
         *  2: river
         *  3: sea
         */

        // Generates a map based on the provided value. If there's no map to match that value, null is returned.
        public static string[,] GenerateStageMap(int idNumber)
        {
            // The map to be returned.
            string[,] map;

            // Checks the value to see what map to return.
            switch(idNumber)
            {
                default:
                    map = null;
                    break;
                    
                case 0:
                    map = GenerateStageMapDebug();
                    break;
            }

            return map;
        }

        // Generates and returns an empty stage map.
        private static string[,] GenerateStageMapEmpty()
        {
            // An empty map to use as a base to make other function.
            string[,] map = new string[,] {
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" }
            };

            return map;
        }

        // Generates and returns the debug stage map.
        public static string[,] GenerateStageMapDebug()
        {
            // A debug stage map.
            string[,] map = new string[,] {
                { "01A", "00A", "01A", "02A", "03A", "02A", "01A", "00A", "01A", "02A", "03A", "02A", "01A", "00A", "01A", "02A" },
                { "01A", "00A", "01A", "02A", "03A", "02A", "01A", "00A", "01A", "02A", "03A", "02A", "01A", "00A", "01A", "02A" },
                { "01A", "00A", "01A", "02A", "03A", "02A", "01A", "00A", "01A", "02A", "03A", "02A", "01A", "00A", "01A", "02A" },
                { "01A", "00A", "01A", "02A", "03A", "02A", "01A", "00A", "01A", "02A", "03A", "02A", "01A", "00A", "01A", "02A" },
                { "01A", "00A", "01A", "02A", "03A", "02A", "01A", "00A", "01A", "02A", "03A", "02A", "01A", "00A", "01A", "02A" },
                { "01A", "00A", "01A", "02A", "03A", "02A", "01A", "00A", "01A", "02A", "03A", "02A", "01A", "00A", "01A", "02A" },
                { "01A", "00A", "01A", "02A", "03A", "02A", "01A", "00A", "01A", "02A", "03A", "02A", "01A", "00A", "01A", "02A" }
            };

            return map;
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