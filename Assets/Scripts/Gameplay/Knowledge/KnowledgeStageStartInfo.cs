using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The start info for the knowledge stage.
    public class KnowledgeStageStartInfo : GameplayStartInfo
    {
        [Header("Knowledge")]

        // The statements in the knowledge stage.
        public List<KnowledgeStatement> statements = new List<KnowledgeStatement>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Applies the start info to the provided manager.
        public override void ApplyStartInfo(GameplayManager manager)
        {
            // If this isn't the right manager type, return.
            if(manager is not KnowledgeManager)
                return;

            // Gets the knowledge manager.
            KnowledgeManager knowledgeManager = (KnowledgeManager)manager;

            // Gives it the natural resources.
            knowledgeManager.naturalResources = naturalResources;

            // Sets the difficulty.
            knowledgeManager.difficulty = difficulty;
        }
    }
}