using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Holds info for the gameplay scenes.
    // This should be set to not be destroyed on load, created before the gameplay starts, and destroyed once the game finishes.
    public class GameplayInfo : MonoBehaviour
    {
        // The singleton instance.
        private static GameplayInfo instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The statements in the knowledge stage.
        public List<KnowledgeStatement> statements = new List<KnowledgeStatement>();

        // The resources in the knowledge stage.
        public List<KnowledgeResource> resources = new List<KnowledgeResource>();

        // Constructor
        private GameplayInfo()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        void Awake()
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
        void Start()
        {

        }

        // Gets the instance.
        public static GameplayInfo Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<GameplayInfo>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("GameplayInfo (singleton)");
                        instance = go.AddComponent<GameplayInfo>();
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

    }
}