using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A tile that is facing a specific direction. By default, all player user units face right.
    public class ActionTileDirectional : ActionTile
    {
        [Tooltip("Directional")]
        // The direction the tile is facing.
        public Vector2 direction = Vector2.right;
    }
}