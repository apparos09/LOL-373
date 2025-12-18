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

        // The areas.
        public List<WorldArea> areas = new List<WorldArea>();

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

        // Gets a world stage by its index.
        public WorldStage GetWorldStage(int index)
        {
            // If the index is greater than 0 and less than the stage count...
            // Get the stage from the list.
            if(index >= 0 && index < stages.Count)
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

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}