using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The stage manager.
    public abstract class StageManager : GameplayManager
    {
        // The game will skip these overwritten functions in StageManager and go to GameplayUI instead.
        // Uncomment these functions if actual functionaliy is added.

        [Header("Stage")]

        // The stage user interface.
        public StageUI stageUI;

        // The timer for the stage. This is scaled with delta time since it's tied to game events.
        // This is effected by the time scale and is reset if the stage starts over.
        [Tooltip("The stage timer, which is used for some stage events. This is effected by time scale and reset if the stage starts over.")]
        public float stageTimer = 0.0F;

        // The stage's ID number.
        public int idNumber = 0;

        // The world stage index.
        public int worldStageIndex = -1;

        // The stage's difficulty.
        public int difficulty = 0;

        // The natural resources that will be used.
        public List<NaturalResources.naturalResource> naturalResources = new List<NaturalResources.naturalResource>();

        // Gets set to 'true' if the stage is initialized.
        protected bool stageInitialized = false;

        // Gets set to 'true' when the stage is active/running.
        private bool stagePlaying = false;

        // NOTE: this is not called in the Start() function of StageManager() since some other things...
        // Need to be initialized first.

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Finds the stage ui if it isn't set.
            if (stageUI == null)
                stageUI = FindObjectOfType<StageUI>();

            
        }

        // Initializes the stage.
        public virtual void InitializeStage()
        {
            stageInitialized = true;

            stagePlaying = true;
        }

        // Returns 'true' if the stage has been initialized.
        public bool StageInitialized
        {
            get { return stageInitialized; }
        }

        // Sets the natural resource list to the resource type list.
        public virtual void SetNaturalResourceListToTypeList(bool includeUnknown = false)
        {
            naturalResources.Clear();
            naturalResources = NaturalResources.GenerateNaturalResourceTypeList(includeUnknown);
        }

        // Returns 'true' if the stage is playing.
        public bool IsStagePlaying()
        {
            return stagePlaying;
        }

        // Sets if the stage is playing or not.
        public void SetStagePlaying(bool stagePlaying)
        {
            this.stagePlaying = stagePlaying;
        }

        // Returns 'true' if the stage is playing and the game is unpaused.
        public bool IsStagePlayingAndGameUnpaused()
        {
            return IsStagePlaying() && !IsGamePaused();
        }

        // Gets the stage type.
        public WorldStage.stageType GetStageType()
        {
            // Checks the manager to see what type of stage it is.
            if(this is ActionManager)
            {
                return WorldStage.stageType.action;
            }
            else if(this is KnowledgeManager)
            {
                return WorldStage.stageType.knowledge;
            }
            else
            {
                return WorldStage.stageType.unknown;
            }
        }

        // Returns the stage time.
        public virtual float GetStageTimer()
        {
            return stageTimer;
        }

        // Resets the game timer and stage timer.
        public void ResetGameTimerAndStageTimer()
        {
            ResetGameTimer();
            ResetStageTimer();
        }

        // Resets the stage timer.
        // NOTE: this doesn't reset gameTimer. The stage timer is effected by time scale, but the game timer is not.
        // If you want to reset the game timer, call ResetStageTimer().
        public virtual void ResetStageTimer()
        {
            stageTimer = 0.0F;
        }

        // Returns the stage score.
        public virtual float GetStageScore()
        {
            // Returns the game score.
            return gameScore;
        }

        // Calculates the stage score.
        public abstract float CalculateStageScore();

        // Calculates and sets the game score.
        public void CalculateAndSetGameScore()
        {
            gameScore = CalculateStageScore();
        }

        // Resets the stage score.
        public void ResetStageScore()
        {
            gameScore = 0;
        }

        // Gets the stage energy total.
        public abstract float GetStageEnergyTotal();

        // Gets the stage's air pollution amount, which only pertains to the action stage.
        public virtual float GetStageAirPollution()
        {
            return 0.0F;
        }

        // Returns 'true' if the stage is complete.
        public abstract bool IsComplete();

        // Called when the stage is over.
        public virtual void OnStageOver()
        {
            // Stage no longer playing.
            SetStagePlaying(false);

            // Calculates the game score.
            CalculateAndSetGameScore();
        }

        // Resets the stage.
        public virtual void ResetStage()
        {
            // Resets the tiem and the score.
            ResetGameTimerAndStageTimer();
            ResetStageScore();
        }

        // Finishes the stage.
        public virtual void FinishStage()
        {
            // Reset the game time scale to make sure it's 1.00.
            ResetGameTimer();
            ResetGameTimeScale(true);
            ResetStageTimer();

            // Stop running the game timer so that the time is measured accurately.
            PauseGameTimer();

            // Stage is no longer playing.
            SetStagePlaying(false);
        }

        // Generates the stage data.
        public WorldStage.WorldStageData GenerateStageData(bool complete)
        {
            WorldStage.WorldStageData data = new WorldStage.WorldStageData();

            data.worldStageIndex = worldStageIndex;
            data.idNumber = idNumber;

            data.stageType = GetStageType();
            // TODO: review where the game score is being gotten from.
            data.score = GetStageScore();

            data.complete = complete;

            return data;
        }

        // Generates a world start info object.
        // dontDestroyOnLoad: marks the object as not being destroyed on load if true.
        public WorldStartInfo GenerateWorldStartInfo(bool dontDestroyOnLoad)
        {
            // Creates a start info object and adds the world info component.
            GameObject startInfoObject = new GameObject("World Start Info");
            WorldStartInfo startInfo = startInfoObject.AddComponent<WorldStartInfo>();

            // Applies the start info from this manager.
            startInfo.SetStartInfo(this);

            // If the object shouldn't be destroyed on load, mark the object as such.
            if(dontDestroyOnLoad)
            {
                DontDestroyOnLoad(startInfo.gameObject);
            }

            return startInfo;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the stage is playing and the game is unpaused...
            if(IsStagePlayingAndGameUnpaused())
            {
                stageTimer += Time.deltaTime;
            }
        }
    }
}