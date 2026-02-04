using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RM_EDU
{
    // Logs data from the gameplay scenes (world, action, knowledge).
    // This doesn't have any proper functionality. It just tracks data.
    public class DataLogger : MonoBehaviour
    {
        // The singleton instance.
        private static DataLogger instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The game time. The data logger tracks how long the game has been going.
        public float gameTimer = 0.0F;

        // The score for the entire game.
        public float gameScore = 0.0F;

        // If 'true', the timer should be running.
        public bool runGameTimer = true;

        [Header("World")]
        // The world stage data. The index of the array matches up with the index in the world stage list.
        public WorldStage.WorldStageData[] worldStageDatas = new WorldStage.WorldStageData[WorldManager.STAGE_COUNT];

        // The natural resources that the player has used.
        public List<NaturalResources.naturalResource> usedResources = new List<NaturalResources.naturalResource>();

        [Header("Action")]

        // The unlocked defense units for the action stages.
        public List<int> defenseIds = new List<int>();

        // The energy start bonus the player gets.
        public float energyStartBonus = 0.0F;

        [Header("Knowledge")]
        
        // The list of statements the player has matched correctly.
        public List<KnowledgeStatementList.Statement.StatementData> matchedStatementDatas = new List<KnowledgeStatementList.Statement.StatementData>();

        // Constructor
        private DataLogger()
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
        protected virtual void Start()
        {
            // ...
        }

        // Gets the instance.
        public static DataLogger Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<DataLogger>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("DataLogger (singleton)");
                        instance = go.AddComponent<DataLogger>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Resets the game time.
        public void ResetGameTimer()
        {
            gameTimer = 0.0F;
        }

        // Returns 'true' if the game time is paused.
        public bool IsGameTimerPaused()
        {
            return !runGameTimer;
        }

        // Sets if the timer should be paused.
        public void SetGameTimerPaused(bool paused)
        {
            runGameTimer = !paused;
        }

        // WORLD

        // Saves data from the provided world stage.
        public void SaveWorldStageData(WorldStage worldStage)
        {
            // Gets the stage's index.
            int stageIndex = WorldManager.Instance.GetWorldStageIndex(worldStage);

            // If there's an available index.
            if(stageIndex >= 0 && stageIndex < worldStageDatas.Length)
            {
                // Generates world stage data.
                worldStageDatas[stageIndex] = worldStage.GenerateWorldStageData();
            }
            else
            {
                Debug.LogError("There's no place in the world data array for the provided world stage.");
            }
        }

        // Saves all stage datas from the provided world manager.
        public void SaveWorldStageDatas(WorldManager worldManager)
        {
            // Goes through each stage.
            for (int i = 0; i < worldManager.stages.Count; i++)
            {
                // There is a stage in the world manager's list, so save its data.
                if (worldManager.stages[i] != null)
                {
                    // Saves the world stage data.
                    SaveWorldStageData(worldManager.stages[i]);
                }
            }
        }


        // Applies world stage datas to the world.
        public void ApplyWorldStageDatasToWorld(WorldManager worldManager)
        {
            // The stage datas match up with the world stages.
            for(int i = 0; i < worldStageDatas.Length && i < worldManager.stages.Count; i++)
            {
                // There is stage data and a stage at the set index.
                if (worldStageDatas[i] != null && worldManager.stages[i] != null)
                {
                    // Applies the world stage data.
                    worldManager.stages[i].ApplyWorldStageData(worldStageDatas[i]);
                }
            }
        }

        // Gets the energy total from the stage datas.
        public float GetWorldStageDatasEnergyTotal()
        {
            // The energy total to return.
            float energyTotal = 0;

            // Goes through all the stage datas.
            foreach(WorldStage.WorldStageData worldStageData in worldStageDatas)
            {
                // The data exists, so add to the total.
                if (worldStageData != null)
                    energyTotal += worldStageData.energyTotal;
            }

            return energyTotal;
        }

        // Gets the air pollution total from the stage datas.
        public float GetWorldStageDatasAirPollutionTotal()
        {
            // The air pollution total to return.
            float airPollutionTotal = 0;

            // Goes through all the stage datas.
            foreach (WorldStage.WorldStageData worldStageData in worldStageDatas)
            {
                // The data exists, so add to the total.
                if (worldStageData != null)
                    airPollutionTotal += worldStageData.airPollution;
            }

            return airPollutionTotal;
        }


        // NATURAL RESOURCES //

        // Returns true if there are natural resources.
        public bool HasUsedNaturalResources()
        {
            return usedResources.Count > 0;
        }

        // Adds natural resources to the list.
        public void AddUsedNaturalResources(List<NaturalResources.naturalResource> newResources)
        {
            // Goes through the list of new resources.
            foreach (NaturalResources.naturalResource newRes in newResources)
            {
                // The new resource isn't in the list, so add it to the list.
                if(!usedResources.Contains(newRes))
                    usedResources.Add(newRes);
            }

            // Optimizes the list, sorting it and removing duplicates.
            OptimizeUsedNaturalResourcesList();
        }

        // Clears the action defense units list.
        public void ClearUsedNaturalResourcesList()
        {
            usedResources.Clear();
        }

        // Removes duplicates in the natural resources list and sorts it.
        public void OptimizeUsedNaturalResourcesList()
        {
            // Removes duplicates and sorts the list.
            List<NaturalResources.naturalResource> optList = usedResources.Distinct().ToList();
            optList.Sort();

            // Adds the list.
            usedResources.Clear();
            usedResources.AddRange(optList);

        }


        // DEFENSE //
        // Returns 'true' if there are action defense units.
        public bool HasActionDefenseUnits()
        {
            return defenseIds.Count > 0;
        }


        // Adds action defense units to the list.
        public void AddActionDefenseUnits(List<int> newIds)
        {
            // Goes through the list of new ids.
            foreach(int newId in newIds)
            {
                // If the id isn't already in the list, add it.
                if(!defenseIds.Contains(newId))
                    defenseIds.Add(newId);
            }

            // Optimizes the list, sorting it and removing duplicates.
            OptimizeActionDefenseUnitsList();
        }

        // Clears the action defense units list.
        public void ClearActionDefenseUnitsList()
        {
            defenseIds.Clear();
        }

        // Removes duplicates in the action defense units list and sorts it.
        public void OptimizeActionDefenseUnitsList()
        {
            // Removes duplicates and sorts the list.
            List<int> optList = defenseIds.Distinct().ToList();
            optList.Sort();

            // Adds the list.
            defenseIds.Clear();
            defenseIds.AddRange(optList);

        }

        // Update is called once per frame
        void Update()
        {
            // Add to the timer.
            if (runGameTimer)
            {
                gameTimer += Time.unscaledDeltaTime;
            }
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}