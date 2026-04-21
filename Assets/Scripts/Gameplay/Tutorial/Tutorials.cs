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
            // World - 1
            public bool clearedIntroTutorial = false;

            // Action
            public bool clearedFirstActionIntroTutorial = false;
            public bool clearedFirstActionGeneratorsTutorial = false;
            public bool clearedFirstActionDefensesTutorial = false;
            public bool clearedFirstActionFirstKillTutorial = false;

            // Action Complete (World)
            public bool clearedFirstActionCompleteTutorial = false;

            // Knowledge 
            public bool clearedFirstKnowledgeIntroTutorial = false;
            public bool clearedFirstKnowledgeVerifyTutorial = false;

            // Knowledge Complete (World)
            public bool clearedFirstKnowledgeCompleteTutorial = false;

            // World - 2
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

                // World - 1
                copyData.clearedIntroTutorial = clearedIntroTutorial;

                // Action
                copyData.clearedFirstActionIntroTutorial = clearedFirstActionIntroTutorial;
                copyData.clearedFirstActionGeneratorsTutorial = clearedFirstActionGeneratorsTutorial;
                copyData.clearedFirstActionDefensesTutorial = clearedFirstActionDefensesTutorial;
                copyData.clearedFirstActionFirstKillTutorial = clearedFirstActionFirstKillTutorial;

                // Action - Complete (World)
                copyData.clearedFirstActionCompleteTutorial = clearedFirstActionCompleteTutorial;

                // Knowledge
                copyData.clearedFirstKnowledgeIntroTutorial = clearedFirstKnowledgeIntroTutorial;
                copyData.clearedFirstKnowledgeVerifyTutorial = clearedFirstKnowledgeVerifyTutorial;

                // Knowledge - Complete (World)
                copyData.clearedFirstKnowledgeCompleteTutorial = clearedFirstKnowledgeCompleteTutorial;

                // World - 2
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
                // World - 1
                clearedIntroTutorial = pasteData.clearedIntroTutorial;

                // Action
                clearedFirstActionIntroTutorial = pasteData.clearedFirstActionIntroTutorial;
                clearedFirstActionGeneratorsTutorial = pasteData.clearedFirstActionGeneratorsTutorial;
                clearedFirstActionDefensesTutorial = pasteData.clearedFirstActionDefensesTutorial;
                clearedFirstActionFirstKillTutorial = pasteData.clearedFirstActionFirstKillTutorial;

                // Action - Complete
                clearedFirstActionCompleteTutorial = pasteData.clearedFirstActionCompleteTutorial;

                // Knowledge
                clearedFirstKnowledgeIntroTutorial = pasteData.clearedFirstKnowledgeIntroTutorial;
                clearedFirstKnowledgeVerifyTutorial = pasteData.clearedFirstKnowledgeVerifyTutorial;

                // Knowledge - Complete
                clearedFirstKnowledgeCompleteTutorial = pasteData.clearedFirstKnowledgeCompleteTutorial;

                // World - 2
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

            // You can set an image to go along with a page by providing it to EDU_Page constructor.

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
            // The title and title translation key.
            string title = "Game Introduction";
            string titleKey = "trl_intro_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "Welcome to the Energy Defense Unit (EDU)! You've been tasked with completing a training simulation game that teaches you about natural resources and the EDU's defense systems. In this exercise, aliens are attacking Earth's bases to steal their resources. Your job is to defend each base from the invaders.", "trl_intro_txt_00"),
                new EDU_Page(title, titleKey, "This simulation has two stage types: action and knowledge. In action stages you defend a base from enemies and in knowledge stages you're assessed on your knowledge of natural resources. You'll be given more details on both stage types later.", "trl_intro_txt_01", tutorialsUI.textBox.stageTypesSprite),
                new EDU_Page(title, titleKey, "In the top left is the info log button, which opens the info log. The info log provides information on simulation elements you've encountered.", "trl_intro_txt_02", tutorialsUI.textBox.logButtonSprite),
                new EDU_Page(title, titleKey, "In the top right is the options button, which opens the options menu. The options menu allows you to save your progress, quit the game, and adjust the game's settings.", "trl_intro_txt_03", tutorialsUI.textBox.optionsButtonSprite),
                new EDU_Page(title, titleKey, "In the top middle is the energy start bonus display, but that'll be explained later.", "trl_intro_txt_04"),
                new EDU_Page(title, titleKey, "With all that covered, it's time to select a stage. When a stage is selected, it'll show what natural resources it uses. Details on natural resources will be provided when relevant. Please select the available stage, which is an action stage.", "trl_intro_txt_05"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedIntroTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first action stage - intro tutorial.
        public void LoadFirstActionIntroTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "First Action Stage - Introduction";
            string titleKey = "trl_firstActionIntro_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This is an action stage! In an action stage, you place units on the field to fight off enemies. The enemies start on the right side of the field and make their way to the left side. If an enemy makes it to the left end of the field, you lose. But if the enemy side runs out of energy, you win.", "trl_firstActionIntro_txt_00"),
                new EDU_Page(title, titleKey, "The enemy side loses energy automatically, but defeating an enemy makes the enemy side lose energy faster. The enemy side's energy amount is displayed by the bar on the right.", "trl_firstActionIntro_txt_01"),
                new EDU_Page(title, titleKey, "On the left side of the field are lane blasters. If an enemy touches a lane blaster, the lane blaster will go off, destroying itself and all enemies in its lane. Once a lane blaster is destroyed, it leaves an opening for enemies to reach the end of the field. You cannot restore a lane blaster, but you always get your lane blasters back at the start of a stage.", "trl_firstActionIntro_txt_02"),
                new EDU_Page(title, titleKey, "To fend off enemies, you place units on the field, which requires energy. The energy needed to create a unit is displayed on its unit button. Some energy is generated automatically, but you'll need to place generator units on the field to generate more energy. Your generator units can be found in the bottom left.", "trl_firstActionIntro_txt_03"),
                new EDU_Page(title, titleKey, "When you have a unit selected, it will be displayed in the bottom middle, and spots on the field will be highlighted to show where it can go. Two units cannot share the same space, and some units can only use certain spaces. With all that explained, please place a generator unit on the field.", "trl_firstActionIntro_txt_04"),
            };

            // Add callback for the action manager.
            if(ActionManager.Instantiated)
            {
                pages[0].OnPageOpenedAddCallback(ActionManager.Instance.OnFirstActionIntroTutorialStart);
            }

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionIntroTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first action stage - generators tutorial.
        public void LoadFirstActionGeneratorsTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "First Action Stage - Generators";
            string titleKey = "trl_firstActionGenerators_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "You've placed a generator! Generators generate energy if their conditions are met, and some generators only generate energy for a limited time. A generator flashes blue when it generates energy and goes dark if it cannot generate energy. Some generators also produce air pollution, which can lower your stage score. The generators that you're able to use will vary by stage.", "trl_firstActionGenerators_txt_00"),
                new EDU_Page(title, titleKey, "In the top left is the info log button, which opens the info log. The info log is the same as it is in the world area. If you ever need information on what your units do, check the info log.", "trl_firstActionGenerators_txt_01", tutorialsUI.textBox.logButtonSprite),
                new EDU_Page(title, titleKey, "Also on the top left is the day-night indicator, which shows the time of day. As mentioned, solar power can only be generated during the day.", "trl_firstActionGenerators_txt_02", tutorialsUI.textBox.dayNightIndicatorSprite),
                new EDU_Page(title, titleKey, "In the top right is the options button, which opens the options menu. The options menu allows you to quit the stage, reset the stage, and adjust the game's settings.", "trl_firstActionGenerators_txt_03", tutorialsUI.textBox.optionsButtonSprite),
                new EDU_Page(title, titleKey, "Also on the top right is the wind indicator, which shows the current wind speed. Generators that are affected by wind speed will be explained later.", "trl_firstActionGenerators_txt_04", tutorialsUI.textBox.windIndicatorSprite),
                new EDU_Page(title, titleKey, "In the top middle is the energy display (top value) and air pollution display (bottom value). The energy display shows the current amount of energy you have, and the air pollution display shows how much air pollution you've generated in the current stage.", "trl_firstActionGenerators_txt_05"),
                new EDU_Page(title, titleKey, "With all that covered, let's continue with the stage.", "trl_firstActionGenerators_txt_06"),
            };

            // Add callback for the action manager.
            if (ActionManager.Instantiated)
            {
                pages[0].OnPageOpenedAddCallback(ActionManager.Instance.OnFirstActionGeneratorsTutorialStart);
            }

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionGeneratorsTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first action stage - defenses tutorial.
        public void LoadFirstActionDefensesTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "First Action Stage - Defenses";
            string titleKey = "trl_firstActionDefenses_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "Enemies are approaching! You'll need to use defense units to defeat them. Your defense units can be found in the bottom right. Defense units come in three types: blaster, shield, and trap.", "trl_firstActionDefenses_txt_00", tutorialsUI.textBox.actionDefenseTypesSprite),
                new EDU_Page(title, titleKey, "Blasters (BSRs) use energy to fire projectiles at enemies, shields (SHDs) block enemies, and traps (TRPs) attack enemies that make contact with them. You'll unlock more defense units naturally as the simulation progresses.", "trl_firstActionDefenses_txt_01", tutorialsUI.textBox.actionDefenseTypesSprite),
                new EDU_Page(title, titleKey, "Any unit that uses energy to attack pulls from the same energy that you use to create generator and defense units. Notably, lane blasters and traps don't use any energy to attack, and shields don't attack at all. Remember: your current energy amount is displayed in the top middle.", "trl_firstActionDefenses_txt_02", tutorialsUI.textBox.actionDefenseTypesSprite),
                new EDU_Page(title, titleKey, "If you want to stop defense units from using energy, select the energy block button on the left. This will block the energy flow to defense units, which will prevent them from functioning. Units that don't use energy for anything once they've been created are unaffected. Select the block button again to allow defense units to use energy again.", "trl_firstActionDefenses_txt_03", tutorialsUI.textBox.energyBlockSprite),
                new EDU_Page(title, titleKey, "With all that explained, back to the stage!", "trl_firstActionDefenses_txt_04"),
            };

            // Add callback for the action manager.
            if (ActionManager.Instantiated)
            {
                pages[0].OnPageOpenedAddCallback(ActionManager.Instance.OnFirstActionDefensesTutorialStart);
            }

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionDefensesTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first action stage - first kill tutorial.
        public void LoadFirstActionFirstKillTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "First Action Stage - Enemy Defeated";
            string titleKey = "trl_firstActionFirstKill_ttl";


            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "You've defeated your first enemy, but there's still more on the way. Now that you have an idea of how action stages work, there's some more mechanics that you should know about.", "trl_firstActionFirstKill_txt_00"),
                new EDU_Page(title, titleKey, "If you have more units than a unit selector can display at once, you use the arrow buttons to switch rows. The arrows on the unit buttons light up if you have enough energy to create the unit they're pointing towards in an adjacent row. A unit button's arrows remain dark if they have no unit to point to.", "trl_firstActionFirstKill_txt_01", tutorialsUI.textBox.actionUnitButtonsSprite),
                new EDU_Page(title, titleKey, "On the left are the stage speed button, the unit deselect button, the unit remove button, and the energy block button. Since the energy block button has already been covered, it won't be gone over again.", "trl_firstActionFirstKill_txt_02", tutorialsUI.textBox.actionButtonsSprite),
                new EDU_Page(title, titleKey, "The stage speed button allows you to speed the stage up. Press the speed button again to return the stage to normal speed.", "trl_firstActionFirstKill_txt_03", tutorialsUI.textBox.stageSpeedSprite),
                new EDU_Page(title, titleKey, "The unit deselect button allows you to deselect the unit you currently have selected. Your selected unit will also be deselected automatically if you no longer have the energy to create it.", "trl_firstActionFirstKill_txt_04", tutorialsUI.textBox.unitDeselectSprite),
                new EDU_Page(title, titleKey, "The unit remove button turns on remove mode, which allows you to remove any of the units you've placed on the field. When remove mode is enabled, select one of your field units to remove them. To turn off remove mode, press the remove button again, press the unit deselect button, or select a unit from one of the unit selectors on the bottom.", "trl_firstActionFirstKill_txt_05", tutorialsUI.textBox.unitRemoveSprite),
                new EDU_Page(title, titleKey, "With all that explained, let the stage recommence!", "trl_firstActionFirstKill_txt_06"),
            };

            // Add callback for the action manager.
            if (ActionManager.Instantiated)
            {
                pages[0].OnPageOpenedAddCallback(ActionManager.Instance.OnFirstActionFirstKillTutorialStart);
            }

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionFirstKillTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first action stage complete tutorial.
        public void LoadFirstActionCompleteTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "First Action Stage Complete";
            string titleKey = "trl_firstActionComplete_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "You've completed your first action stage! When you complete a stage, sometimes you'll unlock new defense units, which will have their information available in the info log. Relatedly, when a stage is successfully completed, sometimes one or more new stages are unlocked. Some stages can be beaten before others, but all stages must be completed to finish the simulation.", "trl_firstActionComplete_txt_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionCompleteTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first knowledge stage - intro tutorial.
        public void LoadFirstKnowledgeIntroTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "First Knowledge Stage - Introduction";
            string titleKey = "trl_firstKnowledgeIntro_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This is a knowledge stage! In knowledge stages, you must match statements with the natural resources they apply to. Once you've made connections, select the verify button in the bottom right to check your answers. All statements must be matched correctly to complete the stage.", "trl_firstKnowledgeIntro_txt_00"),
                new EDU_Page(title, titleKey, "The statements and resources are on separate screens, with the current screen's title being displayed in the top middle. To switch screens, use the arrow buttons at the top. When you have a statement or resource selected, it will be shown at the bottom.", "trl_firstKnowledgeIntro_txt_01"),
                new EDU_Page(title, titleKey, "Once you have a statement selected, it will be connected to the next resource you select. If you have a resource selected, it will be connected to the next statement you select. When a statement and resource are connected, they will have the same number attached to them.", "trl_firstKnowledgeIntro_txt_02"),
                new EDU_Page(title, titleKey, "In the top left is the info log button and in the top right is the options menu button. The info log and options menu are the same as they are in action stages. However, if you open the info log, statements that haven't been matched correctly will be randomized to new ones. The info log will also be locked once you close it, but it will be unlocked again once you perform a verification.", "trl_firstKnowledgeIntro_txt_03", tutorialsUI.textBox.logOptionsButtonsSprite),
                new EDU_Page(title, titleKey, "Try to beat the stage with as few verifications as possible. With all that stated, time to start the stage!", "trl_firstKnowledgeIntro_txt_04"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedFirstKnowledgeIntroTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first knowledge stage verify tutorial.
        public void LoadFirstKnowledgeVerifyTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "First Knowledge Stage - Verification";
            string titleKey = "trl_firstKnowledgeVerify_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "You've performed a connection verification! All statements and resources that have been matched correctly have become locked. All statements that weren't matched correctly or weren't connected to anything have been randomized to new ones.", "trl_firstKnowledgeVerify_txt_00"),
                new EDU_Page(title, titleKey, "When you complete a knowledge stage, you can get an energy start bonus depending on how many verifications it took. An energy start bonus gives you more energy at the start of your next action stage. If you took too many verifications to beat a knowledge stage, you won't get any energy bonus from that stage.", "trl_firstKnowledgeVerify_txt_01"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedFirstKnowledgeVerifyTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first knowledge stage complete tutorial.
        public void LoadFirstKnowledgeCompleteTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "First Knowledge Complete";
            string titleKey = "trl_firstKnowledgeComplete_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "You've completed your first knowledge stage! If you got an energy start bonus, it'll be displayed at the top middle of the screen. As mentioned, the energy start bonus will be applied in your next action stage.", "trl_firstKnowledgeComplete_txt_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedFirstKnowledgeCompleteTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the first area complete tutorial.
        public void LoadFirstAreaCompleteTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "First Area Complete";
            string titleKey = "trl_firstAreaComplete_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "You've cleared the first area! Once all the stages in an area have been completed, you can move onto the next area. Switch areas using the on-screen arrow buttons.", "trl_firstAreaComplete_txt_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedFirstAreaCompleteTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the final area start tutorial.
        public void LoadFinalAreaTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "Final Area Introduction";
            string titleKey = "trl_finalArea_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This is the final area! Once you clear all the stages in this area, the simulation game is complete. Good luck!", "trl_finalArea_txt_00"),
            };

            // Sets the bool and loads the tutorial.
            data.clearedFinalAreaTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }


        // Natural Resources //
        // Loads the biomass tutorial.
        public void LoadBiomassTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "Biomass";
            string titleKey = "trl_biomass_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses the renewable resource biomass. Biomass entails burning plant and/or waste materials to spin a turbine to generate energy.", "trl_biomass_txt_00", tutorialsUI.textBox.biomassSprite),
                new EDU_Page(title, titleKey, "A biomass generator can be placed anywhere on land, generates energy regularly, and produces no air pollution. It generates a moderate amount of energy at a slow rate.", "trl_biomass_txt_01", tutorialsUI.textBox.biomassSprite),
            };

            // Sets the bool and loads the tutorial.
            data.clearedBiomassTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the geothermal tutorial.
        public void LoadGeothermalTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "Geothermal";
            string titleKey = "trl_geothermal_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses the renewable resource geothermal. Geothermal power uses the Earth's internal heat to produce hot water and/or steam to spin a turbine to generate energy.", "trl_geothermal_txt_00", tutorialsUI.textBox.geothermalSprite),
                new EDU_Page(title, titleKey, "A geothermal generator can only be placed on geothermal spots, generates energy perpetually, and produces no air pollution. It generates a high amount of energy at a moderate rate.", "trl_geothermal_txt_01", tutorialsUI.textBox.geothermalSprite),
            };

            // Sets the bool and loads the tutorial.
            data.clearedGeothermalTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the hydro tutorial.
        public void LoadHydroTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "Hydro";
            string titleKey = "trl_hydro_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses the renewable resource hydro. Hydropower uses the flow of water to turn a turbine to generate energy.", "trl_hydro_txt_00", tutorialsUI.textBox.hydroSprite),
                new EDU_Page(title, titleKey, "Hydro generators can only be placed in rivers, cannot be next to other hydro generators, and cannot be next to wind turbines in the water. Hydro generators produce a very small amount of air pollution and will flood the spots behind them if they run for too long. They produce a high amount of energy at a high rate.", "trl_hydro_txt_01", tutorialsUI.textBox.hydroSprite),
            };

            // Sets the bool and loads the tutorial.
            data.clearedHydroTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the solar tutorial.
        public void LoadSolarTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "Solar";
            string titleKey = "trl_solar_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses the renewable resource solar. Solar power entails using solar panels to convert sunlight into electricity, which can only be done during the day.", "trl_solar_txt_00", tutorialsUI.textBox.solarSprite),
                new EDU_Page(title, titleKey, "Solar generators can be placed anywhere on land, produce no air pollution, and can only be used during the daytime. They produce a moderate amount of energy at a high rate.", "trl_solar_txt_01", tutorialsUI.textBox.solarSprite),
            };

            // Sets the bool and loads the tutorial.
            data.clearedSolarTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the wave tutorial.
        public void LoadWaveTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "Wave";
            string titleKey = "trl_wave_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses the renewable resource wave. Wave power uses the movement of waves to turn turbines to generate energy. High wind areas have the most potential for using wave energy, and no energy is generated if there's no waves.", "trl_wave_txt_00", tutorialsUI.textBox.waveSprite),
                new EDU_Page(title, titleKey, "Wave generators can only be placed in the sea, generate energy based on the wind speed, and produce no air pollution. They produce a moderate amount of energy at varying speeds.", "trl_wave_txt_01", tutorialsUI.textBox.waveSprite),
            };

            // Sets the bool and loads the tutorial.
            data.clearedWaveTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the wind tutorial.
        public void LoadWindTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "Wind";
            string titleKey = "trl_wind_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses the renewable resource wind. Wind is used to spin wind turbines to generate energy. Wind turbines can be placed on land or offshore in shallow waters. Wind turbines can't produce energy when there's no wind.", "trl_wind_txt_00", tutorialsUI.textBox.windSprite),
                new EDU_Page(title, titleKey, "Wind generators can be placed on land or in water tiles close to land. They create no air pollution, and generate energy based on the wind speed. Wind generators produce a moderate amount of energy at varying rates.", "trl_wind_txt_01", tutorialsUI.textBox.windSprite),
            };

            // Sets the bool and loads the tutorial.
            data.clearedWindTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the coal tutorial.
        public void LoadCoalTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "Coal";
            string titleKey = "trl_coal_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses coal, which is a non-renewable fossil fuel resource that's fed to a furnace to heat water. This water then produces steam, which is used to spin a turbine to generate energy. Coal produces toxic chemicals when burned and can release toxic minerals if mined on the surface.", "trl_coal_txt_00", tutorialsUI.textBox.coalSprite),
                new EDU_Page(title, titleKey, "Coal generators can only be placed on coal spots, generate energy for a limited time, and create a high amount of air pollution. They produce a high amount of energy at a high rate.", "trl_coal_txt_01", tutorialsUI.textBox.coalSprite),
            };

            // Sets the bool and loads the tutorial.
            data.clearedCoalTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the naturalgas tutorial.
        public void LoadNaturalGasTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "Natural Gas";
            string titleKey = "trl_naturalGas_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses the non-renewable fossil fuel resource natural gas, which is burned to produce gases to spin a turbine to generate energy. Natural gas is often found with coal or oil, is highly flammable, must have its poisonous elements removed before being burned, and releases pollutants when burned.", "trl_naturalGas_txt_00", tutorialsUI.textBox.naturalGasSprite),
                new EDU_Page(title, titleKey, "Natural gas generators generate energy for a limited time, produce moderate amounts of air pollution, and can be placed on natural gas, oil, or coal spots. They produce a high amount of energy at a low rate.", "trl_naturalGas_txt_01", tutorialsUI.textBox.naturalGasSprite),
            };

            // Sets the bool and loads the tutorial.
            data.clearedNaturalGasTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the nuclear tutorial.
        public void LoadNuclearTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "Nuclear";
            string titleKey = "trl_nuclear_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses the non-renewable resource nuclear. Nuclear power involves splitting uranium atoms to generate heat to boil water, which then produces steam to spin a turbine to generate energy. This reaction produces no pollution, but it creates radioactive waste, which must be stored away to prevent harm to people and/or the environment.", "trl_nuclear_txt_00", tutorialsUI.textBox.nuclearSprite),
                new EDU_Page(title, titleKey, "Nuclear generators can only be placed on nuclear spots, generate energy for a limited time, and produce no air pollution. Nuclear generators are automatically destroyed once they deplete their resources but will leave nuclear waste behind if they're destroyed by enemies. No units can be placed on a tile that has nuclear waste. Nuclear generators produce a very high amount of energy at a very high rate.", "trl_nuclear_txt_01", tutorialsUI.textBox.nuclearSprite),
            };

            // Sets the bool and loads the tutorial.
            data.clearedNuclearTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

        // Loads the oil tutorial.
        public void LoadOilTutorial(bool startTutorial = true)
        {
            // The title and title translation key.
            string title = "Oil";
            string titleKey = "trl_oil_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses the non-renewable fossil fuel resource oil, which is burned to produce steam to turn a turbine to generate energy. Oil can be found on land or at sea, but drilling for oil in the ocean can cause an oil spill if done improperly.", "trl_oil_txt_00", tutorialsUI.textBox.oilSprite),
                new EDU_Page(title, titleKey, "Oil generators can only be placed on oil spots, generate energy for a limited time, and produce high amounts of air pollution. They can also cause oil spills if destroyed by enemies before they've used up their resources. No units can be placed on spots that have oil spills. Oil generators generate a very high amount of energy at a moderate rate.", "trl_oil_txt_01", tutorialsUI.textBox.oilSprite),
            };

            // Sets the bool and loads the tutorial.
            data.clearedOilTutorial = true;
            LoadTutorial(ref pages, startTutorial);
        }

    }
}