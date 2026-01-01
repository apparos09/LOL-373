using System.Collections;
using System.Collections.Generic;
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
            return base.UsableTileConfiguration(tile);
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