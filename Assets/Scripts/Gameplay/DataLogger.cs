using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Logs data from the gameplay scenes (world, action, knowledge).
    // This doesn't have any proper functionality. It just tracks data.
    public class DataLogger : MonoBehaviour
    {
        // The singleton instance.
        private static DataLogger instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // // The saved game time. This script does not run the timer itself.
        // // This should be updated whenever a scene that measures time finishes.
        // [Tooltip("Saves the game time. This should be updated when a scene that tracks the game time ends.")]
        // public float savedGameTime = 0.0F;
        // 
        // [Header("World")]
        // 
        // 
        // 
        // [Header("Action")]
        // 
        // [Header("Knowledge")]
        // 
        // Constructor
        private DataLogger()
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
        public static DataLogger Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<DataLogger>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("DataLogger (singleton)");
                        instance = go.AddComponent<DataLogger>();
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

        // Update is called once per frame
        void Update()
        {

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