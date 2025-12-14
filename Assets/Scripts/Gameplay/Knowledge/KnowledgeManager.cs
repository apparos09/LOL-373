using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RM_EDU
{
    // The knowledge manager.
    public class KnowledgeManager : GameplayManager
    {
        // The singleton instance.
        private static KnowledgeManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("Knowledge")]

        // The knowledge UI.
        public KnowledgeUI knowledgeUI;

        // The selected knowledge statement.
        public KnowledgeStatement selectedStatement;

        // The selected knowledge resource.
        public KnowledgeResource selectedResource;

        // Constructor
        private KnowledgeManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected override void Awake()
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

            base.Awake();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Sets the knowledge UI if it isn't already set.
            if(knowledgeUI == null)
            {
                knowledgeUI = KnowledgeUI.Instance;
            }
        }

        // Gets the instance.
        public static KnowledgeManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<KnowledgeManager>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Knowledge Manager (singleton)");
                        instance = go.AddComponent<KnowledgeManager>();
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

        // Clears the selections for the knowledge manager.
        public void ClearSelections()
        {
            // Checks if there's a selected statement.
            if(selectedStatement != null)
            {
                // Deselect the button.
                // selectedStatement.button.deselect
            }

            // Checks if there's a selected resource.
            if(selectedResource != null)
            {
                // Deselect the button.

            }

            // Deselect the selected button, which is either the statement button or the resource button.
            EventSystem.current.SetSelectedGameObject(null);

            // Clear the selections.
            selectedStatement = null;
            selectedResource = null;
        }

        // Update is called once per frame
        protected override void Update()
        {
            // // Grabs the current event system.
            // EventSystem eventSystem = EventSystem.current;
            // 
            // // If the event system has been set, check
            // if(eventSystem != null)
            // {
            // }

            // A statement and resource have both been selected.
            if(selectedStatement != null && selectedResource != null)
            {
                // Attaches the statement to the provided resource.
                selectedResource.AttachToStatement(selectedStatement);
                
                // Clears the selections.
                ClearSelections();
            }
            
        }
    }
}