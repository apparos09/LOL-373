using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action manager.
    public class ActionManager : StageManager
    {
        // The singleton instance.
        private static ActionManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("Action")]

        // The action UI.
        public ActionUI actionUI;

        // The action stage list.
        public ActionStageList actionStageList;

        // The action stage.
        public ActionStage actionStage;

        // The total amount of time the stage lasts in seconds.
        // The stage lasts 2 minutes (120 seconds).
        // Day and night are 1 minute each (half of the stage).
        public const float STAGE_LENGTH_MAX_SECONDS = 120.0F;

        // The stage day timer, which is used to determine the time of day.
        // This is seperate from 'gameTime' which is the real-world time it takes the player to finish the stage.
        // TODO: loop around to day time.
        private float stageDayTimer = 0.0F;

        // The player user.
        public ActionPlayerUser playerUser;

        // The player enemy.
        public ActionPlayerEnemy playerEnemy;

        // Constructor
        private ActionManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected override void Awake()
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

            base.Awake();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the action UI is not set, find it.
            if(actionUI == null)
            {
                actionUI = ActionUI.Instance;
            }

            // Gets the action stage list.
            if(actionStageList == null)
            {
                actionStageList = ActionStageList.Instance;
            }

            // If the action stage is not set.
            if(actionStage == null)
            {
                actionStage = FindObjectOfType<ActionStage>();
            }

            // Initializes the stage.
            InitializeStage();
        }

        // Gets the instance.
        public static ActionManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<ActionManager>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Action Manager (singleton)");
                        instance = go.AddComponent<ActionManager>();
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

        // Initializes the knowledge stage.
        public override void InitializeStage()
        {
            // Generates the map using the id number.
            actionStage.GenerateMap(idNumber);

            // Sets the energy to max.
            playerEnemy.SetEnergyToMax();

            // Call the base function to mark that the stage has been initialized successfully.
            base.InitializeStage();
        }

        // Returns the stage
        public float GetStageDayTimer()
        {
            return stageDayTimer;
        }

        // Returns 'true' if its day time. Day time is the first half of the stage.
        public bool IsDayTime()
        {
            return stageDayTimer < STAGE_LENGTH_MAX_SECONDS / 2.0F;
        }

        // Returns 'true' if it's night time. Night time is the second half of the stage.
        public bool IsNightTime()
        {
            return stageDayTimer >= STAGE_LENGTH_MAX_SECONDS / 2.0F;
        }

        // Called when the game rolls over to the next day.
        // In other words, the day timer has gone path the stage length time.
        public void OnStageNextDay()
        {
            // TODO: roll over time?
        }

        // Called on the death of the user.
        public void OnPlayerUserDeath()
        {
            OnStageOver();
        }

        // Called on the death of the enemy.
        public void OnPlayerEnemyDeath()
        {
            OnStageOver();
        }

        // Called when the stage is over.
        public void OnStageOver()
        {
            SetStagePlaying(false);
        }

        // Returns 'true' if the stage is complete.
        public override bool IsComplete()
        {
            // TODO: implement
            return false;
        }

        // Called to finish the stage. TODO: implement.
        public override void FinishStage()
        {
            throw new System.NotImplementedException();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the stage is playing and the game isn't paused, adjust the stage day timer.
            if(IsStagePlayingAndGameUnpaused())
            {
                stageDayTimer += Time.deltaTime;

                // If the stage day timer has passed the stage length...
                // Mark that the stage has looped around to the next day.
                if(stageDayTimer > STAGE_LENGTH_MAX_SECONDS)
                {
                    OnStageNextDay();
                }
            }
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected override void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }

            base.OnDestroy();
        }

    }
}