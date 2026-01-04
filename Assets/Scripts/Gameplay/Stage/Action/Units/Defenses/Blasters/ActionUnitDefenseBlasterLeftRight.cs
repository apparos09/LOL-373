using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A action unit blaster that shoots from the front (right) and the back (left).
    public class ActionUnitDefenseBlasterLeftRight : ActionUnitDefenseBlaster
    {
        [Header("Blaster/Left-Right")]

        // The projectile start pos in the parent class is for firing forward (right).
        // The starting position of the projectile if shooting towards the left side. 
        [Tooltip("Starting position used when firing a projectile to the left (back).")]
        public GameObject projectileStartPosLeft;

        // The offset of the projectile if it's being fired left.
        [Tooltip("Offset used when firing a projecitle to the left (back).")]
        public Vector3 projectileStartPosLeftOffset = Vector3.zero;

        // If 'true', the offset for the left side is flipped.
        [Tooltip("Flips the offset for the left starting position if true.")]
        public bool flipOffsetLeftX = false;

        // Has a target to the left (back) and a target to the right (front).
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

                // Calculates the starting position for shooting backwards.
                projectile.transform.position = CalculateProjectileStartingPositionBack();

                // If the target is to the left, have the projectile move left.
                // If the target is to the right, have the projectile move right (default).
                if (hasTargetLeft)
                    projectile.moveDirec = Vector2.left;
                else
                    projectile.moveDirec = Vector2.right;
            }
        }

        // Calculates and returns the projectile's starting position if firing from the back.
        public virtual Vector3 CalculateProjectileStartingPositionBack()
        {
            // The start position to be returned.
            Vector3 startPos;

            // The offset to be applied (left side).
            Vector3 offset = projectileStartPosLeftOffset;

            // If the offset should be flipped on the x-axis, flip it.
            if (flipOffsetLeftX)
                offset.x = -offset.x;

            // If the starting position object for the left exists, use that.
            if (projectileStartPosLeft != null)
            {
                startPos = projectileStartPosLeft.transform.position + offset;
            }
            // Use the base transform position plus the offset.
            else
            {
                startPos = transform.position + offset;
            }
                
            // Return the starting position.
            return startPos;
        }
    }
}
