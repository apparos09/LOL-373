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
        public float energyMax = 120.0F;

        // The enemy's decrementation amount, which reduces from the enemy's energy every frame.
        // When the enemy runs out of energy, the stage is over.
        private float energyDec = 1.0F;

        // The rate at which enemies are spawned.
        public float spawnRate = 1.0F;

        // The countdown timer for spawning enemies.
        public float spawnTimer = 0.0F;

        // The spawn time max.
        // This should change based on the game difficulty.
        public float spawnTimeMax = 4.0F;

        // The enemy spawn count minimum. This determines the minimum of enemies to spawn eachi nstance.
        [Tooltip("Minimum number of enemies to spawn at once.")]
        public int enemiesPerSpawnMin = 1;

        // The enemy spawn count maximum. This determines the maximum of enemies to spawn each instance.
        [Tooltip("Maximum number of enemies to spawn at once.")]
        public int enemiesPerSpawnMax = 7;

        // If 'true', spawning is allowed.
        private bool allowSpawns = true;

        // The list of usable enemies by their indexes in the prefab list.
        // The enemy id number should match the index number.
        [Tooltip("Lists the indexes for usable enemies from the prefab list. These indexes should be the same as the enemy ids in said slots.")]
        public List<int> usableEnemyIndexes = new List<int>();

        // The action enemy units.
        public List<ActionUnitEnemy> spawnedEnemies = new List<ActionUnitEnemy>();

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the player enemyis null, set it to this.
            if (actionManager.playerEnemy == null)
            {
                actionManager.playerEnemy = this;
            }

            // Sets the enemy to max and resets the spwn timer.
            SetEnergyToMax();
            ResetSpawnTimer();

            // If the list is empty, fill it with every enemy unit.
            if(usableEnemyIndexes.Count <= 0)
            {
                // For every valid enemy in the list, add the id number as usable.
                // Note: the enemy at index 0 is a debug enemy that shouldn't be used.
                // That's why index 0 is skipped.
                for(int i = 1; i < actionUnitPrefabs.enemyPrefabs.Count; i++)
                {
                    // If there is an enemy prefab, get the id number.
                    if (actionUnitPrefabs.enemyPrefabs[i] != null)
                    {
                        // Adds as a usable index.
                        usableEnemyIndexes.Add(i);
                    }
                }
            }
        }

        // Applies the game difficulty to the enemy.
        public void ApplyDifficulty(int difficulty)
        {

        }

        // Gets the difficulty from the manager and uses that to apply the settings.
        public void ApplyDifficulty()
        {
            ApplyDifficulty(actionManager.difficulty);
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
        public void ResetSpawnTimer()
        {
            spawnTimer = spawnTimeMax;
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

            // While spawns is greater than 0.
            while(spawns > 0)
            {
                // Gets the prefab.
                int prefabIndex = Random.Range(0, usableEnemyIndexes.Count);
                ActionUnitEnemy prefab = actionUnitPrefabs.GetEnemyPrefab(usableEnemyIndexes[prefabIndex]);

                // Prefab exists.
                if(prefab != null)
                {
                    // Creates the new enemy, making a child of this object by default.
                    ActionUnitEnemy enemyUnit = Instantiate(prefab, transform);

                    // If there's a dedicated unit parent, give it that as the parent.
                    if (unitParent != null)
                        enemyUnit.transform.parent = unitParent.transform;

                    // Gets a random row.
                    int row = Random.Range(0, actionManager.actionStage.MapRowCount);

                    // The column should be the end of the map.
                    int col = actionManager.actionStage.MapColumnCount - 1;

                    // Gives enemy the values.
                    // Gets the enemy position.
                    Vector3 enemyPos = actionManager.actionStage.ConvertMapPositionToWorldUnits(col, row);

                    // Saves the row this enemy is in and increases the row count.
                    enemyUnit.row = row;
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
            ResetSpawnTimer();
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
            // Kills all the enemy units.
            KillAllEnemyUnits();

            // Sets the energy to max.
            SetEnergyToMax();
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
                            SpawnEnemyUnits();
                        }
                    }
                    
                }
            }
        }
    }
}