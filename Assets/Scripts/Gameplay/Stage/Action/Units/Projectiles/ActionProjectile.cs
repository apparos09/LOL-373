using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A projectile fired by a unit in the action scene.
    public class ActionProjectile : MonoBehaviour
    {
        // The action manager.
        public ActionManager actionManager;

        // The action unit that fired the projectile.
        // If null, then assume no one fired this unit.
        public ActionUnit shooterUnit;

        // The sprite renderer of the action projectile.
        public SpriteRenderer spriteRenderer;

        // The collider.
        public new Collider2D collider;

        // The rigid body.
        public new Rigidbody2D rigidbody;

        // The default attack power of the projectile.
        public float defaultAttackPower = 1.0F;

        // If 'true', the attack power of the shooter is used instead of the projectile's attack power...
        // Upon collision with a valid target occurring.
        public bool useAttackPowerOfShooter = true;

        // The default 'one hit kill' check. If true, the projectile kills its target in one hit.
        [Tooltip("If true, by default the projectile kills the target in one hit.")]
        public bool defaultOneHitKill = false;

        // The movement direction.
        public Vector2 moveDirec = Vector2.right;

        // The movement speed. The projectile moves at a fixed speed.
        [Tooltip("The movement speed of a projectile. Projectiles move at a fixed speed.")]
        public float moveSpeed = 1.0F;

        // Valid targets for the projectile.
        // If the list is empty, all units are valid targets.
        public List<ActionUnit.unitType> validTargets = new List<ActionUnit.unitType>();

        // If the projectile should die on contact.
        [Tooltip("If true, the projectile will die on contact with a valid target.")]
        public bool dieOnContact = true;

        // Start is called before the first frame update
        void Start()
        {
            // If the action manager isn't set, set it.
            if (actionManager == null)
                actionManager = ActionManager.Instance;

            // Gets the collider if not set.
            if (collider == null)
                collider = GetComponent<Collider2D>();

            // Gets the rigidbody if not set.
            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody2D>();

            // If the shooter unit exists, set to ignore the collision.
            if (shooterUnit != null)
                Physics2D.IgnoreCollision(collider, shooterUnit.collider);
        }

        // OnTriggerEnter2D is called when the Collider2D other enters this trigger (2D physics only)
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            // The unit the collider hit.
            ActionUnit colUnit;

            // Tries to get the component.
            if(collision.TryGetComponent(out colUnit))
            {
                OnContactWithTarget(colUnit);
            }
        }

        // Called to ignore the collision of this projectile with it's shooter.
        public void IgnoreCollisionWithShooter(bool ignore)
        {
            Physics2D.IgnoreCollision(collider, shooterUnit.collider, ignore);
        }

        // Called wehn the projectile has made contact with a target.
        public virtual void OnContactWithTarget(ActionUnit target)
        {
            // If the valid targets list is empty, or if there are no valid targets...
            // Attack the unit.
            if (validTargets.Count <= 0 || validTargets.Contains(target.GetUnitType()))
            {
                // If the attack power of the shooter should be used...
                // And there is a shooter unit.
                if (useAttackPowerOfShooter && shooterUnit != null)
                {
                    // Use the attack function and send the shooter as the attacker.
                    target.AttackUnit(shooterUnit);
                }
                else
                {
                    // Power for attack.
                    float damage = defaultAttackPower;

                    // If this is a one-hit kill projectile, set damage to target's health.
                    if (defaultOneHitKill)
                        damage = target.health;

                    // Apply damage using the projectile.
                    target.ApplyDamage(damage, false);
                }

                // If the projectile should die on contact, kill it.
                if(dieOnContact)
                {
                    Kill();
                }
            }
        }

        // Kills the projectile.
        public void Kill()
        {
            // If the shooter is a blaster.
            if(shooterUnit is ActionUnitDefenseBlaster)
            {
                // Downcast to blaster.
                ActionUnitDefenseBlaster blaster = (ActionUnitDefenseBlaster)shooterUnit;

                // Called when this projectile has been killed.
                blaster.OnProjectileKilled(this);

                // Set shooter to null.
                shooterUnit = null;
            }

            // Destroys object.
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            // If the game is playing and the game is unpaused.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // Sets the velocity and then clamps it.
                rigidbody.velocity = moveDirec * moveSpeed;
                rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, moveSpeed);

                // If not within the stage bounds, destroy this projectile.
                if (!actionManager.actionStage.InStageBounds(gameObject))
                {
                    Kill();
                }
            }
            else
            {
                // If there game is paused, or if 
                rigidbody.velocity = Vector2.zero;
            }
        }

        // This function is called when the MonoBehaviour will be destroyed
        private void OnDestroy()
        {
            // The shooter unit exists.
            if(shooterUnit != null)
            {
                // If the shooter is a blaster.
                if (shooterUnit is ActionUnitDefenseBlaster)
                {
                    // Downcast to blaster.
                    ActionUnitDefenseBlaster blaster = (ActionUnitDefenseBlaster)shooterUnit;

                    // Called when this projectile has been killed.
                    blaster.OnProjectileKilled(this);
                }
            }
        }
    }
}