using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The user action player.
    public class ActionPlayerUser : ActionPlayer
    {
        [Header("User")]

        // The energy the player user starts with.
        public float startingEnergy = 50.0F;

        // The energy the player had on the previous update.
        // Used to see if the player's energy has changed between updates.
        private float prevUpdateEnergy = -1.0F;

        // The energy auto generation timer.
        public float energyAutoGenTimer = 0.0F;

        // The amount of time it takes for the player to generate energy.
        public float energyAutoGenTimerMax = 5.0F;

        // The amount of energy that's generated for every instance.
        public float energyAutoGenAmount = 5.0F;

        // If 'true', the player automatically generates energy.
        private bool energyAutoGenEnabled = true;

        // The unit prefab the action player user has selected.
        private ActionUnit selectedUnitPrefab;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the player user is null, set it to this.
            if(actionManager.playerUser == null)
            {
                actionManager.playerUser = this;
            }

            // Sets the user's energy to the starting energy.
            SetEnergyToStartingEnergy();

            // Resets the energy auto generation timer.
            ResetEnergyAutoGenerationTimer();
        }

        // Sets the starting energy.
        public void SetStartingEnergy(float startingEnergy)
        {
            this.startingEnergy = startingEnergy;
        }

        // Sets the user's energy to their starting energy.
        public void SetEnergyToStartingEnergy()
        {
            energy = startingEnergy;
        }

        // Adds the starting energy to the user's energy.
        public void AddStartingEnergyToEnery()
        {
            energy += startingEnergy;
        }

        // Returns 'true' if the energy auto generates.
        public bool IsEnergyAutoGenerating()
        {
            return energyAutoGenEnabled;
        }

        // Gets the energy auto generation timer max.
        public float GetEnergyAutoGenerationTimerMax()
        {
            return energyAutoGenTimerMax;
        }

        // Gets the energy auto generation timer.
        public float GetEnergyAutoGenerationTimer()
        {
            return energyAutoGenTimer;
        }

        // Resets the energy auto generation timer.
        public void ResetEnergyAutoGenerationTimer()
        {
            energyAutoGenTimer = energyAutoGenTimerMax;
        }

        // Gets the amount of energy generated automatically per instance.
        public float GetEnergyAutoGenerationAmount()
        {
            return energyAutoGenAmount;
        }

        // Called when the energy amount for the user has changed.
        public void OnEnergyChanged()
        {
            // Refreshes the unit buttons.
            actionManager.actionUI.RefreshUnitButtonsInteractable();

            // Sets previous update saved energy to this energy.
            prevUpdateEnergy = energy;
        }

        // Returns 'true' if the player is selecting a unit.
        public bool IsSelectingUnitPrefab()
        {
            return selectedUnitPrefab != null;
        }

        // Gets the unit the player has selected.
        public ActionUnit GetSelectedUnitPrefab()
        {
            return selectedUnitPrefab;
        }

        // Sets the prefab the player has selected. This should be a prefab, not an actual object.
        public void SetSelectedUnitPrefab(ActionUnit unitPrefab)
        {
            selectedUnitPrefab = unitPrefab;
        }

        // Clears the prefab the player has selected.
        public void ClearSelectedUnitPrefab()
        {
            selectedUnitPrefab = null;
        }

        // Resets the player.
        public override void ResetPlayer()
        {
            SetEnergyToStartingEnergy();
            ResetEnergyAutoGenerationTimer();
            ClearSelectedUnitPrefab();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the game is playing and unpaused.
            if (actionManager.IsStagePlayingAndGameUnpaused())
            {
                // If energy should be generated automatically.
                if (energyAutoGenEnabled)
                {
                    // Reduces timer.
                    energyAutoGenTimer -= Time.deltaTime;

                    // If the auto gen timer is less than or equal to 0, add energy.
                    if(energyAutoGenTimer <= 0.0F)
                    {
                        // Sets the timer to 0 to stop it from going negative.
                        energyAutoGenTimer = 0.0F;

                        // Adds energy.
                        energy += energyAutoGenAmount;

                        // Set timer to max.
                        ResetEnergyAutoGenerationTimer();
                    }
                }

                // If the player's energy has changed.
                if(prevUpdateEnergy != energy)
                {
                    OnEnergyChanged();
                }
            }
                

        }
    }
}
