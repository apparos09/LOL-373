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

        // The action manager.
        public ActionManager actionManager;

        // The tile type.
        public actionTile tileType = actionTile.unknown;

        // The tile verison.
        public char tileVersion = 'A';

        // The collider.
        public new Collider2D collider;

        [Header("Sprites")]

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
        private static float highlightAlpha = 0.75F;

        [Header("World")]

        // The map position on the tile.
        // This is in map space, so its extents are (0, 0) to the size of the map.
        [Tooltip("The tile's position in the map's space.")]
        public Vector2Int mapPos = new Vector2Int(-1, -1);

        

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
            highlightSpriteRenderer.gameObject.SetActive(false);
            overlaySpriteRenderer.gameObject.SetActive(false);

        }

        // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
        protected virtual void OnMouseDown()
        {
            // Debug.Log("Clicked");
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
            return highlightSpriteRenderer.gameObject.activeSelf;
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

        // // Update is called once per frame
        // protected virtual void Update()
        // {
        // 
        // }
    }
}