using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action stage start info.
    public class ActionStageStartInfo : GameplayStartInfo
    {
        // Sets the start info using the provided world stage.
        public override void SetStartInfo(WorldStage worldStage)
        {
            // If the world stage is not an action stage, return.
            if (worldStage is not WorldActionStage)
                return;

            // Call the base function.
            base.SetStartInfo(worldStage);

            // Convert to a action stage.
            WorldActionStage knowledgeStage = (WorldActionStage)worldStage;
        }

        // Applies the start info.
        public override void ApplyStartInfo(GameplayManager manager)
        {
            // ...
        }
    }
}