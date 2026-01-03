using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A action unit blaster that shoots from the front and the back.
    public class ActionUnitDefenseBlasterFrontBack : ActionUnitDefenseBlaster
    {
        // Has a target to the left and a target to the right.
        private bool hasTargetLeft = false;
        private bool hasTargetRight = false;

        // Returns 'true' if the defense unit has a target to attack.
        // SInce this is a front-back blaster, it checks the left and right side.
        public override bool HasTarget()
        {
            // Result to be returned.
            bool hasTarget = false;

            // Sets both to false. Will set to true if targets are found.
            hasTargetLeft = false;
            hasTargetRight = false;

            // On a tile.
            if (tile != null)
            {
                // Gets the tile's row.
                int row = tile.GetMapRowPosition();

                // If the row is valid.
                if (row >= 0 && row < actionManager.actionStage.MapRowCount)
                {
                    // Checks for targets to the left and right.
                    hasTargetLeft = actionManager.actionStage.IsEnemyInRowLeftOfPosition(row, transform.position, true, false);
                    hasTargetRight = actionManager.actionStage.IsEnemyInRowRightOfPosition(row, transform.position, true, false);
                    
                    // There's a target if there is one to the left or right.
                    hasTarget = hasTargetLeft || hasTargetRight;
                }
            }

            return hasTarget;
        }

        // Performs an attack.
        public override void PerformAttack()
        {
            // If there are targets are both sides, so shooting twice.
            bool shootTwice = hasTargetLeft && hasTargetRight;

            // Performs an attack.
            base.PerformAttack();

            // If shooting twice.
            if(shootTwice)
            {
                // Gets set to 'true' if the owner has energy for a second shot.
                bool hasEnergyForShot2 = true;

                // Checks owner's energy.
                if (owner != null)
                    hasEnergyForShot2 = owner.HasEnergyForAttack(this);

                // If the owner has energy for shot 2.
                if (hasEnergyForShot2)
                {
                    // Projectiles go right by default, so the existing projectile stays going right.
                    ActionProjectile rightProj = firedProjectiles[firedProjectiles.Count - 1];

                    // Gets the left projectile. This function also puts iti n the list.
                    ActionProjectile leftProj = Shoot();

                    // Projectile directions.
                    rightProj.moveDirec = Vector2.right;
                    leftProj.moveDirec = Vector2.left;

                    // The owner exists.
                    if(owner != null)
                    {
                        // Reduce the owner's energy by the attack cost.
                        owner.DecreaseEnergy(attackEnergyCost);
                    }
                }
            }
            else
            {
                // Only fired one shot.

                // Gets the most recently fired projectile.
                ActionProjectile projectile = firedProjectiles[firedProjectiles.Count - 1];

                // If the target is to the left, have the projectile move left.
                // If the target is to the right, have the projectile move right.
                if (hasTargetLeft)
                    projectile.moveDirec = Vector2.left;
                else
                    projectile.moveDirec = Vector2.right;
            }
        }
    }
}
