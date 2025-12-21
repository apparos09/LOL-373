using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action player enemy.
    public class ActionPlayerEnemy : ActionPlayer
    {
        // The maximum amount of energy the enemy has.
        public float energyMax = 120.0F;

        // The enemy's decrementation amount, which reduces from the enemy's energy every frame.
        // When the enemy runs out of energy, the stage is over.
        private float energyDec = 1.0F;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the player enemyis null, set it to this.
            if (actionManager.playerEnemy == null)
            {
                actionManager.playerEnemy = this;
            }

            // Sets the enemy to max.
            SetEnergyToMax();
        }

        // Gets the energy max.
        public float GetEnergyMax()
        {
            return energyMax;
        }

        // Sets the energy max.
        public void SetEnergyMax(float energyMax)
        {
            this.energyMax = energyMax;

            // Bounds check.
            if (energyMax < 0)
                energyMax = 0.0F;
        }

        // Sets the enemy's energy to its max.
        public void SetEnergyToMax()
        {
            energy = energyMax;
        }


        // Returns the energy decrement amount.
        // This is the amount of energy lost per frame, which is multiplied by delta time.
        public float GetEnergyDecrementAmount()
        {
            return energyDec;
        }

        // Calculates the energy decrement amount.
        public float CalculateEnergyDecrementAmount()
        {
            // The maximum length of the stage is used...
            // To determine how much energy the enemy should lose every second.
            float result = energyMax / ActionManager.STAGE_LENGTH_SECONDS;

            return result;
        }

        // Calculates and sets the energy decrement amount.
        public void CalculateAndSetEnergyDecrementAmount()
        {

        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the game is playing and the game is unpaused, run the enemy actions.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // Reduces the energy.
                energy -= energyDec * Time.deltaTime;

                // Bounds check.
                if (energy < 0)
                    energy = 0.0F;

                // TODO: update UI

                // If the enemy has no energy left, the stage is over.
                if(energy <= 0.0F)
                {
                    actionManager.OnPlayerDeath(this);
                }
                // The enemy still has energy.
                else
                {

                }
            }
        }
    }
}