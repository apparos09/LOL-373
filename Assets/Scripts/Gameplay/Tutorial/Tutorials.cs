using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace RM_EDU
{
    // The script for the tutorials.
    public class Tutorials : MonoBehaviour
    {
        [System.Serializable]
        public class TutorialsData
        {
            public bool clearedIntroTutorial;
            public bool clearedFirstActionStageTutorial;
            public bool clearedFirstKnowledgeStageTutorial;
            public bool clearedFirstStageCompletedTutorial;


            // TODO: Add
        }

        // The tutorial types 
        // TODO: remove.
        // public enum tutorialType
        // {
        //     none, intro, stage, firstWin, mixStage, weightImperial, lengthImperial, time, lengthMetric, weightMetric, capacity
        // };


        // The tutorial type count. TODO: finalize.
        public const int TUTORIAL_TYPE_COUNT = 15;

        // The singleton instance.
        private static Tutorials instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The game manager.
        public GameplayManager gameManager;

        // The tutorials UI.
        public TutorialUI tutorialsUI;

        // // If 'true', the tutorials object constantly checks for starting tutorials.
        // [Tooltip("Constant check for tutorial start.")]
        // public bool constantTutorialStartCheck = true;


        [Header("Tutorials")]

        public bool clearedIntroTutorial;
        public bool clearedFirstActionStageTutorial;
        public bool clearedFirstKnowledgeStageTutorial;
        public bool clearedFirstStageCompletedTutorial;

        // Constructor
        private Tutorials()
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
        void Start()
        {
            // Gets the game manager object if it isn't set.
            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameplayManager>();
            }

            // Gets the tutorials UI object if it exists.
            // Notably, TutorialUI checks for Tutorials as well, and will save itself...
            // To that object if this check fails.
            if (tutorialsUI == null && TutorialUI.Instantiated)
            {
                tutorialsUI = TutorialUI.Instance;
            }   

            // Don't destroy this game object.
            DontDestroyOnLoad(gameObject);
        }


        // This function is called when the object is enabled and active
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // Gets the instance.
        public static Tutorials Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<Tutorials>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Tutorial (singleton)");
                        instance = go.AddComponent<Tutorials>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Called when the scene is loaded.
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // If the game manager is not set, set it.
            if (gameManager == null)
            {
                // Try to find the game manager.
                gameManager = FindObjectOfType<GameplayManager>();
            }

            // Try to get the tutorials UI again if it has been instantiated.
            // Since the tutorial object might be going into a scene that will end up destroying it...
            // No message is printed to show that there's no TutorialUI object if TutorialUI...
            // Hasn't been instantiated.
            if (tutorialsUI == null && TutorialUI.Instantiated)
            {
                tutorialsUI = TutorialUI.Instance;
            }
        }

        // Checks if a tutorial is running.
        public bool IsTutorialRunning()
        {
            return tutorialsUI.IsTutorialRunning();
        }

        // Starts the tutorial.
        public void StartTutorial()
        {
            tutorialsUI.StartTutorial();
        }

        // Restarts the tutorial.
        public void RestartTutorial()
        {
            tutorialsUI.RestartTutorial();
        }

        // Ends the tutorial.
        public void EndTutorial()
        {
            tutorialsUI.EndTutorial();
        }

        // Called when a tutorial is started.
        public void OnTutorialStart()
        {
            // UI start function.
            tutorialsUI.OnTutorialStart();

            // Freeze the game.
            Time.timeScale = 0.0F;
        }

        // Called when a tutorial ends.
        public void OnTutorialEnd()
        {
            // UI end function.
            tutorialsUI.OnTutorialEnd();

            // Unfreeze the game if the game is not paused.
            if (!gameManager.IsGamePaused())
            {
                // If the game manager is set, check it for the time scale.
                // If it's not set, use 1.0F.
                if (gameManager != null)
                {
                    Time.timeScale = gameManager.GetGameTimeScale();
                }
                else
                {
                    Time.timeScale = 1.0F;
                }

            }

            // Ignore the current input for this frame in case the player is holding the space bar.
            // gameManager.player.IgnoreInputs(1);
        }


        // TUTORIAL DATA
        // Generates the tutorials data.
        public TutorialsData GenerateTutorialsData()
        {
            TutorialsData data = new TutorialsData();

            data.clearedIntroTutorial = clearedIntroTutorial;
            data.clearedFirstActionStageTutorial = clearedFirstActionStageTutorial;
            data.clearedFirstKnowledgeStageTutorial = clearedFirstKnowledgeStageTutorial;
            data.clearedFirstStageCompletedTutorial = clearedFirstStageCompletedTutorial;

            return data;
        }

        // Sets the tutorials data.
        public void LoadTutorialsData(TutorialsData data)
        {
            clearedIntroTutorial = data.clearedIntroTutorial;
            clearedFirstActionStageTutorial = data.clearedFirstActionStageTutorial;
            clearedFirstKnowledgeStageTutorial = data.clearedFirstKnowledgeStageTutorial;
            clearedFirstStageCompletedTutorial = data.clearedFirstStageCompletedTutorial;
        }


        // Tutorial Loader

        // Loads the tutorial
        private void LoadTutorial(ref List<Page> pages, bool startTutorial = true)
        {
            // The gameplay manager isn't set, try to find it.
            if (gameManager == null)
                gameManager = FindObjectOfType<GameplayManager>();

            // Loads pages for the tutorial.
            if (gameManager != null && startTutorial) // If the game manager is set, start the tutorial.
            {
                gameManager.StartTutorial(pages);
            }
            else // No game manager, so just load the pages.
            {
                tutorialsUI.LoadPages(ref pages, false);
            }
        }

        // // Loads the tutorial of the provided type.
        // public void LoadTutorial(tutorialType tutorial)
        // {
        //     // ...
        // }


        // Load the tutorial (template)
        private void LoadTutorialTemplate(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new Page("Insert text here.")
            };

            // Change the display image when certain pages are opened using callbacks.

            // Loads the tutorial.
            LoadTutorial(ref pages, startTutorial);
        }

        // Load test tutorial
        public void LoadTutorialTest(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("This is a test."),
                new EDU_Page("This is only a test.")
            };

            // Change the display image when certain pages are opened using callbacks.

            // Loads the tutorial.
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the intro tutorial.
        public void LoadIntroTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("Welcome to the Meteor Strike Team (MST)! We track down meteors and destroy them before they hit the Earth's surface, which we do by converting larger measurement units into smaller measurement units. This is the world area, which is where you select stages, view unit information, change the game settings, and save your game. I'm Reteor...", "trl_intro_00"),
                new EDU_Page("And I'm Astrite! When you select a stage, the units you'll be working with will be displayed. Once this information is given, it's added to the 'units info menu', which can be viewed using the 'units info button'. With all that explained, please select the available stage to start destroying meteors!", "trl_intro_01"),
            };

            // Sets the bool and loads the tutorial.
            clearedIntroTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}