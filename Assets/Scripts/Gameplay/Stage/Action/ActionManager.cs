using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
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
        public float dayNightTimer = 0.0F;

        // The fade duration for transitioning from day to night.
        private float dayNightTransDur = 5.0F;

        // If 'true', the stage is progressing from day to night.
        // If 'false', the stage is progerssing from night to day.
        private bool dayToNight = true;

        // If 'true', the day-night cycle loops. When the end of a day-night period is reached...
        // The timer resets, and the game marks that it's going from night to day.
        // If this is 'false', the timer continues without adjusting the day-night cycle at all.
        private bool loopDayNightCycle = false;

        // If 'true', the day-night cycle is enabled.
        private bool dayNightEnabled = true;

        // The player user.
        public ActionPlayerUser playerUser;

        // The player enemy.
        public ActionPlayerEnemy playerEnemy;

        // The action post processor.
        public ActionPostProcessor postProcessor;

        // If 'true', the game uses post processing.
        private bool usePostProcessing = true;

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
        public float GetDayNightTimer()
        {
            return dayNightTimer;
        }

        // Gets the stage time progress as a percentage based on how long the stage has been going.
        public float GetDayNightTimerProgress()
        {
            return dayNightTimer / STAGE_LENGTH_MAX_SECONDS;
        }

        // Returns 'true' if the day-night cycle is in the first half of the timer (< 50).
        // Check dayToNight to see if it's day to night or night to day.
        private bool IsDayNightProgressInFirstHalf()
        {
            return dayNightTimer < STAGE_LENGTH_MAX_SECONDS / 2.0F;
        }

        // Returns 'true' if the day-night cycle is in the second half of the timer (>= 50%).
        // Check dayToNight to see if it's day to night or night to day.
        private bool IsDayNightProgressInSecondHalf()
        {
            return dayNightTimer >= STAGE_LENGTH_MAX_SECONDS / 2.0F;
        }
        
        // Returns true if the stage is going from day to night.
        public bool IsDayToNight()
        {
            return dayToNight;
        }

        // Returns 'true' if teh stage is going from night to day.
        public bool IsNightToDay()
        {
            return !dayToNight;
        }

        // Returns 'true' if its day time. Day time is the first half of the stage.
        public bool IsDayTime()
        {
            // If the stage is transitioning from day to night, check if it's in the first half of the cycle (day).
            // If the stage is transitioning from night to day, check if it's in the second half of the cycle (day).
            return (dayToNight) ? IsDayNightProgressInFirstHalf() : IsDayNightProgressInSecondHalf();
        }

        // Returns 'true' if it's night time. Night time is the second half of the stage.
        public bool IsNightTime()
        {
            // If the stage is transitioning from day to night, check if it's in the second half of the cycle (night).
            // If the stage is transitioning from night to day, check if it's in the first half of the cycle (night).
            return (dayToNight) ? IsDayNightProgressInSecondHalf() : IsDayNightProgressInFirstHalf();
        }

        // Does the day night cycle loop? If not, it stops when it becomes night.
        public bool IsDayNightCycleLooping()
        {
            return loopDayNightCycle;
        }

        // Gets the day night transition duration.
        public float GetDayNightTransitionDuration()
        {
            return dayNightTransDur;
        }

        // Returns 'true' if the day night cycle is in transition.
        public bool InDayNightTransition()
        {
            // Calculates the end of the transition and the start of the transition.
            // Keep in mind that the day night timer counts up.
            // TODO: make this a dedicated variable?
            float transEnd = STAGE_LENGTH_MAX_SECONDS / 2.0F;
            float transStart = transEnd - dayNightTransDur;

            // Gets the result.
            bool result = dayNightTimer >= transStart && dayNightTimer <= transEnd;

            return result;
        }

        // Gets the day-night transition value.
        // If it's below the transition point, it's 0.0F. If it's above the transition point, it's 1.0F.
        public float GetDayNightTransitionValue()
        {
            // The transition end and start.
            float transEnd = STAGE_LENGTH_MAX_SECONDS / 2.0F;
            float transStart = transEnd - dayNightTransDur;

            // Calculates the result.
            float result = Mathf.InverseLerp(transStart, transEnd, dayNightTimer);

            // Clamps it into 01 space.
            result = Mathf.Clamp01(result);

            // Returns the result.
            return result;
        }


        // Updates the day-night effect.
        public void UpdateDayNightEffect()
        {
            // If the post processor isn't set, do nothing.
            if (postProcessor == null)
                return;

            // Gets the day-night transition value.
            float t = Mathf.Clamp01(GetDayNightTransitionValue());

            // Sets t-value.
            postProcessor.lerpT = t;
        }

        // Called when the day-night timer has finished, which rolls the timer over to another day.
        public void OnDayNightTimerFinished()
        {
            // Checks if the day night cycle should be looped or not.
            if(loopDayNightCycle) // Roll over to the next day.
            {
                dayNightTimer = 0.0F; // Set timer to 0.
                dayToNight = !dayToNight; // Swap cycle.
            }
            else // Clamp the timer since day-night cycle is over.
            {
                // Stop the timer from going past the stage length.
                if (dayNightTimer > STAGE_LENGTH_MAX_SECONDS)
                    dayNightTimer = STAGE_LENGTH_MAX_SECONDS;
            }
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

            // Open the end UI.
            actionUI.SetStageEndWindowActive(true);
        }

        // Returns 'true' if the stage is complete.
        public override bool IsComplete()
        {
            // TODO: implement
            return false;
        }

        // Resets the acion stage.
        public void ResetStage()
        {
            playerUser.ClearSelectedUnitPrefab();
        }

        // Called to finish the stage. TODO: implement.
        public override void FinishStage()
        {
            base.FinishStage();

            // Open the window.
            // actionUI.SetStageEndWindowActive(true);

            // TODO: put this into the UI instead.
            LoadWorldScene();

        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the stage is playing and the game isn't paused, adjust the stage day timer.
            if(IsStagePlayingAndGameUnpaused())
            {
                // If the day night cycle is enabled.
                if(dayNightEnabled)
                {
                    dayNightTimer += Time.deltaTime;

                    // If the stage day timer has passed the stage length...
                    // Mark that the timer has finished.
                    if (dayNightTimer >= STAGE_LENGTH_MAX_SECONDS)
                    {
                        OnDayNightTimerFinished();
                    }
                    else
                    {
                        // If post processing is being used.
                        if (usePostProcessing)
                        {
                            // Update the day-night effect.
                            UpdateDayNightEffect();
                        }
                    }
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