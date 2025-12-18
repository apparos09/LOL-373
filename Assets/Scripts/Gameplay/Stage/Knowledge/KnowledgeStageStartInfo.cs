using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The start info for the knowledge stage.
    public class KnowledgeStageStartInfo : StageStartInfo
    {
        [Header("Knowledge")]

        // The statements in the knowledge stage.
        public List<KnowledgeStatement> statements = new List<KnowledgeStatement>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Sets the start info from the provided stage.
        public override void SetStartInfo(WorldStage worldStage)
        {
            // If the world stage is not a knowledge stage, do nothing.
            if (worldStage is not WorldKnowledgeStage)
                return;

            // Calls the base function.
            base.SetStartInfo(worldStage);

            // Convert to a knowledge stage.
            WorldKnowledgeStage knowledgeStage = (WorldKnowledgeStage)worldStage;
        }

        // Applies the start info to the provided manager.
        public override void ApplyStartInfo(GameplayManager manager)
        {
            // If this isn't the right manager type, return.
            if(manager is not KnowledgeManager)
                return;

            // Apply the start info.
            base.ApplyStartInfo(manager);

            // Gets the knowledge manager.
            KnowledgeManager knowledgeManager = (KnowledgeManager)manager;
        }

    }
}