using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

        // If 'true', auto saving is enabled.
        private bool autoSave = true;

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

        // The world stages.
        public List<WorldStage> stages = new List<WorldStage>();

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
            // Tries to find the start info.
            WorldStartInfo startInfo = FindObjectOfType<WorldStartInfo>();

            // If start info was found, apply the data and destroy the info object.
            if (startInfo != null)
            {
                // Apply the start info.
                startInfo.ApplyStartInfo(this);

                // Destroy the start info.
                Destroy(startInfo.gameObject);
            }
            else
            {
                // The current area index.
                currAreaIndex = 0;

                // Sets the current area to be the first one.
                SetCurrentWorldArea(currAreaIndex);
                
            }

            // If the data logger exists.
            if (dataLogger != null)
            {
                // Reset.
                dataLogger.gameScore = 0;

                // Goes through all stages.
                for (int i = 0; i < stages.Count; i++)
                {
                    // Stage exists.
                    if (stages[i] != null)
                    {
                        // Adds the score to the data logger.
                        dataLogger.gameScore += stages[i].score;
                    }
                }
            }
            
            // TODO: add a variable that shows that initialization took place.
        }

        // SAVING/LOADING
        // Generates the save data for the game.
        public EDU_GameData GenerateSaveData()
        {
            // TODO: implement.
            return null;
        }

        // Sets if the game should be auto saving.
        public bool AutoSave
        {
            get
            {
                return autoSave;
            }

            set
            {
                autoSave = value;
            }
        }

        // GEts the world area at the provided index.
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
                worldCamera.Move(currArea, instantCameraMovement);
            }
        }

        // Gets the current world area index.
        public int GetCurrentWorldAreaIndex()
        {
            return currAreaIndex;
        }

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

        // Starts the provided stage.
        public void StartStage(WorldStage worldStage)
        {
            // TODO: create start info.

            // Checks the stage type to see what kind of scene to load.
            if (worldStage is WorldActionStage)
            {
                LoadActionScene();
            }
            else if(worldStage is WorldKnowledgeStage)
            {
                // Creates a temporary object and add the knowledge stage start info.
                GameObject tempObject = new GameObject("Knowledge Stage Start Info");
                KnowledgeStageStartInfo kssi = tempObject.AddComponent<KnowledgeStageStartInfo>();

                // Sets the start info.
                kssi.SetStartInfo(worldStage);

                // Don't destroy the temporary object. It will be destroyed in the action scene.
                DontDestroyOnLoad(tempObject);

                // Load the scene.
                LoadKnowledgeScene();
            }
            else
            {
                Debug.LogError("No destination scene could be determined.");
                return;
            }
        }

        // SAVE/LOAD
        // Saves the data for the game.
        public bool SaveGame()
        {
            // TODO: implement.
            return false;
        }

        // Loads data, and return a 'bool' to show it was successful.
        public bool LoadGame()
        {
            // TODO: implement.
            return false;
        }

        // Checks if the game is complete.
        public bool IsGameComplete()
        {
            // TODO: implement.
            return false;
        }

        // Called when the game has been completed.
        public void CompleteGame()
        {
            // TODO: create results data

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