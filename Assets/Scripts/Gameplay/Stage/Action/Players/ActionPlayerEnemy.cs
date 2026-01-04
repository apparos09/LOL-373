using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action player enemy.
    public class ActionPlayerEnemy : ActionPlayer
    {
        [Header("Enemy")]
        // The maximum amount of energy the enemy has.
        [Tooltip("The maximum amount of energy the enemy player can have.")]
        public float energyMax = 300.0F;

        // The enemy's decrementation amount, which reduces from the enemy's energy every frame.
        // When the enemy runs out of energy, the stage is over.
        private float energyDec = 1.0F;

        // Automatically loses energy at a set rate if true.
        [Tooltip("If true, the enemy player loses energy automatically as the stage progresses.")]
        public bool autoEnergyLoss = true;

        // The parent for enemy retreats.
        public GameObject enemyRetreatParent;

        [Header("Enemy/Units")]

        // The rate at which enemies are spawned.
        public float spawnRate = 1.0F;

        // The countdown timer for spawning enemies.
        public float spawnTimer = 0.0F;

        // The spawn time max.
        // This should change based on the game difficulty.
        public float spawnTimeMax = 5.0F;

        // The enemy spawn count minimum. This determines the minimum of enemies to spawn eachi nstance.
        [Tooltip("Minimum number of enemies to spawn at once.")]
        public int enemiesPerSpawnMin = ENEMIES_PER_SPAWN_MIN_DEFAULT;

        // The default enemies per spawn min.
        public const int ENEMIES_PER_SPAWN_MIN_DEFAULT = 1;

        // The enemy spawn count maximum. This determines the maximum of enemies to spawn each instance.
        [Tooltip("Maximum number of enemies to spawn at once.")]
        public int enemiesPerSpawnMax = ENEMIES_PER_SPAWN_MAX_DEFAULT;

        // The default enemies per spawn max.
        public const int ENEMIES_PER_SPAWN_MAX_DEFAULT = 7;

        // If 'true', spawning is allowed.
        private bool allowSpawns = true;

        // The list of usable enemies by their ids in the prefab list.
        // The enemy id number should match the index number.
        [Tooltip("Lists the ids for usable enemies from the prefab list..")]
        public List<int> enemyIds = new List<int>();

        // The action enemy units.
        public List<ActionUnitEnemy> spawnedEnemies = new List<ActionUnitEnemy>();

        // The enemy unit spawn limit.
        public const int ACTIVE_ENEMY_UNIT_LIMIT = ActionStage.MAP_ROW_COUNT_DEFAULT * ActionStage.MAP_COLUMN_COUNT_DEFAULT;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the player enemyis null, set it to this.
            if (actionManager.playerEnemy == null)
            {
                actionManager.playerEnemy = this;
            }

            // Sets the enemy to max, sets the spaw timer to max, and sets to use all enemy prefabs.
            SetEnergyToMax();
            SetSpawnTimerToMax();

            // If there are no usable enemy ids, fill the list with all enemies.
            if(enemyIds.Count <= 0)
                SetEnemyPrefabsToAll(false); 
        }

        // Applies the game difficulty to the enemy.
        // resetValues: if true, reset the current values to match the current difficulty.
        public void ApplyDifficulty(int difficulty, bool resetValues)
        {
            // Clamps the difficulty from 0 to 9.
            float enemyDiff = Mathf.Clamp(difficulty, 0, 9);

            // Clears the enemy ids.
            enemyIds.Clear();

            // Checks the enemy difficulty.
            switch(enemyDiff)
            {
                case 1:
                    energyMax = 200.0F;
                    spawnTimeMax = 9.0F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 3;

                    enemyIds.Add(1);

                    break;

                case 2:
                    energyMax = 212.5F;
                    spawnTimeMax = 8.5F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 3;

                    enemyIds.Add(1);

                    break;

                case 3:
                    energyMax = 225.0F;
                    spawnTimeMax = 8.0F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 4;

                    enemyIds.Add(1);

                    break;

                case 4:
                    energyMax = 237.5F;
                    spawnTimeMax = 7.5F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 4;

                    enemyIds.Add(1);
                    enemyIds.Add(2);

                    break;

                case 5:
                    energyMax = 250.0F;
                    spawnTimeMax = 7.0F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 5;

                    enemyIds.Add(1);
                    enemyIds.Add(2);

                    break;

                case 6:
                    energyMax = 263.5F;
                    spawnTimeMax = 6.5F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 5;

                    enemyIds.Add(1);
                    enemyIds.Add(2);

                    break;

                case 7:
                    energyMax = 275.0F;
                    spawnTimeMax = 6.0F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 6;

                    enemyIds.Add(1);
                    enemyIds.Add(2);
                    enemyIds.Add(3);

                    break;

                case 8:
                    energyMax = 287.5F;
                    spawnTimeMax = 5.5F;

                    enemiesPerSpawnMin = 1;
                    enemiesPerSpawnMax = 6;

                    enemyIds.Add(1);
                    enemyIds.Add(2);
                    enemyIds.Add(3);

                    break;

                default: // Max/Main Difficulty
                case 0:
                case 9:
                    // Energy and spawn time.
                    energyMax = 300.0F;
                    spawnTimeMax = 5.0F;

                    // Enemies per spawn min and max.
                    enemiesPerSpawnMin = ENEMIES_PER_SPAWN_MIN_DEFAULT;
                    enemiesPerSpawnMax = ENEMIES_PER_SPAWN_MAX_DEFAULT;

                    // Give the enemy player all the enemy units.
                    SetEnemyPrefabsToAll(false);
                    break;
            }

            // If values should be reset based on the new difficulty.
            if(resetValues)
            {
                // Sets the values to max.
                SetEnergyToMax();
                SetSpawnTimerToMax();
            }
        }

        // Gets the difficulty from the manager and uses that to apply the settings.
        // If 'resetValues' are true, the relevant parameters are adjusted to their new defaults.
        public void ApplyDifficulty(bool resetValues)
        {
            ApplyDifficulty(ActionManager.Instance.difficulty, resetValues);
        }

        // Sets the enemy prefabs from the list
        public void SetEnemyPrefabs(List<int> newIds)
        {
            // Clears the current list.
            enemyIds.Clear();

            // Sets to the new ids.
            if (newIds != null)
                enemyIds = newIds;
        }

        // Sets the usable enemy prefabs list to include all enemies.
        public void SetEnemyPrefabsToAll(bool include0)
        {
            SetEnemyPrefabs(ActionUnitPrefabs.Instance.GenerateEnemyPrefabIdList(include0));
        }

        // Increases the energy by the provided amount.
        // The decrease function calls this function, so it doesn't need to be overrided.
        public override void IncreaseEnergy(float energyPlus)
        {
            // Call base function.
            base.IncreaseEnergy(energyPlus);

            // Clamp energy within 0 and max.
            energy = Mathf.Clamp(energy, 0, energyMax);
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
            // It's also multiplied by delta time to know how much to reduce it by.
            float result = energyMax / ActionManager.STAGE_LENGTH_MAX_SECONDS * Time.deltaTime;

            return result;
        }

        // Calculates and sets the energy decrement amount.
        public void CalculateAndSetEnergyDecrementAmount()
        {
            energyDec = CalculateEnergyDecrementAmount();
        }

        // Resets the spawn timer.
        public void SetSpawnTimerToMax()
        {
            spawnTimer = spawnTimeMax;
        }

        // Returns the number of spawned enemy units.
        public int GetSpawnedEnemyUnitCount()
        {
            return spawnedEnemies.Count;
        }

        // Returns 'ture' if the enemy is at or above their active enemy unit limit.
        public bool IsBelowActiveEnemyUnitLimit()
        {
            return spawnedEnemies.Count < ACTIVE_ENEMY_UNIT_LIMIT;
        }

        // Spawns enemies.
        public void SpawnEnemyUnits()
        {
            // The number of enemies in each row.
            // By default, it's length is equal to the number of rows. All indexes are filled by 0 by default.
            List<int> rowEnemyCount = new List<int>(new int[actionManager.actionStage.MapRowCount]);

            // Gets the number of spawns.
            int spawns = Random.Range(enemiesPerSpawnMin, enemiesPerSpawnMax + 1);

            // A temporary list of spawned enemies.
            List<ActionUnitEnemy> tempList = new List<ActionUnitEnemy>();

            // While spawns is greater than 0 and the enemy is below the active enemy unit limit.
            while(spawns > 0 && IsBelowActiveEnemyUnitLimit())
            {
                // Gets the prefab using the enemy id.
                int idIndex = Random.Range(0, enemyIds.Count);
                ActionUnitEnemy prefab = actionUnitPrefabs.GetEnemyPrefabById(enemyIds[idIndex]);

                // Prefab exists.
                if(prefab != null)
                {
                    // Creates the new enemy, making a child of this object by default.
                    ActionUnitEnemy enemyUnit = Instantiate(prefab, transform);

                    // This enemy player owns this unit.
                    enemyUnit.owner = this;

                    // Sets the parent.
                    SetActionUnitParentToUnitParent(enemyUnit);

                    // Gives the enemy unit the action manager since it won't be set...
                    // Before certain functions are used.
                    enemyUnit.actionManager = ActionManager.Instance;

                    // Gets a random row.
                    int row = Random.Range(0, actionManager.actionStage.MapRowCount);

                    // The column should be the end of the map.
                    int col = actionManager.actionStage.MapColumnCount - 1;

                    // Gives enemy the values.
                    // Gets the enemy position.
                    Vector3 enemyPos = actionManager.actionStage.ConvertMapPositionToWorldUnits(col, row);

                    // Saves the row this enemy is in and increases the row count.
                    enemyUnit.SetRow(row);
                    rowEnemyCount[row] += 1;

                    // Adjust the enemy's position by how many enemies are in that row.
                    // This moves it off-screen and prevents enemies from overlapping each other.
                    enemyPos.x += rowEnemyCount[row] * actionManager.actionStage.TileSize.x;

                    // Give enemy their position.
                    enemyUnit.transform.position = enemyPos;

                    // Mark enemy unit as belonging to this enemy and add to the list of spawned enemies.
                    enemyUnit.playerEnemy = this;
                    spawnedEnemies.Add(enemyUnit);
                }

                spawns--;
            }


            // Reset the spawn timer.
            SetSpawnTimerToMax();
        }

        // Destroys all enemy units.
        // TODO: add check to see if the death state should be used.
        public void KillAllEnemyUnits()
        {
            // Destroys all enemy units spawned by this enenmy.
            for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
            {
                // Kills the spawned enemy.
                // This function leds to another function where the enemy removes itself from the list.
                spawnedEnemies[i].Kill();
            }

            // Clears all spawned enemies.
            spawnedEnemies.Clear();
        }

        // Called when an enemy unit has been killed.
        public void OnEnemyUnitDeath(ActionUnitEnemy enemyUnit)
        {
            // If the enemy unit is in the list of spawned enemies, remove it.
            if(spawnedEnemies.Contains(enemyUnit))
                spawnedEnemies.Remove(enemyUnit);

            // TODO: have enemy run back to ship before it's destroyed.

            Destroy(enemyUnit.gameObject);
        }

        // Resets the player.
        public override void ResetPlayer()
        {
            // Kills all enemy units, sets the energy to max, and resets the spawn timer to max.
            KillAllEnemyUnits();
            SetEnergyToMax();
            SetSpawnTimerToMax();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the game is playing and the game is unpaused, run the enemy actions.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // If energy should be automatically lost as time progresses.
                if(autoEnergyLoss)
                {
                    // Reduces the energy.
                    energy -= energyDec * Time.deltaTime;
                }

                // Bounds check.
                if (energy < 0.0F)
                    energy = 0.0F;

                // If the enemy has no energy left, the stage is over.
                if(energy <= 0.0F)
                {
                    actionManager.OnPlayerUserDeath();
                }
                // The enemy still has energy.
                else
                {
                    // If spawns are allowed.
                    if(allowSpawns)
                    {
                        spawnTimer -= Time.deltaTime;

                        // If the spawn timer has run out, generate spawns.
                        if(spawnTimer <= 0.0F)
                        {
                            spawnTimer = 0.0F;

                            // If the enemy is below the active enemy unit list limit...
                            // Spawn enemies.
                            if(IsBelowActiveEnemyUnitLimit())
                            {
                                SpawnEnemyUnits();
                            }
                        }
                    }
                    
                }
            }
        }
    }
}