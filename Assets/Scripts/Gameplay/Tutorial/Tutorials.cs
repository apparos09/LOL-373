using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace RM_EDU
{
    // The script for the tutorials.
    public class Tutorials : MonoBehaviour
    {
        // The tutorial data.
        [System.Serializable]
        public class TutorialsData
        {
            // Main
            public bool clearedIntroTutorial = false;
            public bool clearedFirstActionTutorial = false;
            public bool clearedFirstActionCompleteTutorial = false;
            public bool clearedFirstKnowledgeTutorial = false;
            public bool clearedFirstKnowledgeCompleteTutorial = false;
            public bool clearedFirstAreaCompleteTutorial = false;
            public bool clearedFinalAreaTutorial = false;

            // Natural Resources
            public bool clearedBiomassTutorial = false;
            public bool clearedGeothermalTutorial = false;
            public bool clearedHydroTutorial = false;
            public bool clearedSolarTutorial = false;
            public bool clearedWaveTutorial = false;
            public bool clearedWindTutorial = false;

            public bool clearedCoalTutorial = false;
            public bool clearedNaturalGasTutorial = false;
            public bool clearedNuclearTutorial = false;
            public bool clearedOilTutorial = false;

            // Copies the tutorial data.
            public TutorialsData Copy()
            {
                // The copied data.
                TutorialsData copyData = new TutorialsData();

                // Main
                copyData.clearedIntroTutorial = clearedIntroTutorial;
                copyData.clearedFirstActionTutorial = clearedFirstActionTutorial;
                copyData.clearedFirstActionCompleteTutorial = clearedFirstActionCompleteTutorial;
                copyData.clearedFirstKnowledgeTutorial = clearedFirstKnowledgeTutorial;
                copyData.clearedFirstKnowledgeCompleteTutorial = clearedFirstKnowledgeCompleteTutorial;
                copyData.clearedFirstAreaCompleteTutorial = clearedFirstAreaCompleteTutorial;
                copyData.clearedFinalAreaTutorial = clearedFinalAreaTutorial;

                // Natural Resources
                copyData.clearedBiomassTutorial = clearedBiomassTutorial;
                copyData.clearedGeothermalTutorial = clearedGeothermalTutorial;
                copyData.clearedHydroTutorial = clearedHydroTutorial;
                copyData.clearedSolarTutorial = clearedSolarTutorial;
                copyData.clearedWaveTutorial = clearedWaveTutorial;
                copyData.clearedWindTutorial = clearedWindTutorial;

                copyData.clearedCoalTutorial = clearedCoalTutorial;
                copyData.clearedNaturalGasTutorial = clearedNaturalGasTutorial;
                copyData.clearedNuclearTutorial = clearedNuclearTutorial;
                copyData.clearedOilTutorial = clearedOilTutorial;

                // Resulting data.
                return copyData;
            }

            // Pastes the values from the provided data to this object.
            public void Paste(TutorialsData pasteData)
            {
                // Main
                clearedIntroTutorial = pasteData.clearedIntroTutorial;
                clearedFirstActionTutorial = pasteData.clearedFirstActionTutorial;
                clearedFirstActionCompleteTutorial = pasteData.clearedFirstActionCompleteTutorial;
                clearedFirstKnowledgeTutorial = pasteData.clearedFirstKnowledgeTutorial;
                clearedFirstKnowledgeCompleteTutorial = pasteData.clearedFirstKnowledgeCompleteTutorial;
                clearedFirstAreaCompleteTutorial = pasteData.clearedFirstAreaCompleteTutorial;
                clearedFinalAreaTutorial = pasteData.clearedFinalAreaTutorial;

                // Natural Resources
                clearedBiomassTutorial = pasteData.clearedBiomassTutorial;
                clearedGeothermalTutorial = pasteData.clearedGeothermalTutorial;
                clearedHydroTutorial = pasteData.clearedHydroTutorial;
                clearedSolarTutorial = pasteData.clearedSolarTutorial;
                clearedWaveTutorial = pasteData.clearedWaveTutorial;
                clearedWindTutorial = pasteData.clearedWindTutorial;

                clearedCoalTutorial = pasteData.clearedCoalTutorial;
                clearedNaturalGasTutorial = pasteData.clearedNaturalGasTutorial;
                clearedNuclearTutorial = pasteData.clearedNuclearTutorial;
                clearedOilTutorial = pasteData.clearedOilTutorial;
            }

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

        // If 'true', the tutorials object is set to not be destroyed on load in the Start().
        [Tooltip("If true, DontDestroyOnLoad(gameObject) is called in Start().")]
        public bool dontDestroyOnLoadInStart = true;

        // // If 'true', the tutorials object constantly checks for starting tutorials.
        // [Tooltip("Constant check for tutorial start.")]
        // public bool constantTutorialStartCheck = true;


        [Header("Tutorials")]

        // The tutorial data.
        private TutorialsData data = new TutorialsData();

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
                // If the data doesn't exist, create an object.
                if(data == null)
                    data = new TutorialsData();

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
                // The tutorial UI.
                tutorialsUI = TutorialUI.Instance;

                // Set the UI to use this tutorial.
                if (tutorialsUI.tutorials == null)
                    tutorialsUI.tutorials = this;
            }   

            // Don't destroy this game object.
            if(dontDestroyOnLoadInStart)
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

        // Gets the tutorials data.
        public TutorialsData Data
        {
            get { return data; }
        }

        // TUTORIAL FUNCTIONS
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

            // // If the game manager hasn't been set, try to find it.
            if (gameManager == null)
                gameManager = FindObjectOfType<GameplayManager>();

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
        // Generates a copy of the tutorials data.
        public TutorialsData GenerateTutorialsDataCopy()
        {
            // The data.
            TutorialsData copy = data.Copy();

            return data;
        }

        // Sets the tutorials data.
        public void LoadTutorialsData(TutorialsData newData)
        {
            // If the data doesn't exist, create it.
            if (data == null)
                data = new TutorialsData();

            data.Paste(newData);
        }

        // Loads the tutorial debug data.
        public void LoadTutorialsDebugData()
        {
            // The debug data.
            TutorialsData debugData = new TutorialsData();

            // Load the debug data.
            LoadTutorialsData(debugData);
        }

        // Resets the tutorials data.
        public void ResetTutorialsData()
        {
            TutorialsData newData = new TutorialsData();
            data.Paste(newData);
        }

        // TUTORIAL LOADING

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


        // MAIN
        // Loads the intro tutorial.
        public void LoadIntroTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("Welcome to the Energy Defense Unit (EDU)! For this training exercise, you'll be doing a simulation where you defend Earth's bases from alien invaders! The aliens are here to take Earth's natural resources, so you must use said resources to defend Earth. Once the aliens are fended off at every base, the simulation is complete.", "trl_intro_00"),
                new EDU_Page("The game is split two stage types: action and knowledge. Action stages are where you fight off the aliens, and knowledge stages are where you're evaluated on your natural resource knowledge. Some stages can be completed before others, but all stages must be cleared to complete the simulation.", "trl_intro_01"),
                new EDU_Page("From left to right on the top are the info log button, the energy bonus display, and the options button. The info log allows you to lookup information on natural resources, generators, and defenses that you've encountered. The energy bonus display shows how much bonus starting energy you have, which is applied in action stages. Finally, the options button opens the options menu, which allows you to adjust the game's settings, save your game, and return to the title screen.", "trl_intro_02"),
                new EDU_Page("More simulation elements will be explained as they become relevant. With all that said, please select your first stage!", "trl_intro_03"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedIntroTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first action tutorial.
        public void LoadFirstActionTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("This is an action stage! In an action stage, you place units on the field to fight off enemies. The enemies start on the right side of the field and make their way to the left side. If an enemy makes it to the end of the field, you lose. But if the enemy side runs out of power, you win. The enemy side loses power automatically, but defeating an enemy makes the enemy side lose power faster. The enemy's energy amount is displayed with the bar on the right.", "trl_firstAction_00"),
                new EDU_Page("On the left side of the field are the lane blasters. If an enemy reaches a lane blaster, it'll go off, destroying all enemies in its lane. However, this will also destroy said lane blaster, meaning each lane blaster can only be used once per stage. If a lane blaster is destroyed, it leaves an opening for the enemies to reach the end of the field.", "trl_firstAction_01"),
                new EDU_Page("To fight off enemies, you use generator units and defense units. Generators generate energy, which is needed to create a unit. Some energy is generated regularly without generators, but not much. Relatedly, some generators create air pollution, which lowers your stage score, so keep that in mind.", "trl_firstAction_02"),
                new EDU_Page("As for defense units, they attack enemies in their lane and/or defend units in their lane. Defense units use energy to attack, so make sure you have enough energy to power them. Your units can only be placed on certain tiles, which is determined by various factors. When you select a unit, the map will show you where it can be placed.", "trl_firstAction_03"),
                new EDU_Page("On the top is the info log button, the day-night indicator, your energy points, the wind indicator, and the options button. The info log button opens the info log, which gives you information on the units that you've encountered. The day-night indicator shows the time of day, the energy points display shows how much energy you have, and the wind indicator shows the current wind speed. The options button opens the options menu, which allows you to change the game's settings, reset the stage, and return to the world area.", "trl_firstAction_04"),
                new EDU_Page("On the bottom are your generator units on the left side and your defense units on the right side. When you select a unit, it will show in the middle of the bottom area. If you have enough energy to create the unit, the selected unit will be created when you select a valid tile.", "trl_firstAction_05"),
                new EDU_Page("To the left are the stage speed button, the unit deselect button, the unit remove button, and the energy block button. The speed button switches the game between normal and fast speed. The deselect button deselects the unit you currently have selected. The remove button activates remove mode, which allows you to remove any of your created units. Finally, the energy block button stops your defense units from using energy. With all this explained, time to start the stage!", "trl_firstAction_06"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first action complete tutorial.
        public void LoadFirstActionCompleteTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("You've completed the first action stage! When you unlock an action stage, you'll sometimes unlock new defense units. To see what defense units you have and what they do, open the info log.", "trl_firstActionComplete_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionCompleteTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first knowledge tutorial.
        public void LoadFirstKnowledgeTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("...", "trl_firstKnowledge_00"),
                new EDU_Page("...", "trl_firstKnowledge_01"),
                new EDU_Page("...", "trl_firstKnowledge_02"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedFirstKnowledgeTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first knowledge complete tutorial.
        public void LoadFirstKnowledgeCompleteTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("...", "trl_firstKnowledgeComplete_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedFirstKnowledgeCompleteTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first area complete tutorial.
        public void LoadFirstAreaCompleteTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("...", "trl_firstAreaComplete_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedFirstAreaCompleteTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the final area start tutorial.
        public void LoadFinalAreaTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("...", "trl_finalAreaStart_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedFinalAreaTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }


        // Natural Resources
        // Loads the biomass tutorial.
        public void LoadBiomassTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("...", "trl_biomass_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedBiomassTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the geothermal tutorial.
        public void LoadGeothermalTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("...", "trl_geothermal_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedGeothermalTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the hydro tutorial.
        public void LoadHydroTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("...", "trl_hydro_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedHydroTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the solar tutorial.
        public void LoadSolarTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("...", "trl_solar_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedSolarTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the wave tutorial.
        public void LoadWaveTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("...", "trl_wave_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedWaveTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the wind tutorial.
        public void LoadWindTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("...", "trl_wind_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedWindTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the coal tutorial.
        public void LoadCoalTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("...", "trl_coal_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedCoalTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the naturalgas tutorial.
        public void LoadNaturalGasTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("...", "trl_naturalGas_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedNaturalGasTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the nuclear tutorial.
        public void LoadNuclearTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("...", "trl_nuclear_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedNuclearTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the oil tutorial.
        public void LoadOilTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("...", "trl_oil_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedOilTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

    }
}