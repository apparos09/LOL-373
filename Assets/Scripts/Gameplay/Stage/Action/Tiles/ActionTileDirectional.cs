using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A tile that is facing a specific direction. By default, all player user units face right.
    public class ActionTileDirectional : ActionTile
    {
        [Header("Directional")]

        // The direction the tile is facing.
        public Vector2 direction = Vector2.right;

        // If 'true', set the direction by the tile version in start.
        public bool setDirecByTileVersionInStart = true;

        // TODO: replace with animations
        // The sprite for the sprite facing left.
        public Sprite direcLeftSprite;

        // The sprite for the tile facing right.
        public Sprite direcRightSprite;

        // The sprite for the tile facing up.
        public Sprite direcUpSprite;

        // The sprite for the tile facing down.
        public Sprite direcDownSprite;

        // If 'true', the directional sprite is set in the Start() function.
        [Tooltip("Sets the directional sprite in the start function.")]
        public bool setDirecSpriteInStart = true;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the direction shall be set by the tile version.
            if(setDirecByTileVersionInStart)
            {
                // Since this function already updates the sprite...
                // It checks 'setDirecSpriteInStart' to see if it should be changed.
                SetDirectionByTileVersion(setDirecSpriteInStart);
            }
            else
            {
                // If the directional sprite should be set.
                if (setDirecSpriteInStart)
                    SetSpriteByTileDirection();
            }

        }

        // Sets the direction by the tile version.
        // Right is the default.
        public void SetDirectionByTileVersion(bool updateSprite = true)
        {
            // The new direction.
            Vector2 newDirec;

            // Checks the tile version to know the direction.
            switch(tileVersion)
            {
                case 'A':
                case 'a': // Right
                default:
                    newDirec = Vector2.right;
                    break;

                case 'B':
                case 'b': // Up
                    newDirec = Vector2.up;
                    break;

                case 'C':
                case 'c': // Left
                    newDirec = Vector2.left;
                    break;

                case 'D':
                case 'd': // Down
                    newDirec = Vector2.down;
                    break;
            }

            // Sets the new direction.
            direction = newDirec;

            // Updates the sprite.
            if (updateSprite)
                SetSpriteByTileDirection();
        }

        // Sets the sprite by the tile direction.
        // If the direction is neutral (0, 0), no change occurs.
        public void SetSpriteByTileDirection()
        {
            // Checks the direction to see what sprite to set.
            // TODO: change to use animator.
            if (direction.x < 0.0F) // Left
            {
                baseSpriteRenderer.sprite = direcLeftSprite;
            }
            else if (direction.x > 0.0F) // Right
            {
                baseSpriteRenderer.sprite = direcRightSprite;
            }
            else if (direction.y > 0.0F) // Up
            {
                baseSpriteRenderer.sprite = direcUpSprite;
            }
            else if(direction.y < 0.0F) // Down
            {
                baseSpriteRenderer.sprite = direcDownSprite;
            }
        }

        // Sets the sprite by the provided tile direction. This also overrides the saved direction.
        public void SetSpriteByTileDirection(Vector2 newDirection)
        {
            direction = newDirection;
            SetSpriteByTileDirection();
        }
    }
}