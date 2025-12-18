using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Holds the information needed to start a given area of the game (world, action, knowledge).
    public abstract class GameplayStartInfo : MonoBehaviour
    {
        // The resources that will be used.
        public List<NaturalResources.naturalResource> naturalResources = new List<NaturalResources.naturalResource>();

        // The difficulty, which goes from 1-9. A difficulty of (0) is the default value.
        public int difficulty = 0;

        // Sets the start info using the provided world stage.
        public virtual void SetStartInfo(WorldStage worldStage)
        {
            // Set the difficulty.
            difficulty = worldStage.difficulty;

            // Set the resources.
            naturalResources.Clear();
            naturalResources.AddRange(worldStage.naturalResources);
        }

        // Applies the start info to the provided manager.
        public abstract void ApplyStartInfo(GameplayManager manager);
        

    }
}