using System.Collections;
using System.Collections.Generic;
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
        public List<WorldArea> areas;

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
                LoadKnowledgeScene();
            }
            else
            {
                Debug.LogError("No destination scene could be determined.");
                return;
            }
        }

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

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}