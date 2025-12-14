using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
        public KnowledgeUI knowledgeUI = null;

        // The selected knowledge statement.
        public KnowledgeStatement selectedStatement = null;

        // The selected knowledge resource.
        public KnowledgeResource selectedResource = null;

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

        // Verifies the matches to see if they're correct.
        public bool VerifyMatches()
        {
            // If not set, try to get the instance.
            if(knowledgeUI == null)
            {
                knowledgeUI = KnowledgeUI.Instance;
            }

            // Saves if all the statements are matching correctly.
            bool allMatch = true;

            // Go through all the statements.
            if (knowledgeUI.statements.Count > 0)
            {
                // Goes through all statements to see if they all match.
                foreach(KnowledgeStatement statement in knowledgeUI.statements)
                {
                    // If the statement object is active, check it.
                    if(statement.gameObject.activeSelf)
                    {
                        // If an incorrect match is found, mark that a match failed and clear the selection.
                        if (!statement.AttachmentMatchesCorrectly())
                        {
                            allMatch = false;
                            statement.DetachResource();
                        }
                        else
                        {
                            // If the statement matches, disable the buttons since they don't need to be used.
                            statement.button.interactable = false;
                            statement.attachedResource.button.interactable = false;
                        }
                    }
                }
            }
            else
            {
                // If there are no statements, consider all of the matched.
                allMatch = true;
            }

            // Different behaviour based on if everything matches or not.
            // If all the statements match, the stage can be finished. If not, the stage cannot finish.
            if(allMatch)
            {
                knowledgeUI.finishButton.interactable = true;
            }
            else
            {
                knowledgeUI.finishButton.interactable = false;
            }
            
            return allMatch;
        }

        // Clears the selections for the knowledge manager.
        public void ClearCurrentSelection()
        {
            // The current selected game object from the event system.
            GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;

            // Checks if there's a selected statement or resource.
            // If so, remove the selection so that the button becomes deselected.
            if ((currentSelectedGameObject == selectedStatement && selectedStatement != null) ||
                (currentSelectedGameObject == selectedResource && selectedResource != null))
            {
                // Deselects the button.
                EventSystem.current.SetSelectedGameObject(null);
            }

            // Clear the selections.
            selectedStatement = null;
            selectedResource = null;
        }

        // Clears all unverified selections.
        public void ClearUnverifiedMatches()
        {
            // Goes through each statement and clears all statements that aren't verified.
            foreach(KnowledgeStatement statement in knowledgeUI.statements)
            {
                // Checks if the statement object is active.
                if(statement.gameObject.activeSelf)
                {
                    // If the button is interactable, that means it hasn't been verified as correct yet.
                    // If a statement is verified as correct, its button is rendered uninteractable.
                    if(statement.button.interactable)
                    {
                        statement.DetachResource();
                    }
                }
            }

            // Clears the current selection.
            ClearCurrentSelection();
        }


        // Finishes the stage.
        public void FinishStage()
        {
            // TODO: implement.
        }

        // Goes to the world scene.
        public void LoadWorldScene()
        {
            base.LoadWorldScene(false);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // // Grabs the current event system.
            // EventSystem eventSystem = EventSystem.current;
            // 
            // // If the event system has been set, check
            // if(eventSystem != null)
            // {
            // }

            // A statement and resource have both been selected.
            if (selectedStatement != null && selectedResource != null)
            {
                // Attaches the statement to the provided resource.
                selectedResource.AttachToStatement(selectedStatement);

                // Clears the selections.
                ClearCurrentSelection();
            }
            else
            {
                // EventSystem.currentSelectedGameObject shows the last object that was selected.
                // This object is saved until something else is selected.
                // If the player selects empty space, this is set to null.
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    // If one of the statements is selected, clear the selections, since a knowledge button...
                    // Has been deselected without connecting to anything.
                    // If one of these things is not null, but not both, clear the selections.
                    if(selectedStatement != null ^ selectedResource != null)
                    {
                        ClearCurrentSelection();
                    }
                }
            }
            
        }
    }
}