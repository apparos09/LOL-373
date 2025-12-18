using System.Collections;
using System.Collections.Generic;
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

        // If 'true', the timer should be running.
        public bool runGameTimer = true;

        [Header("World")]
        // The world stage data. The index of the array matches up with the index in the world stage list.
        public WorldStage.WorldStageData[] worldStageDatas = new WorldStage.WorldStageData[WorldManager.STAGE_COUNT];
         
        // 
        // [Header("Action")]
        // 
        
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