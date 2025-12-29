using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.tvOS;

namespace RM_EDU
{
    // The action enemy unit.
    public class ActionUnitEnemy : ActionUnit
    {
        [Header("Enemy")]
        // The enemy player.
        public ActionPlayerEnemy playerEnemy;

        // The enemy's movement direction.
        // Enemies go from left to right.
        private Vector3 movementDirec = Vector3.left;

        // If 'true', the enemy moves.
        private bool movementEnabled = true;

        // The row the action unit is in.
        private int row = -1;

        // The amount of energy the enemy loses when a death occurs.
        public float deathEnergyCost = 1;

        [HideInInspector]
        // If 'true', the enemy unit checks if it's at the end of the map on the left.
        // Since this happens every frame, there should be some tile that's used to enable this function...
        // Upon the enemy unit coming into contact with it.
        protected bool checkForMapLeftBound = false;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the stage has no metal tiles, check for the map left bound every frame.
            // The metal tiles tell the enemy to check for the end of the map.
            // If there are no metal tiles, this trigger will never be set by them..
            // Since the enemy doesn't know when it's close to the map end without them...
            // It just checks every frame.
            if (!actionManager.actionStage.HasMetalTiles)
                checkForMapLeftBound = true;
        }

        // OnTriggerEnter2D is called when the Collider2D other enters this trigger (2D physics only)
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            // Checks if colliding with a tile.
            ActionTile tile;

            // Tries to get the tile component.
            if(collision.TryGetComponent(out tile))
            {
                // If this is a metal tile, the enemy should be approaching the end of the map.
                // As such, start checking if at the end of the map every frame.
                if(tile.GetTileType() == ActionTile.actionTile.metal)
                {
                    checkForMapLeftBound = true;
                }
            }
        }

        // Gets the unit type.
        public override unitType GetUnitType()
        {
            return unitType.enemy;
        }

        // Returns the row the enemy is in.
        public int GetRow()
        {
            return row;
        }

        // Sets the row the enemy is in.
        public void SetRow(int newRow)
        {
            // Gets the row count.
            int rowCountMax = ActionManager.Instance.actionStage.rowEnemyUnits.Count;

            // Tries to remove the enemy unit from its row list.
            TryRemoveFromRowList();

            // Set the new row.
            row = newRow;

            // Tries to add to the row list.
            TryAddToRowList();
        }

        // Returns 'true' if the enemy unit is in a row list.
        public bool InRowList()
        {
            bool result = false;

            // If the row index is valid.
            if (row >= 0 && row < actionManager.actionStage.rowEnemyUnits.Count)
            {
                // Checks that the row exists.
                if (actionManager.actionStage.rowEnemyUnits[row] != null)
                {
                    result = actionManager.actionStage.rowEnemyUnits[row].Contains(this);
                }
            }

            return result;
        }

        // Tries to add to the set row list. Returns 'true' if it was added. Won't be added if already in list.
        public bool TryAddToRowList()
        {
            // Gets set to 'true' if added in the list. Won't add if already in list.
            bool added = false;

            // New row is valid.
            if (row >= 0 && row <= actionManager.actionStage.rowEnemyUnits.Count)
            {
                // Checks that the row exists.
                if (actionManager.actionStage.rowEnemyUnits[row] != null)
                {
                    // If the list doesn't contain this enemy unit, add it.
                    if (!actionManager.actionStage.rowEnemyUnits[row].Contains(this))
                    {
                        actionManager.actionStage.rowEnemyUnits[row].Add(this);
                        added = true;
                    }
                }
            }
            // Row is not valid.
            else
            {
                Debug.LogWarning("There is no row " + row.ToString() + " in the row lists for the stage map.");
            }

            return added;
        }

        // Tries to add this to the provided row list. Returns 'true' if it was added. Won't be added if already in list.
        public void TryAddToRowList(int newRow)
        {
            SetRow(newRow);
        }

        // Tries removing this unti enemy from the row list its in.
        // Returns 'true' if it was successfully removed.
        // Returns 'false' if the row didn't exist or if the row didn't contain this enemy.
        public bool TryRemoveFromRowList()
        {
            // Checks if the enemy was removed.
            bool removed = false;

            // If the row index is valid.
            if (row >= 0 && row < actionManager.actionStage.rowEnemyUnits.Count)
            {
                // Checks that the row exists.
                if (actionManager.actionStage.rowEnemyUnits[row] != null)
                {
                    // If the list contains this enemy unit, remove it.
                    if (actionManager.actionStage.rowEnemyUnits[row].Contains(this))
                    {
                        actionManager.actionStage.rowEnemyUnits[row].Remove(this);

                        // Enemy unit removed.
                        removed = true;
                    }
                }
            }

            return removed;
        }

        // Returns true if the entity can use the tile.
        public override bool UsableTile(ActionTile tile)
        {
            // Enemies units can share a tile, and are not locked to a single tile.
            return true;
        }

        // Kills the unit.
        public override void Kill()
        {
            base.Kill();
        }

        // Called when a unit has died/been destroyed.
        public override void OnUnitDeath()
        {
            base.OnUnitDeath();

            // If attached to a player enemy.
            if(playerEnemy != null)
            {
                // Calls the related function.
                playerEnemy.OnEnemyUnitDeath(this);
            }

            // Tries to remove the unit from its row list.
            // This automatically checks if the row list contains this.
            TryRemoveFromRowList();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the stage is playing and the game is unpaused.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // If the enemy should use movement.
                if (movementEnabled)
                {
                    // NOTE: the speed isn't effected by the factor variable.

                    // Calculates the move speed.
                    float moveSpeedAdjusted = movementSpeed / 100.0F * statFactor;


                    // Moves the enemy unit. The enemy shoud move at a fixed speed.
                    // Old - Uses translate function.
                    // transform.Translate(movementDirec * moveSpeedAdjusted * Time.deltaTime);

                    // New - adds force and clamps it to the move speed.
                    rigidbody.AddForce(movementDirec * moveSpeedAdjusted * Time.deltaTime, ForceMode2D.Impulse);
                    rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, moveSpeedAdjusted);
                }
                else
                {
                    // If the rigidbody's velocity is not 0, make it 0.
                    if (rigidbody.velocity != Vector2.zero)
                        rigidbody.velocity = Vector2.zero;
                }

                // If the enemy should check for the end of the map on the left side.
                if (checkForMapLeftBound)
                {
                    // Gets the reference vector.
                    Vector3 refVec = actionManager.actionStage.GetMapWorldPositionReferenceVector(transform.position);

                    // Outside the map on the left side.
                    if (refVec.x < 0.0F)
                    {
                        // The player has died.
                        actionManager.OnPlayerUserDeath();
                    }
                }
                
            }
            // If the stage isn't playing, no actions should be performed.
            else if(!actionManager.IsStagePlaying())
            {
                // If the rigidbody's velocity is not 0, make it 0.
                if (rigidbody.velocity != Vector2.zero)
                    rigidbody.velocity = Vector2.zero;
            }
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Tries to remove this object from the row list.
            TryRemoveFromRowList();
        }
    }
}