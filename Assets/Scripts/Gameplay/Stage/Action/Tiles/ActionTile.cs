using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A tile in the action stage.
    public class ActionTile : MonoBehaviour
    {
        // The action tile.
        public enum actionTile { unknown, land, river, sea };

        // The action manager.
        public ActionManager actionManager;

        // The tile type.
        public actionTile tileType = actionTile.unknown;

        // The sprite renderer.
        public SpriteRenderer spriteRenderer;

        // The animator.
        public Animator animator;

        // The tile variant.
        public List<Sprite> tileVersions = new List<Sprite>();

        // The tile verison.
        public char tileVersion = 'A';

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
            if (spriteRenderer.sprite != null)
            {
                // If the tile is in the list, get the index.
                if (tileVersions.Contains(spriteRenderer.sprite))
                {
                    index = tileVersions.IndexOf(spriteRenderer.sprite);
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