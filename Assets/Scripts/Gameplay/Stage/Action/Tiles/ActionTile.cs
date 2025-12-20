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

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the action manager isn't set, make the instance.
            if (actionManager == null)
                actionManager = ActionManager.Instance;
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }
}