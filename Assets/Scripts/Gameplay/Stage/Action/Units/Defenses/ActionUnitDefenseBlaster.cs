using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action unit defense blaster.
    public class ActionUnitDefenseBlaster : ActionUnitDefense
    {
        [Header("Defense/Blaster")]

        // The projectile prefab.
        public ActionProjectile projectilePrefab;

        // A game object used to position the projectile when it's first shot.
        // If this is null, the shooter's object is used.
        [Tooltip("The starting position of the projectile as an object. If null, the shooter's position is the starting position.")]
        public GameObject projectileStartPos;

        // The offset of the starting position of a fired projectile.
        // This is applied to projectileStartPos's position if that object isn't null.
        [Tooltip("The offset of the starting position of a fired projectile.")]
        public Vector3 projectileStartPosOffset = Vector3.zero;

        // The projectiles fired by this unit.
        public List<ActionProjectile> firedProjectiles = new List<ActionProjectile>();

        // Performs an attack.
        public override void PerformAttack()
        {
            Shoot();
            base.PerformAttack();
        }

        // Shoots a projectile. Returns the projectile if it's been instantiated correctly.
        public virtual ActionProjectile Shoot()
        {
            // The projectile to be returned.
            ActionProjectile projectile = null;

            // Projectile prefab exists.
            if (projectilePrefab != null)
            {
                projectile = Instantiate(projectilePrefab);

                // Sets the projectile's parent.
                if (owner != null)
                {
                    // If the owner's projectile parent is set, use that.
                    // If the owner has no projectile parent, use the owner's transform.
                    // This doesn't use the blaster's parent in case the blaster gets destroyed.
                    projectile.transform.parent = owner.unitProjectileParent != null ? owner.unitProjectileParent.transform : owner.transform;
                }

                // The starting position.
                Vector3 startPos;

                // If the projecile start position exists, use that plus the offset.
                if (projectileStartPos != null)
                {
                    startPos = projectileStartPos.transform.transform.position + projectileStartPosOffset;
                }
                // No projectile start position object, so use shooter's transform plus offset.
                else
                {
                    startPos = transform.position + projectileStartPosOffset;
                }

                // Sets the starting position.
                projectile.transform.position = startPos;

                // Sets the shooter and move direction.
                projectile.shooterUnit = this;
                projectile.moveDirec = Vector2.right;

                // Updates the shooter values in the projectile.
                projectile.UpdateShooterAttackValues();

                // Add to list of fired projectiles.
                firedProjectiles.Add(projectile);
            }

            return projectile;
        }

        // Called when a projectile has been killed.
        public void OnProjectileKilled(ActionProjectile projectile)
        {
            // If the fired projectile list includes this projectile, remove it.
            if(firedProjectiles.Contains(projectile))
            {
                firedProjectiles.Remove(projectile);
            }
        }

    }
}