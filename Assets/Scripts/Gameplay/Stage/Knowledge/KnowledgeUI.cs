using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The UI for the knowledge stage.
    public class KnowledgeUI : StageUI
    {
        // The singleton instance.
        private static KnowledgeUI instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("Knowledge")]

        // Manager
        public KnowledgeManager knowledgeManager;

        // The statements in the knowledge stage.
        public List<KnowledgeStatement> statements = new List<KnowledgeStatement>();

        // The resources in the knowledge stage.
        public List<KnowledgeResource> resources = new List<KnowledgeResource>();

        // The knowledge stage end dialog.
        public KnowledgeStageEndDialog stageEndDialog;

        // The butto used to verify the statements.
        public Button verifyButton;

        // The finish button.
        // NOTE: this is part of the stage end dialog window, so saving the button here...
        // Isn't really needed anymore, but it's already integrated into the code.
        public Button finishButton;

        // Constructor
        private KnowledgeUI()
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

            // Set the knowledge manager.
            if(knowledgeManager == null)
            {
                knowledgeManager = KnowledgeManager.Instance;
            }

            // If the statements list is empty, fill the list automatically.
            if (statements.Count == 0)
            {
                statements = new List<KnowledgeStatement>(FindObjectsOfType<KnowledgeStatement>(true));
            }

            // If the resources list is empty, fill the list automatically.
            if (resources.Count == 0)
            {
                resources = new List<KnowledgeResource>(FindObjectsOfType<KnowledgeResource>(true));
            }

            // The finish button only becomes interactable once all elements have been matched correctly.
            finishButton.interactable = false;
        }

        // Gets the instance.
        public static KnowledgeUI Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<KnowledgeUI>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Knowledge UI (singleton)");
                        instance = go.AddComponent<KnowledgeUI>();
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

        // Verifies the matches.
        public void VerifyMatches()
        {
            knowledgeManager.VerifyMatches();
        }

        // Clears all unverified matches.
        public void ClearUnverifiedMatches()
        {
            knowledgeManager.ClearUnverifiedMatches();
        }

        // DIALOGS //
        // Generates a list of dialogs.
        public override List<GameObject> GenerateDialogList()
        {
            // Gets the base list.
            List<GameObject> dialogList = base.GenerateDialogList();

            dialogList.Add(stageEndDialog.gameObject);

            return dialogList;
        }

        // Opens the stage end dialog.
        public void OpenStageEndDialog()
        {
            OpenDialog(stageEndDialog.gameObject, true);
            stageEndDialog.UpdateStageEndStats();
        }

        // Closes the stage end dialog.
        public void CloseStageEndDialog()
        {
            CloseDialog(stageEndDialog.gameObject);
        }

        // Resets the stage.
        // This function shouldn't be used because the verified correct matches shouldn't be undone...
        // Once the matches have been done correctly.
        public override void ResetStage()
        {
            knowledgeManager.ResetStage();
            CloseAllDialogs();
        }

        // Finishes the stage. This should only be called once all statements have been matched correctly.
        public override void FinishStage()
        {
            knowledgeManager.FinishStage();
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