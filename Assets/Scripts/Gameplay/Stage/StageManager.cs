using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace RM_EDU
{
    // The stage manager.
    public abstract class StageManager : GameplayManager
    {
        // The game will skip these overwritten functions in StageManager and go to GameplayUI instead.
        // Uncomment these functions if actual functionaliy is added.

        [Header("Stage")]
        // The stage's ID number.
        public int idNumber = 0;

        // The world stage index.
        public int worldStageIndex = -1;

        // The stage's difficulty.
        public int difficulty = 0;

        // The natural resources that will be used.
        public List<NaturalResources.naturalResource> naturalResources = new List<NaturalResources.naturalResource>();

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
        public virtual float GetStageTime()
        {
            return gameTimer;
        }

        // Returns the stage score.
        public virtual float GetStageScore()
        {
            // Returns the game score.
            return gameScore;
        }

        // Returns 'true' if the stage is complete.
        public abstract bool IsComplete();

        // Finishes the stage.
        public virtual void FinishStage()
        {
            // Reset the game time scale to make sure it's 1.00.
            ResetGameTimeScale();

            // TODO: maybe have the stage timer be seperate from the game timer?

            // Stop running the game timer so that the time is measured accurately.
            PauseGameTimer();
        }

        // Generates the stage data.
        public WorldStage.WorldStageData GenerateStageData(bool complete)
        {
            WorldStage.WorldStageData data = new WorldStage.WorldStageData();

            data.worldStageIndex = worldStageIndex;
            data.idNumber = idNumber;

            data.stageType = GetStageType();
            // TODO: review where the game score is being gotten from.
            data.score = gameScore;

            data.complete = complete;

            return data;
        }
    }
}