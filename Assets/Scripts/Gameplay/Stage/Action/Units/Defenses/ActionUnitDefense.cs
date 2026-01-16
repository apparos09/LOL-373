using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The defense unit for the player.
    public class ActionUnitDefense : ActionUnitUser
    {
        [Header("Defense")]

        // The sprite renderer for a platform that can be displayed below the user unit.
        public SpriteRenderer platformSpriteRenderer;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Updates the platform visibility.
            UpdatePlatformVisible();
        }

        // OnTriggerEnter2D is called when the Collider2D other enters this trigger (2D physics only)
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
        }

        // OnTriggerStay2D is called once per frame for every Collider2D other that is touching this trigger (2D physics only)
        protected override void OnTriggerStay2D(Collider2D collision)
        {
            base.OnTriggerStay2D(collision);

            // If this unit can attack.
            if (CanAttack())
            {
                // Checks if colliding with an enemy unit.
                ActionUnitEnemy enemyUnit;

                // Tries to get the component.
                if (collision.TryGetComponent(out enemyUnit))
                {
                    // Perform an attack.
                    PerformAttack();
                }
            }
        }

        // Gets the unit type.
        public override unitType GetUnitType()
        {
            return unitType.defense;
        }

        // Returns 'true' if the platform is active.
        public bool IsPlatformVisible()
        {
            return platformSpriteRenderer.gameObject.activeSelf;
        }

        
        // Sets if the platform should be shown or not.
        public void SetPlatformVisible(bool value)
        {
            platformSpriteRenderer.gameObject.SetActive(value);
        }

        // Shows the platform sprite.
        public void ShowPlatform()
        {
            SetPlatformVisible(true);
        }

        // Hides the platform sprite.
        public void HidePlatform()
        {
            SetPlatformVisible(false);
        }

        // Enables the platform if the unit is on water. Disables it if the unit isn't on water.
        public void UpdatePlatformVisible()
        {
            // Checks if a tile exists.
            if(tile != null)
            {
                // If it's a land tile, don't use the platform.
                if(tile.IsLandTile())
                {
                    SetPlatformVisible(false);
                }
                // If it's not a land tile, use the platform.
                else
                {
                    SetPlatformVisible(true);
                }
            }
            else
            {
                // Tile doesn't exist, so the platform should be invisible by default.
                SetPlatformVisible(false);
            }
        }

        // Returns 'true' if the defense unit has a target to attack.
        // By default, this checks if there are any enemies in the same row as this defense unit.
        public virtual bool HasTarget()
        {
            // Result to be returned.
            bool hasTarget = false;

            // On a tile.
            if (tile != null)
            {
                // TODO: enable limit on range.

                // Gets the tile's row.
                int row = tile.GetMapRowPosition();

                // If the row is valid.
                // if(actionManager.actionStage.ValidMapPosition)
                if(actionManager.actionStage.ValidMapRow(row))
                {
                    // The defense has a target.
                    hasTarget = actionManager.actionStage.IsEnemyInRowRightOfPosition(row, transform.position, true, false);
                }

            }

            return hasTarget;
        }

        // Performs an attack.
        public virtual void PerformAttack()
        {
            OnUnitAttackPerformed();
        }

        // Kills the unit.
        public override void Kill()
        {
            base.Kill();
        }

        // Called when a unit has died/been destroyed.
        public override void OnUnitDeath()
        {
            base.OnUnitDeath();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the action unit can attack.
            if(CanAttack())
            {
                // If the defense has a target.
                if(HasTarget())
                {
                    // Performs an attack.
                    PerformAttack();
                }
            }
        }
    }
}