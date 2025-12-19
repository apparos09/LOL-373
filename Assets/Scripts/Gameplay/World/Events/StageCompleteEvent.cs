using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // An event that's completed when a stage is completed.
    public class StageCompleteEvent : GameEvent
    {
        // The world stage.
        public WorldStage worldStage;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the world stage isn't set, set it.
            if (worldStage == null)
                worldStage = GetComponent<WorldStage>();
        }

        // Updates the event.
        public override void UpdateEvent()
        {
            // If the world stage is complete.
            if(worldStage.IsComplete())
            {
                cleared = true;   
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}