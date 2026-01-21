using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // The gameplay UI.
    public class GameplayUI : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager;

        // The tutorial UI.
        public TutorialUI tutorialUI;

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // ...
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the game manager isn't set, try to find it.
            if (gameManager == null)
                gameManager = FindObjectOfType<GameplayManager>();

            // If the tutorial UI isn't set, try to find it.
            if(tutorialUI == null)
                tutorialUI = FindObjectOfType<TutorialUI>();
        }

        // TUTORIAL //
        // Start tutorial
        public void StartTutorial(List<Page> pages)
        {
            // Loads the pages, sets the index to 0, and closes the textbox.
            if (tutorialUI.textBox.IsVisible())
            {
                // Close the textbox.
                tutorialUI.textBox.Close();
            }

            // Load the pages.
            tutorialUI.LoadPages(ref pages, true);
            tutorialUI.textBox.CurrentPageIndex = 0;
            tutorialUI.textBox.Open();
        }

        // On Tutorial Start
        public virtual void OnTutorialStart()
        {
            // ...
        }

        // On Tutorial End
        public virtual void OnTutorialEnd()
        {
            // ...
        }

        // Checks if the tutorial text box is open.
        public bool IsTutorialTextBoxOpen()
        {
            // Checks if it's visible normally, and in the hierachy.
            return tutorialUI.textBox.IsVisible() && tutorialUI.textBox.IsVisibleInHierachy();
        }

        // Returns 'true' if the tutorial can be started.
        public bool IsTutorialAvailable()
        {
            return !IsTutorialTextBoxOpen();
        }

        // Checks if the tutorial is running.
        public bool IsTutorialRunning()
        {
            // Checks if the tutorial is instantiated.
            if (Tutorials.Instantiated)
            {
                // Checks if the tutorial UI is not set to null.
                if (tutorialUI != null)
                {
                    // The tutorials object has a dedicated function for seeing if it's running...
                    // You don't know why you did a seperate set up for this, but you're leaving it as is.
                    return IsTutorialTextBoxOpen();
                }
                else // No tutorial UI, so the tutorial cannot run.
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        // Adds the tutorial text box open/close callbacks.
        public void AddTutorialTextBoxCallbacks(GameplayManager manager)
        {
            tutorialUI.textBox.OnTextBoxOpenedAddCallback(manager.OnTutorialStart);
            tutorialUI.textBox.OnTextBoxClosedAddCallback(manager.OnTutorialEnd);
        }

        // Removes the tutorial text box open/close callbacks.
        public void RemoveTutorialTextBoxCallbacks(GameplayManager manager)
        {
            tutorialUI.textBox.OnTextBoxOpenedRemoveCallback(manager.OnTutorialStart);
            tutorialUI.textBox.OnTextBoxClosedRemoveCallback(manager.OnTutorialEnd);
        }

        // SCENES //
        // Goes to the title scene
        public void LoadTitleScene()
        {
            gameManager.LoadTitleScene();
        }

        // Goes to the world scene.
        public void LoadWorldScene()
        {
            gameManager.LoadWorldScene();
        }

        // Goes to the action scene.
        public void LoadActionScene()
        {
            gameManager.LoadActionScene();
        }

        // Goes to the knowledge scene.
        public void LoadKnowledgeScene()
        {
            gameManager.LoadKnowledgeScene();
        }

        // Loads the results scene.
        public void LoadResultsScene()
        {
            gameManager.LoadResultsScene();
        }

        // Quits the game.
        public void QuitGame()
        {
            gameManager.QuitGame();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // ...
        }
    }
}
