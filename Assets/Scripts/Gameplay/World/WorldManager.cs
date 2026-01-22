using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The manager for the world scene.
    public class WorldManager : GameplayManager
    {
        // the instance of the class.
        private static WorldManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The stage count.
        public const int STAGE_COUNT = 11;

        // If saving and loading is enabled.
        // NOTE: set this to true when you have the tutorial set up.
        private bool savingLoadingEnabled = false;

        // If 'true', auto saving is enabled.
        private bool autoSavingEnabled = true;

        [Header("World")]

        // The world UI.
        public WorldUI worldUI;

        // The world camera.
        public WorldCamera worldCamera;

        // If 'true', the camera moves gradually from one area to the next.
        // If 'false', the camera moves instantly from one area to the next.
        private bool instantCameraMovement = true;

        // The areas.
        public List<WorldArea> areas = new List<WorldArea>();

        // The current area.
        private int currAreaIndex = 0;

        // If 'true', the state of the game effects the area buttons.
        // If 'false', the area buttons remain on at all times.
        private bool effectAreaButtons = true;

        // The world stages.
        public List<WorldStage> stages = new List<WorldStage>();

        // The default defense ids that the player has.
        [Tooltip("The defense ids that are set by default.")]
        public List<int> defaultDefenseIds = new List<int>();

        // Set to 'true' if the world has been initialized.
        protected bool worldInitialized = false;

        [Header("World/Events")]

        // The game complete event of the world manager.
        public GameCompleteEvent gameCompleteEvent;

        // Awake is called when the script is being loaded
        protected override void Awake()
        {
            base.Awake();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Gets the world UI instance.
            if (worldUI == null)
                worldUI = WorldUI.Instance;

            // If the world camera isn't set, try to get the world camera...
            // From the main camera.
            if(worldCamera == null)
            {
                worldCamera = Camera.main.GetComponent<WorldCamera>();
            }

            // If the game complete event isn't set, try to grab the component.
            if(gameCompleteEvent == null)
            {
                gameCompleteEvent = GetComponent<GameCompleteEvent>();
            }

            // Finds all the stages in the scene if the list has none.
            if (areas.Count <= 0)
            {
                areas.Clear();
                areas.AddRange(FindObjectsOfType<WorldArea>());
            }

            // Finds all the stages in the scene if the list has none.
            if(stages.Count <= 0)
            {
                stages.Clear();
                stages.AddRange(FindObjectsOfType<WorldStage>());
            }

            // Initializes the world.
            InitializeWorld();
        }

        // Gets the instance.
        public static WorldManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<WorldManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("WorldManager (singleton)");
                        instance = go.AddComponent<WorldManager>();
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

        // Initializes the world.
        public void InitializeWorld()
        {
            // Gets the data logger instance.
            // The data logger should already be set.
            if (dataLogger == null)
                dataLogger = DataLogger.Instance;

            // Applies the data logger's world datas to the world.
            dataLogger.ApplyWorldStageDatasToWorld(this);

            // Tries to find the start info.
            WorldStartInfo startInfo = FindObjectOfType<WorldStartInfo>();

            // If start info was found, apply the data and destroy the info object.
            if (startInfo != null)
            {
                // Apply the start info.
                startInfo.ApplyStartInfo(this);

                // If saving/loading is enabled and auto saving is enabled.
                if(savingLoadingEnabled && autoSavingEnabled)
                {
                    // If the player returned from a stage, and that stage was completed, auto save.
                    if (startInfo.fromStage && startInfo.stageCompleted)
                    {
                        // Auto save the game.
                        SaveGame();
                    }
                }

                // Destroy the start info.
                Destroy(startInfo.gameObject);
            }
            else
            {
                // The current area index.
                currAreaIndex = 0;

                // Sets the current area to be the first one.
                SetCurrentWorldArea(currAreaIndex);

                // If area buttons should be effected.
                if(effectAreaButtons)
                {
                    // Makes sure the world UI's world manager is set.
                    if (worldUI.worldManager == null)
                        worldUI.worldManager = this;

                    // Refreshes the world area buttons.
                    worldUI.RefreshWorldAreaButtons();
                }
            }

            // Gives the data logger the default defense ids if there are ones.
            if (dataLogger.defenseIds.Count <= 0 && defaultDefenseIds.Count > 0)
            {
                dataLogger.defenseIds.Clear();
                dataLogger.defenseIds.AddRange(defaultDefenseIds);
            }

            // Calculates and sets the game score.
            CalculateAndSetGameScore();


            // Save the game score to the data logger.
            dataLogger.gameScore = gameScore;

            // Submits progress for the game.
            SubmitProgress();

            // The world has been intialized.
            worldInitialized = true;
        }

        // Returns 'true' if the world has been initialized.
        public bool WorldInitialized
        {
            get { return worldInitialized; }
        }

        // SAVING/LOADING
        // Generates the save data for the game.
        public EDU_GameData GenerateSaveData()
        {
            // The game data to return.
            EDU_GameData data = new EDU_GameData();

            // Gets the game score, game time, and game energy.
            data.gameScore = dataLogger.gameScore;
            data.gameTime = dataLogger.gameTimer;
            data.gameEnergy = CalculateEnergyTotal();

            // Gets the world stage data for every stage.
            for(int i = 0; i < data.worldStageDatas.Length && i < stages.Count; i++)
            {
                // The stage exists.
                if (stages[i] != null)
                {
                    data.worldStageDatas[i] = stages[i].GenerateWorldStageData();
                }
                // The stage doesn't exist.
                else
                {
                    data.worldStageDatas[i] = null;
                }
            }

            // Saves the current area index.
            data.currentAreaIndex = GetCurrentWorldAreaIndex();

            // Tutorial parameter.
            data.useTutorial = GameSettings.Instance.UseTutorial;

            // Generates the tutorial data.
            data.tutorialData = tutorials.GenerateTutorialsData();

            // Saves if the game is complete.
            data.complete = IsGameComplete();

            // The data is valid.
            data.valid = true;

            // Returns the result.
            return data;
        }

        // Checks if saving and loading is allowed.
        public bool SavingLoadingEnabled
        {
            get 
            { 
                return savingLoadingEnabled; 
            }
        }

        // Sets if the game should be auto saving.
        public bool AutoSavingEnabled
        {
            get
            {
                return autoSavingEnabled;
            }
        }


        // Saves the data for the game.
        public bool SaveGame()
        {
            // First checks if saving/loading is enabled.
            if(!savingLoadingEnabled)
            {
                Debug.LogError("Saving and loading data is disabled. Save failed.");
                return false;
            }

            // Not needed since the save system is checke for anyway.
            // // Checks if the LOL Manager and the LOLSDK exists.
            // if(LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
            // {
            //     Debug.LogError("The LOLSDK has not been initialized.");
            //     return false;
            // }

            // The save system hasn't bene instantiated.
            if(!SaveSystem.Instantiated)
            {
                Debug.LogError("The save system has not been instantiated.");
                return false;
            }

            // Gets the save system.
            SaveSystem saveSystem = SaveSystem.Instance;

            // Sets the world manager for the save system.
            saveSystem.worldManager = this;

            // Save the game.
            bool result = saveSystem.SaveGame();

            // Returns the result.
            return result;
        }

        // Saves and continues the game.
        public bool SaveAndContinue()
        {
            // Saves the game.
            bool result = SaveGame();

            // Closes all the dialogs.
            worldUI.CloseAllDialogs();

            // Returns the result.
            return result;
        }

        // Saves and quits the game.
        public bool SaveAndQuit()
        {
            // Saves the game.
            bool result = SaveGame();

            // Loads the title screen.
            LoadTitleScene();

            // Returns the result.
            return result;
        }

        // Loads data, and return a 'bool' to show it was successful.
        public bool LoadGame()
        {
            // Checks if saving/loading is enabled.
            if(!savingLoadingEnabled)
            {
                Debug.LogError("Saving and loading is disabled. Load failed.");
                return false;
            }

            // Cheks if the save system has been instantiated.
            if (!SaveSystem.Instantiated)
            {
                Debug.LogError("The save system hasn't been instantiated. Load failed.");
                return false;
            }

            // Gets the save system.
            SaveSystem saveSystem = SaveSystem.Instance;

            // Sets the world manager for the save system.
            saveSystem.worldManager = this;

            // Checks if the save system has loaded data.
            if (!saveSystem.HasLoadedData())
            {
                Debug.LogWarning("No data was found to load.");
                return false;
            }

            // Gets the loaded data.
            EDU_GameData data = saveSystem.loadedData;

            // Checks for validity.
            if(!data.valid)
            {
                Debug.LogError("The loaded data was invalid. Load failed.");
                return false;
            }

            // Checks if the loaded data is complete.
            if(data.complete)
            {
                Debug.LogWarning("The loaded data is for a completed game. Load failed.");
                return false;
            }

            // Load the data.
            // Gets the game score, game time, and game energy.
            dataLogger.gameScore = data.gameScore;
            dataLogger.gameTimer = data.gameTime;
            // TODO: the energy total should be set with the stage data automatically.

            // Gets the world stage data for every stage.
            for (int i = 0; i < data.worldStageDatas.Length && i < stages.Count; i++)
            {
                // The current data.
                WorldStage.WorldStageData currData = data.worldStageDatas[i];

                // If the world stage data exists and the stage exists, apply the world data.
                if (currData != null && stages[i] != null)
                {
                    // If the id numbers don't match, make a warning.
                    if(currData.idNumber != stages[i].idNumber)
                    {
                        Debug.LogWarning("The id number of the loaded data doesn't match the stage it's giving the data to.");
                    }

                    // Applies the world stage data to the stage.
                    stages[i].ApplyWorldStageData(data.worldStageDatas[i]);
                }
            }

            // Saves the current area index.
            SetCurrentWorldArea(data.currentAreaIndex);

            // Tutorial parameter.
            GameSettings.Instance.UseTutorial = data.useTutorial;

            // Loads the tutorials data.
            tutorials.LoadTutorialsData(data.tutorialData);

            // Complete and Valid parameters were already checked.

            // Data set successfully.
            return true;
        }

        // WORLD //

        // Calculates the game score.
        public float CalculateGameScore()
        {
            float totalScore = 0.0F;

            // Goes through all stages.
            for (int i = 0; i < stages.Count; i++)
            {
                // Stage exists.
                if (stages[i] != null)
                {
                    // Adds the score to the data logger.
                    totalScore += stages[i].score;
                }
            }

            return totalScore;
        }

        // Calculates the game score as an int.
        public int CalculateGameScoreAsInt()
        {
            float scoreFloat = CalculateGameScore();
            int scoreInt = Mathf.CeilToInt(scoreFloat);

            return scoreInt;
        }

        // Calculates and sets the game score.
        public void CalculateAndSetGameScore()
        {
            float score = CalculateGameScore();
            gameScore = score;
        }

        // Calculates the energy total.
        public float CalculateEnergyTotal()
        {
            // The energy total to return.
            float energyTotal = 0;

            // Goes through all stages to get the energy total.
            foreach(WorldStage stage in stages)
            {
                // If the stage exists, add to the energy total.
                if(stage != null)
                {
                    energyTotal += stage.energyTotal;
                }
            }

            return energyTotal;
        }


        // AREA //
        // Gets the world area at the provided index.
        public WorldArea GetWorldArea(int index)
        {
            // Index validity check.
            if (index >= 0 && index < areas.Count)
            {
                return areas[index];
            }
            else
            {
                return null;
            }
        }

        // Gets the index of the current world area.
        public int GetWorldAreaIndex()
        {
            // Gets the area.
            WorldArea currArea = GetCurrentWorldArea();

            // Area is in list.
            if (areas.Contains(currArea))
            {
                // Return index.
                return areas.IndexOf(currArea);
            }
            else
            {
                // Return -1 since not in list.
                return -1;
            }
        }

        // Gets the current world area.
        public WorldArea GetCurrentWorldArea()
        {
            return GetWorldArea(currAreaIndex);
        }

        // Sets the current world area.
        public void SetCurrentWorldArea(int index)
        {
            // Index validity check.
            if (index >= 0 && index < areas.Count)
            {
                // Set the current area index.
                currAreaIndex = index;
            }

            // Sets the current world area.
            SetCurrentWorldArea(areas[currAreaIndex]);
        }

        // Sets the current world area.
        public void SetCurrentWorldArea(WorldArea currArea)
        {
            // TODO: enable/disable areas?

            // If the current area exists, move the camera towards it.
            if (currArea != null)
            {
                // Move camera.
                worldCamera.Move(currArea, instantCameraMovement);

                // This is now handled by a function call later in this function.
                // // If the state of the world should effect the area buttons.
                // if(effectAreaButtons)
                // {
                //     // Current area index, prev area index, and next area index.
                //     int currAreaIndex = GetCurrentWorldAreaIndex();
                //     int prevAreaIndex = currAreaIndex - 1;
                //     int nextAreaIndex = currAreaIndex + 1;
                // 
                //     // Turn on both area buttons to start.
                //     worldUI.prevAreaButton.interactable = true;
                //     worldUI.nextAreaButton.interactable = true;
                // 
                //     // Previous
                //     // If negative, make the prev button non-interactable.
                //     if (prevAreaIndex < 0)
                //     {
                //         worldUI.prevAreaButton.interactable = false;
                //     }
                //     else
                //     {
                //         // Set button interactable if the previous area has been cleared.
                //         worldUI.prevAreaButton.interactable = areas[prevAreaIndex].areaCompleteEvent.cleared;
                //     }
                // 
                //     // Next
                //     // If the current area is completed, the next area must be open.
                //     if (nextAreaIndex < areas.Count)
                //     {
                //         worldUI.nextAreaButton.interactable = currArea.areaCompleteEvent.cleared;
                //     }
                //     // If this is the last area in the list, there is no next area to go to.
                //     else
                //     {
                //         worldUI.nextAreaButton.interactable = false;
                //     }
                // }

            }

            // Refreshes the world area buttons.
            if(effectAreaButtons)
                worldUI.RefreshWorldAreaButtons();
        }

        // Gets the current world area index.
        public int GetCurrentWorldAreaIndex()
        {
            return currAreaIndex;
        }

        // Returns 'true' if this is the first world area.
        public bool IsCurrentWorldAreaFirstArea()
        {
            return currAreaIndex == 0;
        }

        // Returns 'true' if this is the last world area.
        public bool IsCurrentWorldAreaLastArea()
        {
            return currAreaIndex == areas.Count - 1;
        }


        // STAGE //
        // Gets a world stage by its index.
        public WorldStage GetWorldStage(int index)
        {
            // Index validity check.
            if (index >= 0 && index < stages.Count)
            {
                return stages[index];
            }
            else
            {
                return null;
            }
        }

        // Gets the index of the stage in the manager's list. Returns "-1" if not in list.
        public int GetWorldStageIndex(WorldStage worldStage)
        {
            // Checks if in list.
            if (stages.Contains(worldStage))
            {
                return stages.IndexOf(worldStage);
            }
            // Not in list.
            else
            {
                return -1;
            }
        }

        // Gets the world area this stage is in.
        public WorldArea GetWorldStageArea(WorldStage worldStage)
        {
            // The area the stage is in.
            WorldArea stageArea = null;

            // Goes through each area.
            foreach(WorldArea area in areas)
            {
                // Goes through each stage.
                foreach(WorldStage areaStage in area.stages)
                {
                    // If the area stage is eqal to the world stage...
                    // This is the area the stage is in.
                    if(areaStage == worldStage)
                    {
                        stageArea = area;
                        break;
                    }
                }

                // Stage area found, so break.
                if(stageArea != null)
                {
                    break;
                }
            }

            // Returns the stage area.
            return stageArea;
        }

        // Goes to the previous world area.
        // If 'wrapAround' is true, the game loops around to the other area if it underflows.
        public void PreviousWorldArea(bool wrapAround)
        {
            // Gets the potential index of the previous area.
            int index = currAreaIndex - 1;

            // If the index is less than 0, either wrap around or stay at 0.
            if(index < 0)
                index = (wrapAround) ? areas.Count - 1 : 0;

            // Set the new area.
            SetCurrentWorldArea(index);
        }

        // Goes to the previous world area, not allowing wrap arounds.
        public void PreviousWorldArea()
        {
            PreviousWorldArea(false);
        }

        // Goes to the next world area.
        // If 'wrapAround' is true, the game loops around to the other area if it overflows.
        public void NextWorldArea(bool wrapAround)
        {
            // Gets the potential index of the next area.
            int index = currAreaIndex + 1;

            // If the index is greater than or equal to the area count, either wrap around or stay at 0.
            if (index >= areas.Count)
                index = (wrapAround) ? 0 : areas.Count - 1;

            // Set the new area.
            SetCurrentWorldArea(index);
        }

        // Goes to the next world area, not allowing wrap arounds.
        public void NextWorldArea()
        {
            NextWorldArea(false);
        }

        // If area buttons should be effected by the state of the world.
        public bool EffectAreaButtons
        {
            get { return effectAreaButtons; }
        }

        // Starts the provided stage.
        public void StartStage(WorldStage worldStage)
        {
            // TODO: create start info.

            // A temporary object that will be used for start info.
            GameObject startInfoObject = null;

            // Checks the stage type to see what kind of scene to load.
            if (worldStage is WorldActionStage)
            {
                // Creates a temporary object and add the action stage start info.
                startInfoObject = new GameObject("Action Stage Start Info");
                ActionStageStartInfo assi = startInfoObject.AddComponent<ActionStageStartInfo>();

                // Sets the start info data.
                assi.SetStartInfo(worldStage);

                // Don't destroy the temporary object. It will be destroyed in the action scene.
                DontDestroyOnLoad(startInfoObject);

                // Load the scene.
                LoadActionScene();
            }
            else if(worldStage is WorldKnowledgeStage)
            {
                // Creates a temporary object and add the knowledge stage start info.
                startInfoObject = new GameObject("Knowledge Stage Start Info");
                KnowledgeStageStartInfo kssi = startInfoObject.AddComponent<KnowledgeStageStartInfo>();

                // Sets the start info data
                kssi.SetStartInfo(worldStage);

                // Don't destroy the temporary object. It will be destroyed in the knowledge scene.
                DontDestroyOnLoad(startInfoObject);

                // Load the scene.
                LoadKnowledgeScene();
            }
            else
            {
                Debug.LogError("No destination scene could be determined.");
                return;
            }
        }


        // PROGRESS, COMPLETE
        // Gets the game progress.
        public int GetGameProgress()
        {
            // Progress
            int progress = 0;

            // Increases progress for every defeated challenger.
            for (int i = 0; i < stages.Count; i++)
            {
                // If the stage is compelte, add it to the progress..
                if (stages[i].IsComplete())
                    progress++;
            }

            // Returns the progress.
            return progress;
        }

        // Gets the game progress as a percentage.
        // If the percentage can't be calculated, -1 is returned.
        public float GetGameProgressAsPercentage()
        {
            // Gets the number of cleared stages.
            int clearedCount = GetGameProgress();
            float progress = 0;

            // Calculates the amount of progress that has been made.
            if (stages.Count > 0)
            {
                progress = (float)clearedCount / stages.Count;
            }
            else // Count is unknown, so just set it to -1.
            {
                progress = -1;
            }

            // Returns the progress.
            return progress;
        }

        // Submits the current game progress.
        public void SubmitProgress()
        {
            // If the LOLManager and the SDK have both been initialized, submit the score and game progress.
            if (LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
                LOLManager.Instance.SubmitProgress(CalculateGameScoreAsInt(), GetGameProgress());
        }

        // Submits the game progress complete.
        public void SubmitProgressComplete()
        {
            // Submits the game score and progress complete.
            if (LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
                LOLManager.Instance.SubmitProgressComplete(CalculateGameScoreAsInt());
        }

        // Checks if the game is complete.
        public bool IsGameComplete()
        {
            return gameCompleteEvent.cleared;
        }

        // Called when the game has been completed.
        public void CompleteGame()
        {
            // TODO: create results data

            // Submit progress complete.
            SubmitProgressComplete();

            // Go to the results scene.
            LoadResultsScene();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}