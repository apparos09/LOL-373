using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The player user unit.
    public abstract class ActionUnitUser : ActionUnit
    {
        // The tiles that are valid to place this unit on.
        // The metal tile is only for the lane blaster, so it's not included by default.
        public List<ActionTile.actionTile> validTiles = new List<ActionTile.actionTile>();

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Returns 'true' if the provided tile can be used by this action unit.
        public virtual bool UsableTile(ActionTile tile)
        {
            // The result to be returned. 
            bool result;

            // If the tile exists, check if it's usable.
            if(tile != null)
            {
                // Checks if there are specific tiles that are valid.
                if (validTiles.Count > 0)
                {
                    // If the valid list contains the tile type, the unit can be placed there.
                    result = validTiles.Contains(tile.GetTileType());
                }
                // Since there are no valid tiles, the game acts as if there are no invalid tiles.
                else
                {
                    result = true;
                }
            }
            // The tile doesn't exist, so nothing can be placed there.
            else
            {
                result = false;
            }

                return result;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}