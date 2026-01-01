using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace RM_EDU
{
    // Action Unit Generator - Wind
    public class ActionUnitGeneratorWind : ActionUnitGenerator
    {
        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // If the resource is unknown, set it to wind.
            if (resource == NaturalResources.naturalResource.unknown)
            {
                resource = NaturalResources.naturalResource.wind;
            }

            // Uses the wind to generate energy.
            if(!useWindToGenEnergy)
                useWindToGenEnergy = true;
        }

        // Checks if the tile configuration is valid.

        public override bool UsableTileConfiguration(ActionTile tile)
        {
            // The result to return.
            bool result;

            // If this is a water tile, check if it's close to land.
            // A wind generator can only be used in the water if it's one tile out from a land tile.
            if(tile.IsWaterTile())
            {
                // Gets the stage.
                ActionStage stage = ActionManager.Instance.actionStage;

                // The tile's row and column positions.
                int tileRow = tile.GetMapRowPosition();
                int tileCol = tile.GetMapColumnPosition();

                // Gets set to 'true' if land is found.
                bool foundLand = false;

                // Goes through every tile that's one space away from the current tile.
                for(int r = tileRow - 1; r < tileRow + 2; r++) // Row
                {
                    for(int c = tileCol - 1; c < tileCol + 2; c++) // Column
                    {
                        // If the provided row and column positions are valid.
                        if(stage.ValidMapPosition(r, c))
                        {
                            // The tile exists.
                            if(stage.tiles[r, c] != null)
                            {
                                // If this is a land tile, set 'foundLand' to true.
                                foundLand = stage.tiles[r, c].IsLandTile();
                            }
                        }

                        // Nearby land has been found, so break.
                        if (foundLand)
                            break;
                    }

                    // Nearby land has been found, so break.
                    if (foundLand)
                        break;
                }

                // If land was found, the spot is valid.
                result = foundLand;
            }
            // Not a water tile, so treat it as usable by default.
            else
            {
                result = true;
            }

            // Return the result.
            return result;
        }
    }
}