using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A tile in the action stage.
    public class ActionTile : MonoBehaviour
    {
        // The action tile.
        public enum actionTile { unknown, land, river, sea, metal };

        // The tile type count.
        public const int ACTION_TILE_TYPE_COUNT = 5;

        // The action tile overlay.
        public enum actionTileOverlay { none, unusable, geothermalSource, coalSource, naturalGasSource, nuclearSource, oilSource, waterHazard, nuclearHazard, oilHazard }

        // The total number of values in the tile overlay enum.
        public const int ACTION_TILE_OVERLAY_TYPE_COUNT = 10;

        // The action manager.
        public ActionManager actionManager;

        // The tile type.
        public actionTile tileType = actionTile.unknown;

        // The tile overlay.
        // This is public because the prefabs need to be able to set this.
        [Tooltip("The tile overlay type. Use the dedication function for changing this member so that the visuals update as well.")]
        public actionTileOverlay tileOverlayType = actionTileOverlay.none;

        // The default tile overlay type.
        [Tooltip("The default tile overlay type. This is what the tile is set to when reset.")]
        public actionTileOverlay defaultTileOverlayType = actionTileOverlay.none;

        // The tile verison.
        public char tileVersion = 'A';

        // The collider.
        public new Collider2D collider;

        // If 'true', the tile can be interacted with for game functions.
        [Tooltip("If true, the tile is interactable by the user.")]
        public bool interactable = true;

        [Header("Visuals")]

        // The animator.
        public Animator animator;

        // The highlight sprite renderer.
        [Tooltip("The sprite used to highlight the tile to show if it's usable or not.")]
        public SpriteRenderer highlightSpriteRenderer;

        // The overlay sprite renderer.
        [Tooltip("The sprite used to put things on top of the base sprite.")]
        public SpriteRenderer overlaySpriteRenderer;

        // The sprite renderer.
        [Tooltip("The base sprite iamge.")]
        public SpriteRenderer baseSpriteRenderer;

        // The tile variant.
        public List<Sprite> tileVersionSprites = new List<Sprite>();

        // The normal and darkened colours are used to create a checkerboard for the game.
        // The normal tile color.
        private static Color normalColor = Color.white;

        // The darkened tile color.
        private static Color darkenedColor = new Color(0.75F, 0.75F, 0.75F);

        // The colour used to show that the tile is usable.
        private static Color usableColor = Color.yellow;

        // The colour used to show that the tile is unusable.
        private static Color unusableColor = Color.grey;

        // The alpha of highlights on the tile.
        private static float highlightAlpha = 0.50F;

        [Header("World")]

        // The map position on the tile.
        // This is in map space, so its extents are (0, 0) to the size of the map.
        [Tooltip("The tile's position in the map's space.")]
        public Vector2Int mapPos = new Vector2Int(-1, -1);

        // The action unit user on the tile.
        [Tooltip("If action unit user on the tile. If null, then there's no user unit on the tile.")]
        public ActionUnitUser actionUnitUser = null;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the action manager isn't set, make the instance.
            if (actionManager == null)
                actionManager = ActionManager.Instance;

            // Gets the collider.
            if (collider == null)
                collider = GetComponent<Collider2D>();

            // Turn off highlight and overlay.
            SetHighlighted(false, true);
            SetTileOverlayType(defaultTileOverlayType);
        }

        // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
        protected virtual void OnMouseDown()
        {
            // Tries to perform a player user action.
            TryPerformPlayerUserAction();
        }

        // Gets the tile type.
        public actionTile GetTileType()
        {
            return tileType;
        }

        // Gets the tile version index.
        public int GetTileVersionIndex()
        {
            // The index.
            int index;

            // If there is a sprite, see if it's in the tile version list.
            if (baseSpriteRenderer.sprite != null)
            {
                // If the tile is in the list, get the index.
                if (tileVersionSprites.Contains(baseSpriteRenderer.sprite))
                {
                    index = tileVersionSprites.IndexOf(baseSpriteRenderer.sprite);
                }
                else
                {
                    index = -1;
                }
            }
            else
            {
                index = -1;
            }

            return index;
        }

        // Returns the tile overlay type.
        public actionTileOverlay GetTileOverlayType()
        {
            return tileOverlayType;
        }

        // Sets the tile overlay type.
        public void SetTileOverlayType(actionTileOverlay overlayType, bool setAsDefault = false)
        {
            // Sets the new type.
            tileOverlayType = overlayType;

            // If the overlay type should be set as the default, set it.
            if(setAsDefault)
                defaultTileOverlayType = overlayType;

            // Turns on the overlay sprite.
            overlaySpriteRenderer.gameObject.SetActive(true);

            // Gets the tile prefabs.
            ActionTilePrefabs tilePrefabs = ActionTilePrefabs.Instance;

            // Gets the new sprite.
            Sprite newSprite = tilePrefabs.GetActionTileOverlaySprite((int)tileOverlayType);

            // Sets the overlay to use the new sprite.
            overlaySpriteRenderer.sprite = newSprite;

            // If no overlay exists, turn off the overlay sprite renderer.
            if(tileOverlayType == actionTileOverlay.none)
            {
                overlaySpriteRenderer.gameObject.SetActive(false);
            }

            // If the tile is currently highlighted as usable...
            // And was changed to something that makes the tile unusable...
            // Change the highlight.
            if (IsHighlightedUsable())
            {
                // Checks the type.
                switch(tileOverlayType)
                {
                    case actionTileOverlay.waterHazard: // Water

                        // If the player user is selecting a prefab.
                        if(ActionManager.Instance.playerUser.IsSelectingActionUnitPrefab())
                        {
                            // Gets the type of the prefab the player user is selecting.
                            switch (ActionManager.Instance.playerUser.GetSelectedActionUnitPrefabType())
                            {
                                // If a generator is being selected, highlight the tile as not usuable...
                                // Since generators cannot go on this tile.
                                case ActionUnit.unitType.generator:
                                    HighlightTile(false);
                                    break;
                            }
                        }

                        break;

                    // If now set to a hazard, unhighlight the tile, since it shouldn't be usable anymore.
                    case actionTileOverlay.nuclearHazard:
                    case actionTileOverlay.oilHazard:
                        HighlightTile(false);
                        break;
                }
            }
        }

        // Returns the default tile overlay type.
        public actionTileOverlay GetDefaultTileOverlayType()
        {
            return defaultTileOverlayType;
        }

        // Sets the default tile overlay type.
        // If 'setCurrent' is true, the current default type is set as well.
        public void SetDefaultTileOverlayType(actionTileOverlay newDefaultType, bool setAsCurrent = false)
        {
            // Checks if the current overlay type should also be set.
            if (setAsCurrent)
                SetTileOverlayType(newDefaultType, true);
            else
                defaultTileOverlayType = newDefaultType;
        }

        // Resets the tile ovelray type.
        public void ResetTileOverlayType()
        {
            SetTileOverlayType(defaultTileOverlayType, true);
        }

        // COLOR CHANGES

        // Gets the tile's base color.
        public Color GetTileBaseColor()
        {
            return baseSpriteRenderer.color;
        }

        // Returns 'true' if the base tile is its normal color.
        // This checks the base sprite renderer.
        public bool IsNormalBaseColor()
        {
            return baseSpriteRenderer.color == normalColor;
        }

        // Returns 'true' if the base tile is its darkened color.
        // This checks the base sprite renderer.
        public bool IsDarkenedBaseColor()
        {
            return baseSpriteRenderer.color == darkenedColor;
        }

        // Returns the normal color.
        public static Color NormalColor
        {
            get { return normalColor; }
        }

        // Returns the darkened color.
        public static Color DarkenedColor
        {
            get { return darkenedColor; }
        }

        // HIGHLIGHT

        // The color used to show that a tile is usable.
        public static Color UsableColor
        {
            get { return usableColor; }
        }

        // The color used to show that a tile is unusable.
        public static Color UnusableColor
        {
            get { return unusableColor; }
        }

        // Returns the highlight alpha.
        public static float HighlightAlpha
        {
            get { return highlightAlpha; }
        }

        // Returns 'true' if the tile is currently highlighted.
        public bool IsHighlighted()
        {
            return highlightSpriteRenderer.enabled && highlightSpriteRenderer.gameObject.activeSelf;
        }

        // Returns true if the tile is highlighted as usable.
        public bool IsHighlightedUsable()
        {
            // Check if highlighted...
            if (IsHighlighted())
            {
                // Then check for usable color.
                Color tempColor = highlightSpriteRenderer.color;
                tempColor.a = highlightAlpha;

                // If the colors match, this is the usable color.
                return tempColor == UsableColor;
            }
            else
            {
                return false;
            }
        }

        // Returns ture if the tile is highlighted as unusuable.
        public bool IsHighlightedUnusable()
        {
            // Check if highlighted...
            if (IsHighlighted())
            {
                // Then check for usable color.
                Color tempColor = highlightSpriteRenderer.color;
                tempColor.a = highlightAlpha;

                // If the colors match, this is the usable color.
                return tempColor == UnusableColor;
            }
            else
            {
                return false;
            }
        }

        // Sets if the tile should be highlighted.
        // The 'usuable' argument determines what the highlight color will be.
        // If there will be no highlight, the highlight color is white.
        public void SetHighlighted(bool highlighted, bool usable)
        {
            // The new color.
            Color newColor;

            // Checks if the highlight will be turned on or off.
            if (highlighted)
            {
                newColor = usable ? UsableColor : UnusableColor;
            }
            else
            {
                // The tile won't be highlighted, so just make it white.
                newColor = Color.white;
            }

            // Change the alpha value.
            newColor.a = highlightAlpha;

            // Sets the new color.
            highlightSpriteRenderer.color = newColor;

            // Changes the activity of the sprite renderer based on if it's highlighted or not.
            highlightSpriteRenderer.gameObject.SetActive(highlighted);
        }

        // Highlights the tile.
        public void HighlightTile(bool usable)
        {
            SetHighlighted(true, usable);
        }

        // Highlights the tile.
        // If the unit can use this tile, it's highlighted as a usable tile.
        // Highlighted as not usable if the unit cannot use it.
        public void HighlightTile(ActionUnit compUnit)
        {
            SetHighlighted(true, compUnit.UsableTile(this));
        }

        // Unhighlights the tile.
        // Since the highlight is being turned off, usable can be set to anything.
        public void UnhighlightTile(bool usable)
        {
            SetHighlighted(false, usable);
        }

        // Unhighlights the tile. Automatically sets usable based on if there's an action unit on this tile.
        public void UnhighlightTile()
        {
            SetHighlighted(false, !HasActionUnitUser());
        }

        // Refreshes the highlighted tile.
        public void RefreshHighlightedTile()
        {
            SetHighlighted(IsHighlighted(), !HasActionUnitUser());
        }

        // WORLD
        // Gets the map position.
        public Vector2Int GetMapPosition()
        {
            return mapPos;
        }

        // Gets the tile's map row position, which corresponds to the tile's map y-position (mapPos.y).
        public int GetMapRowPosition()
        {
            return mapPos.y;
        }

        // Gets the tile's map colum position, which corresponds to the tile's map x-position (mapPos.x).
        public int GetMapColumnPosition()
        {
            return mapPos.x;
        }

        // ACTION UNUT USER
        // Called when the player unit user is selecting a unit.
        public void OnPlayerUserSelectedUnit(ActionUnit selectedUnit)
        {
            HighlightTile(selectedUnit.UsableTile(this));
        }

        // Called when the player user has deselected a unit.
        public void OnPlayerUserDeselectedUnit()
        {
            UnhighlightTile(HasActionUnitUser());
        }

        // Returns 'true' if an action unit can be placed on this tile.
        // If 'ignoreHasActionUser' is true, this ignores if there's a user on the tile.
        public bool IsUsableByActionUnitUser(ActionUnitUser compUnit, bool ignoreHasActionUser)
        {
            bool result = false;

            // If there are no valid tiles, or the valid tiles list contains this tile's type...
            // This aciton user can use this tile.
            if(compUnit.UsableTileType(this) && compUnit.UsableTileOverlayType(this))
            {
                // Checks if the saved user should be ignored.
                // If the current user should be ignored, this returns true.
                // If the current user shouldn't be ignored, return false if this tile is being used.
                result = ignoreHasActionUser ? true : !HasActionUnitUser();
            }

            // Return result.
            return result;
        }

        // Returns 'true' if an action unit is on the tile.
        public bool HasActionUnitUser()
        {
            return actionUnitUser;
        }

        // Returns 'true' if this action unit is on this tile.
        // Returns 'false' if this unit isn't on this tile or if the tile has no unit.
        public bool HasActionUnitUser(ActionUnitUser compUnit)
        {
            return actionUnitUser == compUnit;
        }

        // Sets the action unit.
        public void SetActionUnitUser(ActionUnitUser newUnitUser)
        {
            actionUnitUser = newUnitUser;

            // If the unit has been set.
            if(actionUnitUser != null)
            {
                actionUnitUser.tile = this;
            }

            // If the tile is currently highlighted, change the highlight.
            if(IsHighlighted())
                RefreshHighlightedTile();
        }

        // Clears the action unit on the tile.
        public void ClearActionUnitUser()
        {
            // If there is an action unit user.
            if(actionUnitUser != null)
            {
                // If the saved tile is this tile, set it to null.
                if (actionUnitUser.tile == this)
                {
                    actionUnitUser.tile = null;
                }
            }

            // Set unit to null.
            actionUnitUser = null;

            // If the tile is currently highlighted, change the highlight.
            if (IsHighlighted())
                RefreshHighlightedTile();
        }

        // Kills the action unit user.
        public void KillActionUnitUser()
        {
            // If there's an action user unit on this tile, kill it.
            if(actionUnitUser != null)
            {
                actionUnitUser.Kill();
            }

            actionUnitUser = null;
        }

        // Tries to perform a player user action when clicked on.
        // Returns true if an action was performed.
        public bool TryPerformPlayerUserAction()
        {
            // Gets set to 'true' if an action was performed.
            bool performed = false;

            // Gets the player user from the aciton manager.
            ActionPlayerUser playerUser = ActionManager.Instance.playerUser;

            // NOTE: since an action unit being on a tile would be blocking the tile's collider...
            // The remove call will likely never be triggered unless the player hits an edge of the tile...
            // That is not covered by the action unit's hit box.

            // Checks what mode the player user is in.
            if (playerUser.InSelectMode()) // Select
            {
                // If there is no unit on the tile, see if the player wants to place a unit.
                if (!HasActionUnitUser())
                {
                    // If the player user has a unit prefab and it can use this tile...
                    // Instantiate the unit and put it on this tile.
                    // This function also checks if the player user is selecting a tile.
                    if (playerUser.CanSelectedActionUnitUseTile(this))
                    {                        
                        playerUser.InstantiateSelectedActionUnit(this);
                        performed = true;
                    }
                }
            }
            else if (playerUser.InRemoveMode()) // Remove
            {
                // Checks if there's a unit on this tile to remove.
                if(HasActionUnitUser())
                {
                    // If the action unit user can be removed by the user.
                    if(actionUnitUser.IsRemovableByUser())
                    {
                        KillActionUnitUser(); // Kill the user on this platform.
                        performed = true;
                    }
                }
            }

            // Returns 'true' if an action was performed.
            return performed;
        }

        // Resets the tile.
        public void ResetTile()
        {
            // Clears the action user unit. 
            ClearActionUnitUser();

            // Resets the tile overlay type.
            ResetTileOverlayType();
        }

        // // Update is called once per frame
        // protected virtual void Update()
        // {
        // 
        // }
    }
}