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

        // The parent of the projectile.
        public GameObject projectileParent;

        // The offset of the starting position of a fired projectile.
        [Tooltip("The offset of the starting position of a fired projectile. This offset is based on the shooter's position.")]
        public Vector3 projectileStartPosOffset = Vector3.zero;

        // The projectiles fired by this unit.
        public List<ActionProjectile> firedProjectiles = new List<ActionProjectile>();

        // Performs an attack.
        public override void PerformAttack()
        {
            Shoot();
            base.PerformAttack();
        }

        // Shoots a projectile.
        public virtual void Shoot()
        {
            // Projectile prefab exists.
            if(projectilePrefab != null)
            {
                ActionProjectile projectile = Instantiate(projectilePrefab);

                // Gives the projectile the positon of the shooter plus an offset.
                projectile.transform.position = transform.position + projectileStartPosOffset;

                // Sets the projectile parent.
                projectile.transform.parent = (projectileParent != null) ? projectile.transform : transform;

                // Sets the shooter and move direction.
                projectile.shooterUnit = this;
                projectile.moveDirec = Vector2.right;

                // Add to list of fired projectiles.
                firedProjectiles.Add(projectile);
            }
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