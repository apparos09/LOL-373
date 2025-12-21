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

        // The map position on the tile.
        // This is in map space, so its extents are (0, 0) to the size of the map.
        [Tooltip("The tile's position in the map's space.")]
        public Vector2Int mapPos = new Vector2Int(-1, -1);

        // Gets the tile type.
        public actionTile GetTileType()
        {
            return tileType;
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the action manager isn't set, make the instance.
            if (actionManager == null)
                actionManager = ActionManager.Instance;
        }

        // // Update is called once per frame
        // protected virtual void Update()
        // {
        // 
        // }
    }
}