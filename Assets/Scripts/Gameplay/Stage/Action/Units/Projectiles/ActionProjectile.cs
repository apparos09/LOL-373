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

        // All the projectiles in the stage.
        private static List<ActionProjectile> actionProjectiles = new List<ActionProjectile>();

        [Header("Attack")]

        // If 'true', the shooter is used for attacking the target instead of the projectile.
        // If 'false', the projectile is used for attacking the target.
        [Tooltip("If true, the shooter is used for attacking the target instead of the projectile when the projectile makes contact with the target.")]
        public bool useShooterDirectly = true;

        // If 'true', the tangible component of a target is ignored.
        [Tooltip("If true, the tangible component of a target is ignored when checking for valid collision.")]
        public bool ignoreTangible = false;

        [Header("Attack/Power, Factor")]

        // The default attack power of the projectile.
        [Tooltip("The default attack power of the projectile.")]
        public float defaultAttackPower = 1.0F;

        // The attack power of the shooter.
        [Tooltip("The attack power of the shooter.")]
        public float shooterAttackPower = 1.0F;

        // The attack factor of the projectile.
        [Tooltip("The attack factor that's applied to the projectile.")]
        public float defaultAttackFactor = 1.0F;

        // The shooter's attack stat factor.
        [Tooltip("The attack factor of the shooter, which is the shooter's stat factor.")]
        public float shooterAttackFactor = 1.0F;

        // If 'true', the attack power and factor of the shooter is applied to the projectile's attack power...
        // Upon collision with a valid target occurring.
        // If false, the projectile's attack power and factor are used.
        [Tooltip("If true, use the attack power and attack factor of the shooter. If false, use the default attack power and factor.")]
        public bool useAttackPowerAndFactorOfShooter = true;

        [Header("Attack/One Hit")]

        // The default 'one hit kill' check. If true, the projectile kills its target in one hit.
        [Tooltip("If true, by default the projectile kills the target in one hit.")]
        public bool defaultOneHitKill = false;

        // The one hit kill value for the shooter.
        [Tooltip("The shooter's one hit kill value, which determines if attacks kill targets in one hit.")]
        public bool shooterOneHitKill = false;

        // If 'true', the shooter's one hit kill value is used.
        [Tooltip("Use the shooter's one hit kill value if available.")]
        public bool useShooterOneHitKill = true;

        [Header("Movement/Contact")]

        // The movement direction.
        public Vector2 moveDirec = Vector2.right;

        // The movement speed. The projectile moves at a fixed speed.
        [Tooltip("The movement speed of a projectile. Projectiles move at a fixed speed.")]
        public float moveSpeed = 25.0F;

        // Valid targets for the projectile.
        // If the list is empty, all units are valid targets.
        public List<ActionUnit.unitType> validTargets = new List<ActionUnit.unitType>();

        // If the projectile should die on contact.
        [Tooltip("If true, the projectile will die on contact with a valid target.")]
        public bool dieOnContact = true;

        // Awake is called when the script instance is being loaded.
        protected virtual void Awake()
        {
            // If this projectile isn't in the projectiles list, add it.
            if (!actionProjectiles.Contains(this))
                actionProjectiles.Add(this);
        }

        // Start is called before the first frame update
        protected virtual void Start()
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

            // If the shooter exists...
            if(shooterUnit != null)
            {
                // Update the shooter attack values.
                UpdateShooterAttackValues();
            }
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

        // Updates the shooter's attack values (attack power and one hit kill).
        public void UpdateShooterAttackValues()
        {
            UpdateShooterAttackPowerAndFactor();
            UpdateShooterOneHitKill();
            
        }

        // Updates the shooter's attack power and attack factor.
        public void UpdateShooterAttackPowerAndFactor()
        {
            shooterAttackPower = (shooterUnit != null) ? shooterUnit.attackPower * shooterUnit.statFactor : 1.0F;
            shooterAttackFactor = (shooterUnit != null) ? shooterUnit.statFactor : 1.0F;
        }

        // Update's the shooter's one hit kill.
        public void UpdateShooterOneHitKill()
        {
            shooterOneHitKill = (shooterUnit != null) ? shooterUnit.oneHitKill : false;
        }

        // Calculates the current attack power based on the parameters.
        // If 'updateShooter' is true, the shooter's values are updated for the calculations.
        public virtual float CalculateAttackPower(bool updateShooter)
        {
            // The power to be returned.
            float power;

            // Calculates attack power for the projectile.
            if (useAttackPowerAndFactorOfShooter)
            {
                // Updates the shooter's value if requested.
                if(updateShooter)
                    UpdateShooterAttackPowerAndFactor();

                // Gets the shooter attack power times its factor.
                power = shooterAttackPower * shooterAttackFactor;
            }
            // Shooter shouldn't be used.
            else
            {
                // Get the default attack power times the default attack factor.
                power = defaultAttackPower * defaultAttackFactor;
            }

            return power;
        }

        // Returns 'true' if the projectile is a one-hit kill based on the current parameters.
        public bool IsOneHitKill(bool updateShooter)
        {
            // Checks if using shooter value or projectile value.
            if (useShooterOneHitKill)
            {
                // Updates the shooter's value if requested.
                if (updateShooter)
                    UpdateShooterOneHitKill();

                // Returns shooter one hit kill.
                return shooterOneHitKill;
            }
            // Shooter shouldn't be used.
            else
            {
                return defaultOneHitKill;
            }
        }

        // Cancles the velocity of the projectile.
        // If 'checkVelocity' is true, then the velocity is checked for being zero first. If it is zero, no change is done.
        public void CancelVelocity(bool checkVelocity = true)
        {
            // If the velocity should be checked for it not being zero.
            if (checkVelocity)
            {
                if (rigidbody.velocity != Vector2.zero)
                    rigidbody.velocity = Vector2.zero;
            }
            // Do it regardless.
            else
            {
                rigidbody.velocity = Vector2.zero;
            }
        }

        // Called to ignore the collision of this projectile with it's shooter.
        public void IgnoreCollisionWithShooter(bool ignore)
        {
            if(shooterUnit != null)
                Physics2D.IgnoreCollision(collider, shooterUnit.collider, ignore);
        }

        // Called wehn the projectile has made contact with a target.
        public virtual void OnContactWithTarget(ActionUnit target)
        {
            // If the valid targets list is empty, or if there are no valid targets...
            // Attack the unit.
            if (validTargets.Count <= 0 || validTargets.Contains(target.GetUnitType()))
            {
                // If the shooter exists, use that to attack the target.
                // This is the more accurate version of applying damage.
                if (useShooterDirectly && shooterUnit != null)
                {
                    // Attack the target with the shooter object.
                    target.AttackUnit(shooterUnit);
                }
                else
                {
                    // Gets the current attack power and one hit kill based on the circumstances.
                    // Since the shooter is null, the saved values are used if the shooter values...
                    // Are meant to be used.
                    // Since there's no shooter, the power and one hit kill shouldn't be updated...
                    // They also logically shouldn't be updated since effects to the shooter now...
                    // Shouldn't effect projectiles that have already been fired.
                    float attackPower = CalculateAttackPower(false);
                    bool oneHitKill = IsOneHitKill(false);

                    // Power for attack.
                    float damage = attackPower;

                    // If this is a one-hit kill projectile, set damage to target's health.
                    if (oneHitKill)
                        damage = target.health;

                    // Apply damage using the projectile.
                    // Since the proper attack calculation hasn't been done, apply it.
                    target.ApplyDamage(damage, false);

                    // Call the kill function.
                    target.Kill();
                }

                // If the projectile should die on contact, kill it.
                if(dieOnContact)
                {
                    Kill();
                }
            }
        }

        // Kills the projectile.
        protected virtual void Kill()
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

        // Kills all projectiles in the game world.
        public static void KillAllActionProjectiles()
        {
            // Goes through all the action projectiles, killing each one.
            for(int i = actionProjectiles.Count - 1;  i >= 0; i--)
            {
                // If the projectile exists, kill it.
                if (actionProjectiles[i] != null)
                {
                    // The projectile should remove itself from the projectile list on its own.
                    actionProjectiles[i].Kill();
                }
                else
                {
                    // If the projectile is null, it shouldn't be in the list, so remove it.
                    actionProjectiles.RemoveAt(i);
                }
            }

            // All projectiles should be dead now, so clear the list.
            actionProjectiles.Clear();
        }

        // Update is called once per frame
        protected virtual void Update()
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
                // If the game is paused or the stage isn't playing, cancel the velocity.
                CancelVelocity();
            }
        }

        // This function is called when the MonoBehaviour will be destroyed
        protected virtual void OnDestroy()
        {
            // If this projectile is in the projectile list, remove it.
            if (actionProjectiles.Contains(this))
                actionProjectiles.Remove(this);

            // The shooter unit exists.
            if (shooterUnit != null)
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