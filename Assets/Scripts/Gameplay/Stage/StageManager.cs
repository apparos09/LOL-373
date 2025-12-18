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

        // // Awake is called when the script is being loaded
        // protected override void Awake()
        // {
        //     base.Awake();
        // }
        // 
        // // Start is called before the first frame update
        // protected override void Start()
        // {
        //     base.Start();
        // }
        // 
        // // Update is called once per frame
        // protected override void Update()
        // {
        //     base.Update();
        // }
        // 
        // protected override void OnDestroy()
        // {
        //     base.OnDestroy();
        // }

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