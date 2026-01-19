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

        // Initializes the event.
        public override void InitalizeEvent()
        {
            // Sets to the light on sprite.
            worldStage.SetLightSpriteToOnSprite();

            // Call base.
            base.InitalizeEvent();
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

        // Called when the event is completed.
        public override void OnEventComplete()
        {
            // Sets the light off sprite.
            worldStage.SetLightSpriteToOffSprite();

            // Gives the player their rwards.
            worldStage.GivePlayerRewards();

            // Call base.
            base.OnEventComplete();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}