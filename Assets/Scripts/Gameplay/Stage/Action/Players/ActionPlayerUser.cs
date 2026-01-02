using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RM_EDU
{
    // The user action player.
    public class ActionPlayerUser : ActionPlayer
    {
        // The modes the player user can be in.
        // Select: the player can select action units and place them on the map.
        // Remove: the player can remove action units they've placed on the map.
        public enum playerUserMode { select, remove };

        [Header("User")]

        // The energy the player user starts with.
        public float startingEnergy = 300.0F;

        // The energy the player had on the previous update.
        // Used to see if the player's energy has changed between updates.
        private float prevUpdateEnergy = -1.0F;

        // The energy auto generation timer.
        public float energyAutoGenTimer = 0.0F;

        // The amount of time it takes for the player to generate energy.
        public float energyAutoGenTimerMax = 5.0F;

        // The amount of energy that's generated for every instance.
        public float energyAutoGenAmount = 10.0F;

        // If 'true', the player automatically generates energy.
        private bool energyAutoGenEnabled = true;

        [Header("User/Units")]

        // The generator prefabs the player can use.
        public List<ActionUnitGenerator> generatorPrefabs = new List<ActionUnitGenerator>();

        // The defense prefabs the player can use.
        public List<ActionUnitDefense> defensePrefabs = new List<ActionUnitDefense>();

        // The lane blaster prefab.
        public ActionUnitDefense laneBlasterPrefab;

        // The id of the lane blaster.
        public const int LANE_BLASTER_ID = 1;

        // The player mode of the player.
        private playerUserMode userMode = playerUserMode.select;

        // The unit prefab the action player user has selected.
        // TODO: make private.
        public ActionUnit selectedUnitPrefab;

        // A list of the units created by this player.
        // Only user units can be placed in this list.
        public List<ActionUnitUser> createdUserUnits = new List<ActionUnitUser>();

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the player user is null, set it to this.
            if(actionManager.playerUser == null)
            {
                actionManager.playerUser = this;
            }

            // If the lane blaster is null, set the prefab using the lane blaster id.
            if(laneBlasterPrefab == null)
            {
                laneBlasterPrefab = actionUnitPrefabs.GetDefensePrefab(LANE_BLASTER_ID);
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

        // Sets the generator prefabs from the provided resources.
        public void SetGeneratorPrefabs(List<NaturalResources.naturalResource> resources)
        {
            // The unit list.
            List<ActionUnit> unitList = new List<ActionUnit>();

            // Clears the current list.
            generatorPrefabs.Clear();

            // Gets the prefabs for the provided resources.
            foreach (NaturalResources.naturalResource resource in resources)
            {
                // Gets the prefab.
                ActionUnitGenerator generatorPrefab = ActionUnitPrefabs.Instance.GetGeneratorPrefab((int)resource);

                // Puts the prefab in the generator list and the new list.
                generatorPrefabs.Add(generatorPrefab);
                unitList.Add(generatorPrefab);
            }

            // Adds the unit list to the generator unit selector.
            ActionUI.Instance.generatorUnitSelector.SetActionUnitPrefabs(unitList);
        }

        // Sets the generator prefabs from the manager.
        public void SetGeneratorPrefabsFromManager()
        {
            SetGeneratorPrefabs(ActionManager.Instance.naturalResources);
        }

        // Sets the defense unit prefabs using a list of ids. The defense ids should line up with the indexes.
        public void SetDefensePrefabs(List<int> defenseIds)
        {
            // The unit list.
            List<ActionUnit> unitList = new List<ActionUnit>();

            // Clears the current list.
            defensePrefabs.Clear();

            // Gets the prefabs for all the provided defenses.
            foreach (int defenseId in defenseIds)
            {
                // Gets the prefab.
                // The ID should match up with the index of the defense prefab.
                ActionUnitDefense defensePrefab = ActionUnitPrefabs.Instance.GetDefensePrefab(defenseId);

                // Puts the prefab in the defense list and the new unit list.
                defensePrefabs.Add(defensePrefab);
                unitList.Add(defensePrefab);
            }

            // Adds the unit list to the defense unit selector.
            ActionUI.Instance.defenseUnitSelector.SetActionUnitPrefabs(unitList);
        }

        // Sets the defense prefabs from the manager.
        public void SetDefensePrefabsFromManager()
        {
            SetDefensePrefabs(ActionManager.Instance.userDefenseIds);
        }

        // MODES
        // Returns the user mode.
        public playerUserMode GetPlayerUserMode()
        {
            return userMode;
        }

        // Sets the player user mode.
        public void SetPlayerUserMode(playerUserMode newMode)
        {
            userMode = newMode;

            // Update UI based on what mode the user is in.
            switch(userMode)
            {
                case playerUserMode.select:
                    ActionUI.Instance.SetSelectedUnitInfoToSelect();
                    break;

                case playerUserMode.remove:
                    ActionUI.Instance.SetSelectedUnitInfoToRemove(); // Set UI to remove.
                    ActionManager.Instance.actionStage.UnhighlightAllTiles(); // Makes sure all tiles are unhighlighted.
                    break;
            }
        }

        // Returns true if the user is in select mode.
        public bool InSelectMode()
        {
            return userMode == playerUserMode.select;
        }

        // Sets the player to select mode.
        public void SetToSelectMode()
        {
            SetPlayerUserMode(playerUserMode.select);
        }

        // Returns true if the user is in remove mode.
        public bool InRemoveMode()
        {
            return userMode == playerUserMode.remove;
        }

        // Sets to remove mode.
        public void SetToRemoveMode()
        {
            SetPlayerUserMode(playerUserMode.remove);
        }

        // SELECT / INSTANTIATE
        // Returns 'true' if the player is selecting a unit.
        public bool IsSelectingActionUnitPrefab()
        {
            return selectedUnitPrefab != null;
        }

        // Returns 'true' if the user selecting a unit prefab of a certain type.
        public bool IsSelectingActionUnitTypePrefab(ActionUnit.unitType type)
        {
            if (selectedUnitPrefab != null)
                return selectedUnitPrefab.GetUnitType() == type;
            else
                return false;
        }

        // Returns true if the user is selecting a generator unit prefab.
        public bool IsSelectingGeneratorUnitPrefab()
        {
            return IsSelectingActionUnitTypePrefab(ActionUnit.unitType.generator);
        }

        // Returns true if the user is selecting a defense unit prefab.
        public bool IsSelectingDefenseUnitPrefab()
        {
            return IsSelectingActionUnitTypePrefab(ActionUnit.unitType.defense);
        }

        // Returns true if the user is selecting a enemy unit prefab.
        public bool IsSelectingEnemyUnitPrefab()
        {
            return IsSelectingActionUnitTypePrefab(ActionUnit.unitType.enemy);
        }

        // Gets the unit the player has selected.
        public ActionUnit GetSelectedActionUnitPrefab()
        {
            return selectedUnitPrefab;
        }

        // Sets the prefab the player has selected. This should be a prefab, not an actual object.
        public void SetSelectedActionUnitPrefab(ActionUnit unitPrefab)
        {
            // Set to select mode.
            SetToSelectMode();

            // Set prefab.
            selectedUnitPrefab = unitPrefab;

            // Sets the info.
            ActionUI.Instance.SetSelectedUnitInfo(unitPrefab);

            // Highlights tiles if tile highlighting is enabled.
            ActionManager.Instance.actionStage.OnPlayerUserSelectedUnit();
        }

        // Gets the type of the selected action unit prefab.
        // If not selecting a prefab, it returns unknown.
        public ActionUnit.unitType GetSelectedActionUnitPrefabType()
        {
            if (selectedUnitPrefab != null)
                return selectedUnitPrefab.GetUnitType();
            else
                return ActionUnit.unitType.unknown;
        }

        // Clears the prefab the player has selected.
        public void ClearSelectedActionUnitPrefab()
        {
            // Set to select mode.
            SetToSelectMode();

            // Clear selected prefab.
            selectedUnitPrefab = null;

            // Clears the info.
            ActionUI.Instance.ClearSelectedUnitInfo();

            // Unhighlight tiles if highlighting is enabled.
            ActionManager.Instance.actionStage.OnPlayerUserClearedSelectedUnit();
        }

        // Returns 'true' if the selected action unit can use the tile.
        public bool CanSelectedActionUnitUseTile(ActionTile tile)
        {
            // Checks if their is a selected unit prefab.
            if(selectedUnitPrefab != null)
            {
                return selectedUnitPrefab.UsableTile(tile);
            }
            else
            {
                return false;
            }
        }

        // Instantiates an action unit on the provided tile.
        public ActionUnit InstantiateActionUnit(ActionUnit unitPrefab, ActionTile tile, bool setAsOwner = true, bool applyEnergyCost = true)
        {
            // No prefab so return null.
            if(unitPrefab == null)
            {
                return null;
            }

            // Instantiate the unit.
            ActionUnit newUnit = Instantiate(unitPrefab);

            // Sets the parent.
            SetActionUnitParentToUnitParent(newUnit);

            // Downcasts to a user unit.
            ActionUnitUser userUnit = (newUnit is ActionUnitUser) ? (newUnit as ActionUnitUser) : null;

            // If the user unit isn't equal to null, that means the conversion was successful.
            // As such, do functions specific to this unit.
            if(newUnit != null)
            {
                // If the user should be the owner of this unit.
                if(setAsOwner)
                {
                    newUnit.owner = this;
                }

                // If the tile exists, place the unit on it.
                if (tile != null)
                {
                    // If the tile doesn't have an action user unit, give it the generated one.
                    if (!tile.HasActionUnitUser())
                    {
                        // Sets the action tile to have this unit.
                        tile.SetActionUnitUser(userUnit);
                    }

                    // If the new unit is a user unit.
                    if (newUnit is ActionUnitUser)
                    {
                        // Set to tile position using function.
                        (newUnit as ActionUnitUser).SetPositionToTilePosition(tile);
                    }
                    // Not user unit.
                    else
                    {
                        // Set to tile position.
                        newUnit.transform.position = tile.transform.position;
                    }
                }               
            }

            // Adds to the created user units list if it exists.
            if (userUnit != null)
            {
                createdUserUnits.Add(userUnit);
            }

            // If the energy cost should be applied.
            if(applyEnergyCost)
            {
                // Reduce energy.
                energy -= newUnit.energyCreationCost;

                // Bounds check.
                if (energy < 0.0F)
                    energy = 0.0F;

                // If the selected prefab exists.
                if (selectedUnitPrefab != null)
                {
                    // If the player user doesn't have energy to use their currently selected unit prefab...
                    // Clear the selection.
                    if (!HasEnergyToCreateActionUnit(selectedUnitPrefab))
                    {
                        ClearSelectedActionUnitPrefab();
                    }
                }
            }

            // Returns the new unit.
            return newUnit;
        }

        // Instantiates the selected action unit without putting it on a tile.
        public ActionUnit InstantiateSelectedActionUnit(bool setAsOwner = true, bool applyEnergyCost = true)
        {
            return InstantiateSelectedActionUnit(null, setAsOwner, applyEnergyCost);
        }

        // Instantiates the selected unit and places it on the provided tile.
        public ActionUnit InstantiateSelectedActionUnit(ActionTile tile, bool setAsOwner = true, bool applyEnergyCost = true)
        {
            return InstantiateActionUnit(selectedUnitPrefab, tile, setAsOwner, applyEnergyCost);
        }

        // Instantiates a lane blaster.
        public ActionUnit InstantiateLaneBlaster(bool setAsOwner = true, bool applyEnergyCost = true)
        {
            return InstantiateActionUnit(laneBlasterPrefab, null, setAsOwner, applyEnergyCost);
        }

        // Instantiates a lane blaster on the provided tile.
        public ActionUnit InstantiateLaneBlaster(ActionTile tile, bool setAsOwner = true, bool applyEnergyCost = true)
        {
            return InstantiateActionUnit(laneBlasterPrefab, tile, setAsOwner, applyEnergyCost);
        }

        // Tries to remove the user unit from the created units list.
        public bool TryRemoveCreatedUnitFromList(ActionUnitUser userUnit)
        {
            // If it's in the list, remove it.
            if(createdUserUnits.Contains(userUnit))
            {
                createdUserUnits.Remove(userUnit);
                return true;
            }
            // Not in list.
            else
            {
                return false;
            }
        }

        // Kills all user units.
        public void KillAllUserUnits()
        {
            // Goes through all saved units.
            for (int i = createdUserUnits.Count - 1; i >= 0; i--)
            {
                // Kill the enemy.
                if (createdUserUnits[i] != null)
                    createdUserUnits[i].OnUnitDeath();

            }

            // Clears the list.
            createdUserUnits.Clear();
        }

        // Resets the player.
        public override void ResetPlayer()
        {
            // Kills all the user units, resets the energy, and clears the selected prefab.
            KillAllUserUnits();
            SetEnergyToStartingEnergy();
            ResetEnergyAutoGenerationTimer();
            ClearSelectedActionUnitPrefab(); // Also sets to "select" mode.
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
