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
                new EDU_Page("Welcome to the Energy Defense Unit (EDU)! You've been tasked with completing a training simulation game that teaches you about natural resources and the EDU's defense systems. In this exercise, aliens are attacking Earth's bases to steal their resources. Your job is to defend each base from the invaders.", "trl_intro_00"),
                new EDU_Page("This simulation has two stage types: action and knowledge. In action stages you defend a base from enemies and in knowledge stages you're assessed on your knowledge of natural resources. You'll get more information on each type when you play them.", "trl_intro_01"),
                new EDU_Page("From left to right on the top are the info log button, the energy bonus display, and the options button. The info log button opens the info log, which provides information on simulation elements you've encountered. Relatedly, the options button opens the options menu, which allows you to save your progress, quit the game, and adjust the game's settings. As for the energy bonus display, that'll be explained later. With all that said, please select your first stage!", "trl_intro_02"),
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
                new EDU_Page("This is an action stage! In an action stage, you place units on the field to fight off enemies. The enemies start on the right side of the field and make their way to the left side. If an enemy makes it to the end of the field, you lose. But if the enemy side runs out of energy, you win. The enemy side loses energy automatically, but defeating an enemy makes the enemy side lose energy faster. The enemy side's energy amount is displayed using the bar on the right.", "trl_firstAction_00"),
                new EDU_Page("On the left side of the field are the lane blasters. If an enemy reaches a lane blaster, it'll go off, defeating all enemies in its lane. However, this will also destroy said lane blaster, meaning each lane blaster can only be used once per stage. If a lane blaster is destroyed, it leaves an opening for the enemies to reach the end of the field.", "trl_firstAction_01"),
                new EDU_Page("To fend off enemies, you use generator units and defense units. Generators generate energy, which is needed to create a unit. Some energy is generated regularly without generators, but not much. Relatedly, some generators create air pollution, which lowers your stage score, so keep that in mind.", "trl_firstAction_02"),
                new EDU_Page("As for defense units, they attack enemies in their lane and/or defend units from enemies. Defense units use energy to attack, so make sure you have enough energy to power them. Your units can only be placed on certain tiles, which is determined by various factors. When you select a unit, the map will show you where it can be placed.", "trl_firstAction_03"),
                new EDU_Page("From left to right on the top is the info log button, the day-night indicator, the energy display, the wind indicator, and the options button. While the stage info log is the same as the world info log, the stage options menu allows you to quit the stage, reset the stage, and adjust the game's settings. The day-night indicator shows the time of day, and the wind indicator shows the current wind speed. Finally, the energy display shows how much energy you have, and how much air pollution you've generated.", "trl_firstAction_04"),
                new EDU_Page("On the bottom are your generator units on the left side and your defense units on the right side. When you have a unit selected, it'll be displayed in the middle of the bottom area. If you have enough energy to create the selected unit, the unit will be created once you select a valid tile.", "trl_firstAction_05"),
                new EDU_Page("On the left are the stage speed button, the unit deselect button, the unit remove button, and the energy block button. The speed button switches the game between normal and fast speed. The deselect button deselects the unit you currently have selected. The remove button activates remove mode, which allows you to remove your created units by selecting them. Finally, the energy block button stops your defense units from using energy. With all that explained, time to start the stage!", "trl_firstAction_06"),
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
                new EDU_Page("You've completed your first action stage! When you complete a stage, sometimes you'll unlock new defense units, which will have their information available in the info log. Relatedly, when a stage is successfully completed, sometimes one or more new stages are unlocked. Some stages can be beaten before others, but all stages must be completed to finish the simulation.", "trl_firstActionComplete_00"),
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
                new EDU_Page("This is a knowledge stage! In knowledge stages, you must match statements with the natural resources they apply to. Once you've made connections, select the verify button to check your answers. When verification is performed, all correctly matched statements and resources are taken out of the stage, and all remaining statements are randomized to new ones. Once all the statements have been matched correctly, the stage is complete.", "trl_firstKnowledge_00"),
                new EDU_Page("The statements and resources are on separate pages, which are switched between using the arrow buttons at the top. The panel at the bottom shows what you currently have selected. Also on the top are the info log button and the options button. The info log and options menu are the same as they are in action stages. However, if you open the info log, your remaining statements will be randomized to new ones. The info log will also be locked once you close it, but it will be unlocked again once you perform a verification check.", "trl_firstKnowledge_01"),
                new EDU_Page("You can get a starting energy bonus for your next action stage based on how many verifications it took to complete your most recent knowledge stages. The more verifications you perform, the smaller the bonus, and doing too many verifications stops you from receiving any energy bonus for your current knowledge stage. With all that explained, time to start the stage!", "trl_firstKnowledge_02"),
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
                new EDU_Page("You've completed your first knowledge stage! If you got an energy bonus, it'll be displayed at the top of the screen. As mentioned, the starting energy bonus will be applied in your next action stage.", "trl_firstKnowledgeComplete_00"),
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
                new EDU_Page("You've cleared the first area! Once all the stages in an area have been completed, you can move onto the next area. Switch areas using the on-screen arrow buttons.", "trl_firstAreaComplete_00"),
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
                new EDU_Page("This is the final area! Once you clear all the stages in this area, the simulation game is complete. Good luck!", "trl_finalArea_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedFinalAreaTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }


        // Natural Resources //
        // Loads the biomass tutorial.
        public void LoadBiomassTutorial(bool startTutorial = true)
        {
            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page("This stage uses the renewable resource biomass. Biomass entails burning plant and/or waste materials to spin a turbine to generate energy. For the biomass generator, it can be placed anywhere on land, generates energy regularly, and produces no air pollution.", "trl_biomass_00"),
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
                new EDU_Page("This stage uses the renewable resource geothermal. Geothermal power uses the Earth's internal heat to produce hot water and/or steam to spin a turbine to generate energy. The geothermal generator can only be placed on geothermal spots and produces no air pollution.", "trl_geothermal_00"),
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
                new EDU_Page("This stage uses the renewable resource hydro. Hydropower uses the flow of water to turn a turbine to generate energy. Hydro generators can only be placed on rivers and cannot be placed next to certain generator types. Hydro generators produce small amounts of pollution and will flood the areas behind them if they run for too long.", "trl_hydro_00"),
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
                new EDU_Page("This stage uses the renewable resource solar. Solar power entails using solar panels to convert sunlight into electricity, which can only be done during the day. Solar generators can be placed anywhere on land and produce no air pollution but only generate energy during the daytime.", "trl_solar_00"),
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
                new EDU_Page("This stage uses the renewable resource wave. Wave power uses the movement of waves to turn turbines to generate energy. High wind areas have the most potential for using wave energy, and no energy is generated if there's no waves. The wave generators can only be placed in sea tiles, generate energy based on the wind speed, and produce no air pollution.", "trl_wave_00"),
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
                new EDU_Page("This stage uses the renewable resource wind. Wind is used to spin wind turbines to generate energy. Wind turbines can be placed on land or offshore and produce no electricity when there's no wind. Wind generators can be placed on land or in water tiles close to land and create no air pollution. The amount of energy a wind turbine generates is based on the wind speed.", "trl_wind_00"),
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
                new EDU_Page("This stage uses coal, which is a non-renewable resource that's fed to a furnace to heat water. This water then produces steam, which is used to spin a turbine to generate energy. Coal produces toxic chemicals when burned and can release toxic minerals if mined on the surface. Coal generators can only be placed on coal spots, generate energy for a limited time, and create air pollution.", "trl_coal_00"),
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
                new EDU_Page("This stage uses the non-renewable resource natural gas, which is burned to produce gases to spin a turbine to generate energy. Natural gas is often found with coal or oil, is highly flammable, must have its poisonous elements removed before being burned, and releases pollutants when burned. Natural gas generators generate energy for a limited time, produce air pollution, and can be placed on natural gas, oil, or coal spots.", "trl_naturalGas_00"),
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
                new EDU_Page("This stage uses the non-renewable resource nuclear. Nuclear power involves splitting uranium atoms to produce heat to boil water, which then produces steam to spin a turbine to generate energy. This reaction produces no pollution, but it creates radioactive waste, which must be stored away to prevent harm to people and/or the environment.", "trl_nuclear_00"),
                new EDU_Page("Nuclear generators can only be placed on nuclear spots, generate energy for a limited time, and produce no air pollution. Nuclear generators are automatically destroyed upon depleting their resources, but if destroyed by an enemy, they leave nuclear waste. No units can be placed on a tile that has nuclear waste.", "trl_nuclear_01"),
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
                new EDU_Page("This stage uses the non-renewable resource oil, which is burned to produce steam to turn a turbine to generate energy. Oil can be mined on land or at sea, but drilling for oil in the ocean can cause an oil spill if done improperly. Oil generators can only be placed on oil spots, generate energy for a limited time, and cause an oil spill if an enemy destroys them before they're finished. No units can be placed on a spot with an oil spill.", "trl_oil_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedOilTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

    }
}