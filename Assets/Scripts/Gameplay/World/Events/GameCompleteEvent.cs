using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // The game complete event.
    public class GameCompleteEvent : GameEvent
    {
        // The world manager.
        public WorldManager worldManager;        

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Sets the instance.
            if(worldManager == null)
                worldManager = WorldManager.Instance;
        }

        // Updates the event.
        public override void UpdateEvent()
        {
            // Gets set to 'false' if an uncompleted stage is found.
            bool allComplete = true;

            // Goes through all stages.
            foreach(WorldStage stage in worldManager.stages)
            {
                // If the stage exists.
                if(stage != null)
                {
                    // If a stage isn't complete, then the game is complete.
                    if(!stage.IsComplete())
                    {
                        allComplete = false;
                        break;
                    }
                }
            }

            // Set cleared to allComplete.
            cleared = allComplete;

            // TODO: maybe don't check every frame.
        }

        // Called when the event is completed.
        public override void OnEventComplete()
        {
            base.OnEventComplete();

            worldManager.CompleteGame();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}