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
            // The tutorial count.
            public const int TUTORIAL_COUNT = 21;

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
            public bool clearedFinalAreaIntroTutorial = false;

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

            // Gets the tutorials count.
            public int GetTutorialsCount()
            {
                return TUTORIAL_COUNT;
            }

            // The tutorials cleared count.
            public int GetTutorialsClearedCount()
            {
                // The cleared count.
                int clearedCount = 0;

                // // OLD
                // // World - 1
                // clearedCount += clearedIntroTutorial ? 1 : 0;
                // 
                // // Action
                // clearedCount += clearedFirstActionIntroTutorial ? 1 : 0;
                // clearedCount += clearedFirstActionGeneratorsTutorial ? 1 : 0;
                // clearedCount += clearedFirstActionDefensesTutorial ? 1 : 0;
                // clearedCount += clearedFirstActionFirstKillTutorial ? 1 : 0;
                // 
                // // Action Complete (World)
                // clearedCount += clearedFirstActionCompleteTutorial ? 1 : 0;
                // 
                // // Knowledge 
                // clearedCount += clearedFirstKnowledgeIntroTutorial ? 1 : 0;
                // clearedCount += clearedFirstKnowledgeVerifyTutorial ? 1 : 0;
                // 
                // // Knowledge Complete (World)
                // clearedCount += clearedFirstKnowledgeCompleteTutorial ? 1 : 0;
                // 
                // // World - 2
                // clearedCount += clearedFirstAreaCompleteTutorial ? 1 : 0;
                // clearedCount += clearedFinalAreaIntroTutorial ? 1 : 0;
                // 
                // // Natural Resources
                // clearedCount += clearedBiomassTutorial ? 1 : 0;
                // clearedCount += clearedGeothermalTutorial ? 1 : 0;
                // clearedCount += clearedHydroTutorial ? 1 : 0;
                // clearedCount += clearedSolarTutorial ? 1 : 0;
                // clearedCount += clearedWaveTutorial ? 1 : 0;
                // clearedCount += clearedWindTutorial ? 1 : 0;
                // clearedCount += clearedCoalTutorial ? 1 : 0;
                // clearedCount += clearedNaturalGasTutorial ? 1 : 0;
                // clearedCount += clearedNuclearTutorial ? 1 : 0;
                // clearedCount += clearedOilTutorial ? 1 : 0;

                // NEW
                // The cleared tutorial array.
                bool[] clearedArr = new bool[]
                { 
                    // World - 1
                    clearedIntroTutorial,

                    // Action
                    clearedFirstActionIntroTutorial,
                    clearedFirstActionGeneratorsTutorial,
                    clearedFirstActionDefensesTutorial,
                    clearedFirstActionFirstKillTutorial,

                    // Action Complete (World)
                    clearedFirstActionCompleteTutorial,

                    // Knowledge 
                    clearedFirstKnowledgeIntroTutorial,
                    clearedFirstKnowledgeVerifyTutorial,

                    // Knowledge Complete (World)
                    clearedFirstKnowledgeCompleteTutorial,

                    // World - 2
                    clearedFirstAreaCompleteTutorial,
                    clearedFinalAreaIntroTutorial,

                    // Natural Resources
                    clearedBiomassTutorial,
                    clearedGeothermalTutorial,
                    clearedHydroTutorial,
                    clearedSolarTutorial,
                    clearedWaveTutorial,
                    clearedWindTutorial,
                    clearedCoalTutorial,
                    clearedNaturalGasTutorial,
                    clearedNuclearTutorial,
                    clearedOilTutorial
                };

                // Goes through the array.
                foreach(bool value in clearedArr)
                {
                    // If the value is true, add to the cleared count.
                    if (value)
                        clearedCount++;
                }

                // Return the cleared count.
                return clearedCount;
            }

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
                copyData.clearedFinalAreaIntroTutorial = clearedFinalAreaIntroTutorial;

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
                clearedFinalAreaIntroTutorial = pasteData.clearedFinalAreaIntroTutorial;

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

        // A tutorial info set, which is used to store info for a tutorial.
        public class TutorialInfo
        {
            public string title = "";
            public string titleKey = "";
            public List<Page> pages = new List<Page>();

            // Translates the title.
            public void TranslateTitle()
            {
                // If possible, translate the title.
                if(titleKey != "" && LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
                {
                    title = LOLManager.GetLanguageTextStatic(titleKey);
                }
            }

            // Removed since the title should be saved already translated.
            // // Gets the title translated.
            // public string GetTitleTranslated()
            // {
            //     // If possible, translate the title.
            //     if(titleKey != "" && LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
            //     {
            //         return LOLManager.GetLanguageTextStatic(titleKey);
            //     }
            //     // Returns the title.
            //     else
            //     {
            //         return title;
            //     }
            // }
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

        // Gets the tutorials count.
        public int TutorialsCount
        {
            get { return TutorialsData.TUTORIAL_COUNT; }
        }

        // Gets the tutorials cleared count.
        public int TutorialsClearedCount
        {
            get { return data.GetTutorialsClearedCount(); }
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


        // TUTORIAL DATA, INFO

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

        // Generates tutorial info.
        // translateTitle: translates the title using the provided title key and saves it to the object if true.
        //  - The tutorial log assumes the title has already been translated.
        public TutorialInfo GenerateTutorialInfo(string title, string titleKey, ref List<Page> pages, bool translateTitle = true)
        {
            // Creates the info object.
            TutorialInfo tutorialInfo = new TutorialInfo();

            // Load the info.
            tutorialInfo.title = title;
            tutorialInfo.titleKey = titleKey;
            tutorialInfo.pages.AddRange(pages);

            // If the title should be saved as its translated form.
            if(translateTitle)
            {
                // If tranlsation is available, translate.
                if(LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
                {
                    tutorialInfo.title = LOLManager.GetLanguageTextStatic(titleKey);
                }
            }

            // Returns the info.
            return tutorialInfo;
        }

        // Generates tutorials infos list.
        public List<TutorialInfo> GenerateTutorialInfos(bool clearedOnly)
        {
            // The resulting list.
            List<TutorialInfo> resultList = new List<TutorialInfo>();

            // World - 1
            if (!clearedOnly || (clearedOnly && Data.clearedIntroTutorial))
                resultList.Add(GetIntroTutorialInfo());

            // Action
            if (!clearedOnly || (clearedOnly && Data.clearedFirstActionIntroTutorial))
                resultList.Add(GetFirstActionIntroTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedFirstActionGeneratorsTutorial))
                resultList.Add(GetFirstActionGeneratorsTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedFirstActionDefensesTutorial))
                resultList.Add(GetFirstActionDefensesTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedFirstActionFirstKillTutorial))
                resultList.Add(GetFirstActionFirstKillTutorialInfo());

            // Action Complete (World)
            if (!clearedOnly || (clearedOnly && Data.clearedFirstActionCompleteTutorial))
                resultList.Add(GetFirstActionCompleteTutorialInfo());

            // Knowledge 
            if (!clearedOnly || (clearedOnly && Data.clearedFirstKnowledgeIntroTutorial))
                resultList.Add(GetFirstKnowledgeIntroTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedFirstKnowledgeVerifyTutorial))
                resultList.Add(GetFirstKnowledgeVerifyTutorialInfo());

            // Knowledge Complete (World)
            if (!clearedOnly || (clearedOnly && Data.clearedFirstKnowledgeCompleteTutorial))
                resultList.Add(GetFirstKnowledgeCompleteTutorialInfo());

            // World - 2
            if (!clearedOnly || (clearedOnly && Data.clearedFirstAreaCompleteTutorial))
                resultList.Add(GetFirstAreaCompleteTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedFinalAreaIntroTutorial))
                resultList.Add(GetFinalAreaIntroTutorialInfo());

            // Natural Resources
            if (!clearedOnly || (clearedOnly && Data.clearedBiomassTutorial))
                resultList.Add(GetBiomassTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedGeothermalTutorial))
                resultList.Add(GetGeothermalTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedHydroTutorial))
                resultList.Add(GetHydroTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedSolarTutorial))
                resultList.Add(GetSolarTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedWaveTutorial))
                resultList.Add(GetWaveTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedWindTutorial))
                resultList.Add(GetWindTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedCoalTutorial))
                resultList.Add(GetCoalTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedNaturalGasTutorial))
                resultList.Add(GetNaturalGasTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedNuclearTutorial))
                resultList.Add(GetNuclearTutorialInfo());

            if (!clearedOnly || (clearedOnly && Data.clearedOilTutorial))
                resultList.Add(GetOilTutorialInfo());

            // Return the resulting list.
            return resultList;
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
        // GAME INTRO
        // Loads the intro tutorial.
        public void LoadIntroTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetIntroTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedIntroTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the intro tutorial info into an object and returns it.
        public TutorialInfo GetIntroTutorialInfo()
        {  
            // The title and title translation key.
            string title = "Game Introduction";
            string titleKey = "trl_intro_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "Welcome to the Energy Defense Unit (EDU)! You've been tasked with completing a training simulation game that teaches you about natural resources and the EDU's defense systems.", "trl_intro_txt_00"),
                new EDU_Page(title, titleKey, "Earth has various natural resources that are used for generating electricity, among other purposes. These resources all have their advantages and disadvantages, which will be covered in this simulation.", "trl_intro_txt_01"),
                new EDU_Page(title, titleKey, "In this exercise, aliens are attacking Earth's bases to steal their resources. Your job is to defend each base from the invaders. This simulation has two stage types: action (blue) and knowledge (green).", "trl_intro_txt_02", tutorialsUI.textBox.stageTypesSprite),
                new EDU_Page(title, titleKey, "In action stages you defend a base from enemies and in knowledge stages you're assessed on your knowledge of natural resources. You'll be given more details on both stage types later.", "trl_intro_txt_03", tutorialsUI.textBox.stageTypesSprite),
                new EDU_Page(title, titleKey, "In the top left is the info log button, which opens the info log. The info log provides information on simulation elements you've encountered.", "trl_intro_txt_04", tutorialsUI.textBox.logButtonSprite),
                new EDU_Page(title, titleKey, "In the top right is the options button, which opens the options menu. The options menu allows you to save your progress, quit the game, reread tutorials, and adjust the game's settings.", "trl_intro_txt_05", tutorialsUI.textBox.optionsButtonSprite),
                new EDU_Page(title, titleKey, "In the top middle is the energy start bonus display, but that'll be explained later.", "trl_intro_txt_06"),
                new EDU_Page(title, titleKey, "With all that covered, it's time to select a stage. When a stage is selected, it'll show what natural resources it uses. Details on natural resources will be provided when relevant. Please select the available stage, which is an action stage.", "trl_intro_txt_07"),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FIRST ACTION - INTRO
        // Loads the first action stage - intro tutorial.
        public void LoadFirstActionIntroTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstActionIntroTutorialInfo();

            // Add callback for the action manager.
            if (ActionManager.Instantiated)
            {
                tutorialInfo.pages[0].OnPageOpenedAddCallback(ActionManager.Instance.OnFirstActionIntroTutorialStart);
            }

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionIntroTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first action tutorial info into an object and returns it.
        public TutorialInfo GetFirstActionIntroTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Action Stage - Introduction";
            string titleKey = "trl_firstActionIntro_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This is an action stage! In an action stage, you place units on the field to fight off enemies. The enemies start on the right side of the field and make their way to the left side. If an enemy makes it to the left end of the field, you lose.", "trl_firstActionIntro_txt_00"),
                new EDU_Page(title, titleKey, "If the enemy side runs out of energy, you win. The enemy side automatically loses energy as the stage progresses, but defeating an enemy makes the enemy side lose energy faster. The enemy side's energy amount is displayed by the bar on the right.", "trl_firstActionIntro_txt_01", tutorialsUI.textBox.actionEnemyEnergyBarSprite),
                new EDU_Page(title, titleKey, "On the left side of the field are lane blasters. If an enemy touches a lane blaster, the lane blaster will go off, destroying all enemies in its lane. However, this also destroys the lane blaster itself, leaving an opening for enemies to reach the left end of the field.", "trl_firstActionIntro_txt_02", tutorialsUI.textBox.actionDefenseLaneBlasterSprite),
                new EDU_Page(title, titleKey, "A lane blaster cannot be restored once it's destroyed, but you always get your destroyed lane blasters back at the start of a stage.", "trl_firstActionIntro_txt_03", tutorialsUI.textBox.actionDefenseLaneBlasterSprite),
                new EDU_Page(title, titleKey, "As mentioned, you place units on the field to fight off enemies, but to create a unit, you need energy. The energy needed to create a unit is displayed on its unit button.", "trl_firstActionIntro_txt_04"),
                new EDU_Page(title, titleKey, "Some energy is automatically generated periodically, but it won't be enough to defeat the enemies. To generate more energy, you need to place generator units on the field. Your generator units can be found in the bottom left.", "trl_firstActionIntro_txt_05"),
                new EDU_Page(title, titleKey, "When you have a unit selected, it'll be displayed in the bottom middle, and spots on the field will be highlighted to show where that unit can be placed. Two units cannot share the same space, and some units can only use certain tiles.", "trl_firstActionIntro_txt_06", tutorialsUI.textBox.actionUnitSelectedSprite),
                new EDU_Page(title, titleKey, "With all that explained, please place a generator unit on the field.", "trl_firstActionIntro_txt_07"),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FIRST ACTION - GENERATORS
        // Loads the first action stage - generators tutorial.
        public void LoadFirstActionGeneratorsTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstActionGeneratorsTutorialInfo();

            // Add callback for the action manager.
            if (ActionManager.Instantiated)
            {
                tutorialInfo.pages[0].OnPageOpenedAddCallback(ActionManager.Instance.OnFirstActionGeneratorsTutorialStart);
            }

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionGeneratorsTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first action generators tutorial info into an object and returns it.
        public TutorialInfo GetFirstActionGeneratorsTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Action Stage - Generators";
            string titleKey = "trl_firstActionGenerators_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "You've placed a generator unit! Generators generate energy if their conditions are met, and some generators only generate energy for a limited time. A generator flashes blue when it generates energy and goes dark if it cannot generate energy.", "trl_firstActionGenerators_txt_00"),
                new EDU_Page(title, titleKey, "Some generators also produce air pollution, which can lower your stage score. The generators that you're able to use will vary by stage.", "trl_firstActionGenerators_txt_01"),
                new EDU_Page(title, titleKey, "In the top left is the info log button, which opens the info log. The info log is the same as it is in the world area. If you ever need information on what your units do, check the info log.", "trl_firstActionGenerators_txt_02", tutorialsUI.textBox.logButtonSprite),
                new EDU_Page(title, titleKey, "Also on the top left is the day-night indicator, which shows the time of day. As mentioned, solar power can only be generated during the day.", "trl_firstActionGenerators_txt_03", tutorialsUI.textBox.dayNightIndicatorSprite),
                new EDU_Page(title, titleKey, "In the top right is the options button, which opens the options menu. The options menu allows you to quit the stage, reset the stage, reread tutorials, and adjust the game's settings.", "trl_firstActionGenerators_txt_04", tutorialsUI.textBox.optionsButtonSprite),
                new EDU_Page(title, titleKey, "Also on the top right is the wind indicator, which shows the current wind speed. Generators that are affected by wind speed will be explained when relevant.", "trl_firstActionGenerators_txt_05", tutorialsUI.textBox.windIndicatorSprite),
                new EDU_Page(title, titleKey, "In the top middle is the energy display (top value) and air pollution display (bottom value). The energy display shows the current amount of energy you have, and the air pollution display shows how much air pollution you've generated in the current stage.", "trl_firstActionGenerators_txt_06", tutorialsUI.textBox.energyAirPollutionDisplaySprite),
                new EDU_Page(title, titleKey, "With all that covered, let's continue with the stage.", "trl_firstActionGenerators_txt_07"),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FIRST ACTION - DEFENSES
        // Loads the first action stage - defenses tutorial.
        public void LoadFirstActionDefensesTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstActionDefensesTutorialInfo();

            // Add callback for the action manager.
            if (ActionManager.Instantiated)
            {
                tutorialInfo.pages[0].OnPageOpenedAddCallback(ActionManager.Instance.OnFirstActionDefensesTutorialStart);
            }

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionDefensesTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first action defenses tutorial info into an object and returns it.
        public TutorialInfo GetFirstActionDefensesTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Action Stage - Defenses";
            string titleKey = "trl_firstActionDefenses_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "Enemies are approaching! You'll need to use defense units to defeat them. Your defense units can be found in the bottom right. Defense units come in three types: blaster, shield, and trap.", "trl_firstActionDefenses_txt_00", tutorialsUI.textBox.actionDefenseTypesSprite),
                new EDU_Page(title, titleKey, "Blasters (BSRs) use energy to fire projectiles at enemies, shields (SHDs) block enemies, and traps (TRPs) attack enemies that interact with them. You'll unlock more defense units naturally as the simulation progresses.", "trl_firstActionDefenses_txt_01", tutorialsUI.textBox.actionDefenseTypesSprite),
                new EDU_Page(title, titleKey, "Units that use energy to attack pull from the same energy that's used to create generator and defense units. Notably, lane blasters and traps don't use any energy to attack, and shields don't attack at all.", "trl_firstActionDefenses_txt_02", tutorialsUI.textBox.actionDefenseTypesSprite),
                new EDU_Page(title, titleKey, "With all that explained, back to the stage!", "trl_firstActionDefenses_txt_03"),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FIRST ACTION - FIRST KILL
        // Loads the first action stage - first kill tutorial.
        public void LoadFirstActionFirstKillTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstActionFirstKillTutorialInfo();

            // Add callback for the action manager.
            if (ActionManager.Instantiated)
            {
                tutorialInfo.pages[0].OnPageOpenedAddCallback(ActionManager.Instance.OnFirstActionFirstKillTutorialStart);
            }

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionFirstKillTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first action defenses tutorial info into an object and returns it.
        public TutorialInfo GetFirstActionFirstKillTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Action Stage - Enemy Unit Defeated";
            string titleKey = "trl_firstActionFirstKill_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "You've defeated your first enemy, but there's still more on the way. Now that you have an idea of how action stages work, there's some more mechanics that you should know about.", "trl_firstActionFirstKill_txt_00"),
                new EDU_Page(title, titleKey, "You'll get more units as the simulation progresses. When you have more units than a unit selector can display at once, you use the arrow buttons next to the selector to switch rows.", "trl_firstActionFirstKill_txt_01", tutorialsUI.textBox.actionUnitSelectorArrowsSprite),
                new EDU_Page(title, titleKey, "The arrows on a unit button light up if you have energy to create the unit they're pointing towards in an adjacent row. A unit button's arrows remain dark if they have no unit to point to.", "trl_firstActionFirstKill_txt_02", tutorialsUI.textBox.actionUnitButtonsSprite),
                new EDU_Page(title, titleKey, "On the left are the stage speed button, the unit deselect button, the unit remove button, and the energy block button.", "trl_firstActionFirstKill_txt_03", tutorialsUI.textBox.actionButtonsSprite),
                new EDU_Page(title, titleKey, "The stage speed button allows you to toggle between normal stage speed and fast stage speed. The icon displayed on the button shows the current stage speed. 2 arrows mean normal speed and 3 arrows mean fast speed.", "trl_firstActionFirstKill_txt_04", tutorialsUI.textBox.stageSpeedSprite),
                new EDU_Page(title, titleKey, "The unit deselect button deselects the unit you currently have selected. Your selected unit will also be deselected automatically if you no longer have the energy to create it.", "trl_firstActionFirstKill_txt_05", tutorialsUI.textBox.unitDeselectSprite),
                new EDU_Page(title, titleKey, "The unit remove button toggles remove mode, which allows you to remove any of the units you've placed on the field. When remove mode is active, it's displayed in the bottom middle. To remove a field unit, select said unit while in remove mode.", "trl_firstActionFirstKill_txt_06", tutorialsUI.textBox.unitRemoveSprite),
                new EDU_Page(title, titleKey, "The energy block button toggles the energy flow to defense units. When the block is active, defense units are prevented from using any attacks that require energy. Any defense unit affected by the energy block will also go dark.", "trl_firstActionFirstKill_txt_07", tutorialsUI.textBox.energyBlockSprite),
                new EDU_Page(title, titleKey, "The energy block button's icon displays if the energy is currently blocked or not. When the block is inactive, the icon shows a connected plug. When the block is active, the icon shows a disconnected plug.", "trl_firstActionFirstKill_txt_08", tutorialsUI.textBox.energyBlockSprite),
                new EDU_Page(title, titleKey, "With all that explained, let the stage recommence!", "trl_firstActionFirstKill_txt_09"),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FIRST ACTION COMPLETE
        // Loads the first action stage complete tutorial.
        public void LoadFirstActionCompleteTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstActionCompleteTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedFirstActionCompleteTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first action complete tutorial info into an object and returns it.
        public TutorialInfo GetFirstActionCompleteTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Action Stage Complete";
            string titleKey = "trl_firstActionComplete_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "You've completed your first action stage! When you complete a stage, sometimes you'll unlock new defense units, which will have their information available in the info log.", "trl_firstActionComplete_txt_00"),
                new EDU_Page(title, titleKey, "Relatedly, when a stage is successfully completed, sometimes one or more new stages are unlocked. Some stages can be beaten before others, but all stages must be completed to finish the simulation.", "trl_firstActionComplete_txt_01"),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FIRST KNOWLEDGE - INTRO
        // Loads the first knowledge stage - intro tutorial.
        public void LoadFirstKnowledgeIntroTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstKnowledgeIntroTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedFirstKnowledgeIntroTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first knowledge - intro tutorial info into an object and returns it.
        public TutorialInfo GetFirstKnowledgeIntroTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Knowledge Stage - Introduction";
            string titleKey = "trl_firstKnowledgeIntro_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This is a knowledge stage! In knowledge stages, you must match statements with the resources they apply to. Once you've made connections, select the verify button in the bottom right to check your answers. All statements must be matched correctly to complete the stage.", "trl_firstKnowledgeIntro_txt_00"),
                new EDU_Page(title, titleKey, "The statements and resources are on separate screens, with the current screen's title being displayed in the top middle. To switch screens, use the arrow buttons at the top. When you have a statement or resource selected, it will be shown at the bottom.", "trl_firstKnowledgeIntro_txt_01"),
                new EDU_Page(title, titleKey, "If you have a statement selected, it'll be connected to the next resource you select. If you have a resource selected, it'll be connected to the next statement you select. When a statement and resource are connected, they both display the same number.", "trl_firstKnowledgeIntro_txt_02", tutorialsUI.textBox.knowledgeStatementsResourcesSprite),
                new EDU_Page(title, titleKey, "In the top left is the info log button, which opens the info log. The info log provides information on natural resources, which can help you match the statements to the right resources.", "trl_firstKnowledgeIntro_txt_03", tutorialsUI.textBox.logButtonSprite),
                new EDU_Page(title, titleKey, "However, if you open the info log, it will be locked once you close it. To regain access to the info log, you must verify your statement connections using the verify button.", "trl_firstKnowledgeIntro_txt_04", tutorialsUI.textBox.logButtonSprite),
                new EDU_Page(title, titleKey, "In the top right is the options button, which opens the options menu. Like in action stages, the options menu allows you to reset the stage, quit the stage, reread tutorials, and change the game's settings.", "trl_firstKnowledgeIntro_txt_05", tutorialsUI.textBox.optionsButtonSprite),
                new EDU_Page(title, titleKey, "Try to beat the stage with as few verifications as possible. With all that stated, time to start the stage!", "trl_firstKnowledgeIntro_txt_06"),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FIRST KNOWLEDGE - VERIFY
        // Loads the first knowledge stage - verify tutorial.
        public void LoadFirstKnowledgeVerifyTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstKnowledgeVerifyTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedFirstKnowledgeVerifyTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first knowledge verify tutorial info into an object and returns it.
        public TutorialInfo GetFirstKnowledgeVerifyTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Knowledge Stage - Verification";
            string titleKey = "trl_firstKnowledgeVerify_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "You've performed a connection verification! All statements and resources that were matched correctly have become locked. All statements that weren't matched correctly or weren't matched at all have been randomized.", "trl_firstKnowledgeVerify_txt_00"),
                new EDU_Page(title, titleKey, "When you complete a knowledge stage, you can get an energy start bonus depending on how many verifications it took. An energy start bonus gives you more energy at the start of your next action stage.", "trl_firstKnowledgeVerify_txt_01"),
                new EDU_Page(title, titleKey, "If too many verifications are performed in a knowledge stage, no energy start bonus will be given for that stage.", "trl_firstKnowledgeVerify_txt_02"),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FIRST KNOWLEDGE COMPLETE
        // Loads the first knowledge stage complete tutorial.
        public void LoadFirstKnowledgeCompleteTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstKnowledgeCompleteTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedFirstKnowledgeCompleteTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first knowledge complete tutorial info into an object and returns it.
        public TutorialInfo GetFirstKnowledgeCompleteTutorialInfo()
        {
            // The title and title translation key.
            string title = "First Knowledge Stage Complete";
            string titleKey = "trl_firstKnowledgeComplete_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "You've completed your first knowledge stage! If you got an energy start bonus, it'll be displayed at the top middle of the screen. As mentioned, the energy start bonus will be applied in your next action stage.", "trl_firstKnowledgeComplete_txt_00"),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FIRST AREA COMPLETE
        // Loads the first area complete tutorial.
        public void LoadFirstAreaCompleteTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFirstAreaCompleteTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedFirstAreaCompleteTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the first area complete tutorial info into an object and returns it.
        public TutorialInfo GetFirstAreaCompleteTutorialInfo()
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

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // FNAL AREA INTRO
        // Loads the final area intro tutorial.
        public void LoadFinalAreaIntroTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetFinalAreaIntroTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedFinalAreaIntroTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the final area intro tutorial info into an object and returns it.
        public TutorialInfo GetFinalAreaIntroTutorialInfo()
        {
            // The title and title translation key.
            string title = "Final Area Introduction";
            string titleKey = "trl_finalAreaIntro_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This is the final area! Once you clear all the stages in this area, the simulation game is complete. Good luck!", "trl_finalAreaIntro_txt_00"),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }


        // NATURAL RESOURCES //
        // BIOMASS
        // Loads the biomass tutorial.
        public void LoadBiomassTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetBiomassTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedBiomassTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the biomass tutorial info into an object and returns it.
        public TutorialInfo GetBiomassTutorialInfo()
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

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // GEOTHERMAL
        // Loads the geothermal tutorial.
        public void LoadGeothermalTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetGeothermalTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedGeothermalTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the geothermal tutorial info into an object and returns it.
        public TutorialInfo GetGeothermalTutorialInfo()
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

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // HYDRO
        // Loads the hydro tutorial.
        public void LoadHydroTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetHydroTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedHydroTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the hydro tutorial info into an object and returns it.
        public TutorialInfo GetHydroTutorialInfo()
        {
            // The title and title translation key.
            string title = "Hydro";
            string titleKey = "trl_hydro_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses the renewable resource hydro. Hydropower uses the flow of water to turn a turbine to generate energy.", "trl_hydro_txt_00", tutorialsUI.textBox.hydroSprite),
                new EDU_Page(title, titleKey, "Hydro generators can only be placed in rivers. Hydro generators cannot be placed next to other hydro generators and cannot be next to wind generators that are in water.", "trl_hydro_txt_01", tutorialsUI.textBox.hydroSprite),
                new EDU_Page(title, titleKey, "Hydro generators produce a very small amount of air pollution and will flood the spots behind them if they run for too long. They produce a high amount of energy at a high rate.", "trl_hydro_txt_02", tutorialsUI.textBox.hydroSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // SOLAR
        // Loads the solar tutorial.
        public void LoadSolarTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetSolarTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedSolarTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the solar tutorial info into an object and returns it.
        public TutorialInfo GetSolarTutorialInfo()
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

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // WAVE
        // Loads the wave tutorial.
        public void LoadWaveTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetWaveTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedWaveTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the wave tutorial info into an object and returns it.
        public TutorialInfo GetWaveTutorialInfo()
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

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // WIND
        // Loads the wind tutorial.
        public void LoadWindTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetWindTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedWindTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the wind tutorial info into an object and returns it.
        public TutorialInfo GetWindTutorialInfo()
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

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // COAL
        // Loads the coal tutorial.
        public void LoadCoalTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetCoalTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedCoalTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the coal tutorial info into an object and returns it.
        public TutorialInfo GetCoalTutorialInfo()
        {
            // The title and title translation key.
            string title = "Coal";
            string titleKey = "trl_coal_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses coal, which is a non-renewable fossil fuel resource. Coal is fed to a furnace to heat water, which produces steam. This steam then turns a turbine to generate energy. Coal produces toxic chemicals when burned and can release toxic minerals if mined on the surface.", "trl_coal_txt_00", tutorialsUI.textBox.coalSprite),
                new EDU_Page(title, titleKey, "Coal generators can only be placed on coal spots, generate energy for a limited time, and create a high amount of air pollution. They produce a high amount of energy at a high rate.", "trl_coal_txt_01", tutorialsUI.textBox.coalSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // NATURAL GAS
        // Loads the naturalgas tutorial.
        public void LoadNaturalGasTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetNaturalGasTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedNaturalGasTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the natural gas tutorial info into an object and returns it.
        public TutorialInfo GetNaturalGasTutorialInfo()
        {
            // The title and title translation key.
            string title = "Natural Gas";
            string titleKey = "trl_naturalGas_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses the non-renewable fossil fuel resource natural gas. Natural gas is burned to produce gases, which then spin a turbine to generate energy.", "trl_naturalGas_txt_00", tutorialsUI.textBox.naturalGasSprite),
                new EDU_Page(title, titleKey, "Natural gas is often found with coal or oil, is highly flammable, must have its poisonous elements removed before being burned, and releases pollutants when burned.", "trl_naturalGas_txt_01", tutorialsUI.textBox.naturalGasSprite),
                new EDU_Page(title, titleKey, "Natural gas generators generate energy for a limited time, produce moderate amounts of air pollution, and can be placed on natural gas, oil, or coal spots. They produce a high amount of energy at a low rate.", "trl_naturalGas_txt_02", tutorialsUI.textBox.naturalGasSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // NUCLEAR
        // Loads the nuclear tutorial.
        public void LoadNuclearTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetNuclearTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedNuclearTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the nuclear tutorial info into an object and returns it.
        public TutorialInfo GetNuclearTutorialInfo()
        {
            // The title and title translation key.
            string title = "Nuclear";
            string titleKey = "trl_nuclear_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses the non-renewable resource nuclear. Nuclear power involves splitting uranium atoms to generate heat to boil water. This boiled water then produces steam, which spins a turbine to generate energy.", "trl_nuclear_txt_00", tutorialsUI.textBox.nuclearSprite),
                new EDU_Page(title, titleKey, "Splitting uranium atoms produces no pollution, but it creates radioactive waste, which must be stored away to prevent harm to people and/or the environment.", "trl_nuclear_txt_01", tutorialsUI.textBox.nuclearSprite),
                new EDU_Page(title, titleKey, "Nuclear generators can only be placed on nuclear spots, generate energy for a limited time, and produce no air pollution. Nuclear generators are automatically destroyed once they deplete their resources.", "trl_nuclear_txt_02", tutorialsUI.textBox.nuclearSprite),
                new EDU_Page(title, titleKey, "If a nuclear generator is destroyed by an enemy before its resources are depleted, it will leave nuclear waste behind. No units can be placed on a tile that has nuclear waste. Nuclear generators produce a very high amount of energy at a very high rate.", "trl_nuclear_txt_03", tutorialsUI.textBox.nuclearSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }

        // OIL
        // Loads the oil tutorial.
        public void LoadOilTutorial(bool startTutorial = true)
        {
            // Gets the tutorial info.
            TutorialInfo tutorialInfo = GetOilTutorialInfo();

            // Sets the bool and loads the tutorial.
            data.clearedOilTutorial = true;
            LoadTutorial(ref tutorialInfo.pages, startTutorial);
        }

        // Loads the oil tutorial info into an object and returns it.
        public TutorialInfo GetOilTutorialInfo()
        {
            // The title and title translation key.
            string title = "Oil";
            string titleKey = "trl_oil_ttl";

            // Create the pages list.
            List<Page> pages = new List<Page>
            {
                // Load the pages.
                new EDU_Page(title, titleKey, "This stage uses the non-renewable fossil fuel resource oil, which is burned to produce steam to turn a turbine to generate energy. Oil can be found on land or at sea, but drilling for oil in the ocean can cause an oil spill if done improperly.", "trl_oil_txt_00", tutorialsUI.textBox.oilSprite),
                new EDU_Page(title, titleKey, "Oil generators can only be placed on oil spots, generate energy for a limited time, and produce high amounts of air pollution.", "trl_oil_txt_01", tutorialsUI.textBox.oilSprite),
                new EDU_Page(title, titleKey, "Oil generators leave oil spills if they're destroyed by enemies before they've used up their resources. No units can be placed on spots that have oil spills. Oil generators generate a very high amount of energy at a moderate rate.", "trl_oil_txt_02", tutorialsUI.textBox.oilSprite),
            };

            // Creates the info object.
            TutorialInfo tutorialInfo = GenerateTutorialInfo(title, titleKey, ref pages);

            // Returns the info.
            return tutorialInfo;
        }
    }
}