using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The player user unit.
    public abstract class ActionUnitUser : ActionUnit
    {
        [Header("User")]
        // The tiles that are valid to place this unit on.
        // The metal tile is only for the lane blaster, so it's not included by default.
        public List<ActionTile.actionTile> validTiles = new List<ActionTile.actionTile>();

        // The tile the user unit is placed on.
        public ActionTile tile = null;

        // The position offset of the unit when placed on a tile.
        public Vector3 tilePosOffset = Vector3.zero;

        // Returns 'true' if the unit can be removed by the user player once it's placed.
        public bool removableByUser = true;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
        protected virtual void OnMouseDown()
        {
            // Tries to perform a player user action.
            TryPerformPlayerUserAction();
        }

        // Returns 'true' if the provided tile can be used by this action unit.
        public override bool UsableTile(ActionTile tile)
        {
            // The result to be returned. 
            bool result;

            // If the tile exists, check if it's usable.
            if(tile != null)
            {
                // Checks if the tile is interactable, if the tile is usable by an action unit,...
                // And if the tile already has an action unit.
                // If the tile cannot be used for any reason, return false.
                if(!tile.interactable || !tile.IsUsableByActionUnitUser() || tile.HasActionUnitUser())
                {
                    // Tile already being used.
                    result = false;
                }
                // No unit on tile, so check if usable.
                else
                {
                    // Sets result based on if the tile is usable by its type.
                    result = UsableTileType(tile);
                }
            }
            // The tile doesn't exist, so nothing can be placed there.
            else
            {
                result = false;
            }
            
            return result;
        }

        // Returns 'true' if the tile is usable based purely on its type.
        // If there is some other factor that makes the tile usable or unusable, it isn't considered.
        public bool UsableTileType(ActionTile tile)
        {
            bool result;

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

            return result;
        }

        // Sets the position to the provided tile's position.
        public void SetPositionToTilePosition(ActionTile refTile)
        {
            transform.position = refTile.transform.position + tilePosOffset;
        }

        // Sets the position to the provided tile's position.
        public void SetPositionToTilePosition()
        {
            SetPositionToTilePosition(tile);
        }

        // Clears the tile this unit is on.
        public void ClearTile()
        {
            // The tile exists.
            if (tile != null)
            {
                // The tile has this action unit, so clear the tile.
                if (tile.actionUnitUser == this)
                {
                    tile.ClearActionUnitUser();
                }
            }

            // Clear tile.
            tile = null;
        }

        // Returns 'true' if the unit can be removed by the user.
        public bool IsRemovableByUser()
        {
            return removableByUser;
        }

        // Kills the unit.
        public override void Kill()
        {
            base.Kill();
        }

        // Called when a unit has died/been destroyed.
        public override void OnUnitDeath()
        {
            // Tries to remove the unit from its user list.
            actionManager.playerUser.TryRemoveCreatedUnitFromList(this);

            // Clears the tile this unit is on.
            ClearTile();

            base.OnUnitDeath();
        }

        // Tries to perform a player user action when clicked on.
        // Returns true if an action was performed.
        public bool TryPerformPlayerUserAction()
        {
            // Gets set to 'true' if an action was performed.
            bool performed = false;

            // Gets the player user from the aciton manager.
            ActionPlayerUser playerUser = ActionManager.Instance.playerUser;

            // If the player is in remove mode, kill the unit.
            if (playerUser.InRemoveMode())
            {
                // If this unit can be removed by the player user.
                if(IsRemovableByUser())
                {
                    Kill();
                    performed = true;
                }
            }

            // Returns 'true' if an action was performed.
            return performed;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Tries to remove the unit from its user list.
            actionManager.playerUser.TryRemoveCreatedUnitFromList(this);

            // Clears the tile.
            ClearTile();
        }
    }
}