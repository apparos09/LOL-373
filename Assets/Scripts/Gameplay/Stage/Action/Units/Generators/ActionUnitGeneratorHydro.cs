using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RM_EDU
{
    // Action Unit Generator - Hydro
    public class ActionUnitGeneratorHydro : ActionUnitGenerator
    {
        [Header("Generator/Hydro")]

        // The facing direction of the hydro generator.
        public Vector2 facingDirec = Vector2.right;

        // TODO: set facing direction by version.

        // If 'true', the hydro generator is using its restricted configuration.
        private bool restrictConfig = true;

        // The timer used for flooding. When the timer runs out, related tiles are flooded.
        [Tooltip("The countdown timer for nearby tiles being flooded.")]
        public float floodTimer = 0.0F;

        // The maximum of the flood timer.
        public float floodTimerMax = 25.0F;

        // Gets set to 'true' if the generator has flooded related tiles.
        public bool floodedTiles = false;

        // If 'true', the hydro generator can flood tiles.
        public bool floodingEnabled = true;

        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // If the resource is unknown, set it to hydro.
            if (resource == NaturalResources.naturalResource.unknown)
            {
                resource = NaturalResources.naturalResource.hydro;
            }

            // Sets the flood timer to max.
            SetFloodTimerToMax();
        }


        // Checks if the tile configuration is valid.
        public override bool UsableTileConfiguration(ActionTile tile)
        {
            // If the tile configuration isn't restricted, all configurations are valid.
            // If the tile is null, this also returns true, since the game can't find any...
            // Reference tiles for applying the restricted configuration.
            if (!restrictConfig || tile == null)
            {
                return true;
            }

            // The result to be returned.
            // Will be set to false if a nearby hydro or wind generator is found.
            bool result = true;

            // Gets the stage.
            ActionStage stage = ActionManager.Instance.actionStage;

            // The tile's row and column positions.
            int tileRow = tile.GetMapRowPosition();
            int tileCol = tile.GetMapColumnPosition();

            // Gets set to true if this tile is next to a hydro generator.
            // If it is, don't allow the hydro generator to be placed there.
            bool nextToHydroInWater = false;

            // Gets set to 'true' if next to a wind generator in the water.
            bool nextToWindInWater = false;

            // The program only checks up, down, left, and right, so these values can be hard coded.
            ActionTile tileLeft = null;
            ActionTile tileRight = null;
            ActionTile tileUp = null;
            ActionTile tileDown = null;

            // Setting left.
            if (stage.ValidMapPosition(tileRow - 1, tileCol))
                tileLeft = stage.tiles[tileRow - 1, tileCol];

            // Setting right.
            if (stage.ValidMapPosition(tileRow + 1, tileCol))
                tileRight = stage.tiles[tileRow + 1, tileCol];

            // Setting up.
            if (stage.ValidMapPosition(tileRow, tileCol + 1))
                tileUp = stage.tiles[tileRow, tileCol + 1];

            // Setting down.
            if (stage.ValidMapPosition(tileRow, tileCol - 1))
                tileDown = stage.tiles[tileRow, tileCol - 1];

            // Creates a list of tiles to be checked.
            List<ActionTile> checkTiles = new List<ActionTile>() { tileLeft, tileRight, tileUp, tileDown };

            // Goes through the check tiles.
            foreach(ActionTile checkTile in checkTiles)
            {
                // Checks if the tile exists.
                if(checkTile != null)
                {
                    // Checks if there's an action user, then checks if it's a water tile.
                    if(checkTile.HasActionUnitUser() && checkTile.IsWaterTile())
                    {
                        // Checks if it's a generator.
                        if(checkTile.actionUnitUser is ActionUnitGenerator)
                        {
                            // Convert to generator.
                            ActionUnitGenerator generator = (ActionUnitGenerator)checkTile.actionUnitUser;

                            // Checks the resource.
                            // If it's hydro or wind, change the appropriate variables.
                            switch(generator.resource)
                            {
                                case NaturalResources.naturalResource.hydro:
                                    nextToHydroInWater = true;
                                    break;

                                case NaturalResources.naturalResource.wind:
                                    nextToWindInWater = true;
                                    break;
                            }

                        }
                    }
                }

                // If next to a hydro generator or a wind generator, you can't place the unit here.
                if(nextToHydroInWater || nextToWindInWater)
                {
                    // Result is false, and no more need for checks.
                    result = false;
                    break;
                }
            }

            return result;
        }


        // Sets the flood timer to its max.
        public void SetFloodTimerToMax()
        {
            floodTimer = floodTimerMax;
        }

        // Sets the flood timer to its max and sets if the tiles have already been flooded or not.
        public void SetFloodTimerToMax(bool alreadyFloodedTiles)
        {
            SetFloodTimerToMax();
            floodedTiles = alreadyFloodedTiles;
        }

        // Flood the tiles.
        public void FloodTiles()
        {
            // The tile exists.
            if(tile != null)
            {
                // The map position of the tile this generator is on.
                Vector2Int mapPos = tile.mapPos;

                // The map positions to be flooded.
                Vector2Int mapPos1 = Vector2Int.zero;
                Vector2Int mapPos2 = Vector2Int.zero;
                Vector2Int mapPos3 = Vector2Int.zero;

                // Gets set to 'true' when the tile positions have been set.
                bool mapPosSet = false;

                // Checks if facing the four directions (diagonals are ignored).
                if(mapPos.x > 0) // Facing Right
                {
                    // Left-Up, Left, Left-Down
                    mapPos1 = new Vector2Int(mapPos.x - 1, mapPos.y + 1);
                    mapPos2 = new Vector2Int(mapPos.x - 1, mapPos.y);
                    mapPos3 = new Vector2Int(mapPos.x - 1, mapPos.y - 1);

                    mapPosSet = true;
                }
                else if(mapPos.x < 0) // Facing Left
                {
                    // Right-Up, Right, Right-Down
                    mapPos1 = new Vector2Int(mapPos.x + 1, mapPos.y + 1);
                    mapPos2 = new Vector2Int(mapPos.x + 1, mapPos.y);
                    mapPos3 = new Vector2Int(mapPos.x + 1, mapPos.y - 1);

                    mapPosSet = true;
                }
                else if(mapPos.y > 0) // Facing Up
                {
                    // Left-Down, Down, Right-Down
                    mapPos1 = new Vector2Int(mapPos.x - 1, mapPos.y - 1);
                    mapPos2 = new Vector2Int(mapPos.x, mapPos.y - 1);
                    mapPos3 = new Vector2Int(mapPos.x + 1, mapPos.y - 1);

                    mapPosSet = true;
                }
                else if(mapPos.y < 0) // Facing Down
                {
                    // Left-Up, Up, Right-Up
                    mapPos1 = new Vector2Int(mapPos.x - 1, mapPos.y + 1);
                    mapPos2 = new Vector2Int(mapPos.x, mapPos.y + 1);
                    mapPos3 = new Vector2Int(mapPos.x + 1, mapPos.y + 1);

                    mapPosSet = true;
                }

                // The three tiles.
                ActionTile tile1 = null;
                ActionTile tile2 = null;
                ActionTile tile3 = null;

                // Positions have been set.
                if(mapPosSet)
                {
                    // The action stage.
                    ActionStage actionStage = ActionManager.Instance.actionStage;

                    // Map position 1 is valid.
                    if(actionStage.ValidMapPosition(mapPos1.y, mapPos1.x))
                        tile1 = actionStage.tiles[mapPos1.y, mapPos1.x];

                    // Map position 2 is valid.
                    if (actionStage.ValidMapPosition(mapPos2.y, mapPos2.x))
                        tile2 = actionStage.tiles[mapPos2.y, mapPos2.x];

                    // Map position 3 is valid.
                    if (actionStage.ValidMapPosition(mapPos3.y, mapPos3.x))
                        tile3 = actionStage.tiles[mapPos3.y, mapPos3.x];

                }
                // No positions set, so somethign went wrong.
                else
                {
                    Debug.LogWarning("Facing direction wasn't set properly. No nearby tiles could be found.");
                }

                // Tile 1 exists.
                if(tile1 != null)
                {
                    tile1.SetTileOverlayType(ActionTile.actionTileOverlay.waterHazard);
                    tile1.KillActionUnitUser();
                }

                // Tile 2 exists.
                if(tile2 != null)
                {
                    tile2.SetTileOverlayType(ActionTile.actionTileOverlay.waterHazard);
                    tile2.KillActionUnitUser();
                }

                // Tile 3 exists.
                if(tile3 != null)
                {
                    tile3.SetTileOverlayType(ActionTile.actionTileOverlay.waterHazard);
                    tile3.KillActionUnitUser();
                }
            }
            else
            {
                Debug.LogWarning("No tile is set to this generator. Nearby tiles for flooding can't be found.");
            }

            // Tiles have been flooded.
            floodedTiles = true;

            // If the player is selecting an action prefab, that means the tiles are highlighted.
            // As such, refresh the highlighted tiles.
            if (ActionManager.Instance.playerUser.IsSelectingActionUnitPrefab())
                ActionManager.Instance.actionStage.RefreshHighlightedTiles();
        }

        // Update is called every from, if the MonoBehaviour is enabled
        protected override void Update()
        {
            base.Update();

            // If flooding is enabled.
            if(floodingEnabled)
            {
                // If the flood timer isn't finished.
                if(floodTimer > 0.0F)
                {
                    floodTimer -= Time.deltaTime;

                    // Flood timer has reached end.
                    if(floodTimer <= 0.0F)
                    {
                        floodTimer = 0;
                        FloodTiles();
                    }
                }
            }
        }
    }
}