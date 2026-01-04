using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RM_EDU
{
    // A projectile that's power changes based on how far it travels.
    public class ActionProjectileDistancePower : ActionProjectile
    {
        [Header("Distance Power")]

        // If 'true', the distance power changes are used.
        [Tooltip("If true, the power changes based on the projectile's distance from its starting point.")]
        public bool useDistancePowerChange = true;

        // The starting position of the projectile.
        public Vector3 startPosition = Vector3.zero;

        // If 'true', the start position is set in the start function.
        [Tooltip("Sets the start position in the Start() function if true.")]
        public bool setStartPosOnStart = true;

        // The maximum distance the projectile travels while having its power change.
        // The distance max by default is 10 tiles (1.28 x 10 = 12.8).
        [Tooltip("The maximum distance the projectile can travel before it's power is at a fixed amount.")]
        public float attackPowerDistanceMax = ActionStage.TILE_SIZE_X_DEFAULT * 10.0F;

        // The attack power at the maximum distance.
        // The base power is the power of the projectile or shooter based on the projectile's setting.
        [Tooltip("The projectile's power when it's at or beyond its maximum distance")]
        public float attackPowerAtDistanceMax = 10.0F;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Set this to false so that the projectile's attack power...
            // Based on the shooter's attack power is used.
            // In other words, don't pass the shooter for the damage calculation.
            useShooterDirectly = false;

            // Sets the starting position automatically.
            if(setStartPosOnStart)
                startPosition = transform.position;
        }

        // Calculates the attack power.
        public override float CalculateAttackPower(bool updateShooter)
        {
            // Gets the base power.
            float basePower = base.CalculateAttackPower(updateShooter);

            // The modified power.
            float modPower = basePower;

            // The distance from start.
            float distFromStart = Vector3.Distance(startPosition, transform.position);

            // If it's at or greater than the maximum distance, set it to the end power.
            if(distFromStart >= attackPowerDistanceMax)
            {
                // Sets as the attack power at the maximum distance.
                modPower = attackPowerAtDistanceMax;
            }
            else
            {
                // Gets the t-value and uses it to lerp the modified power.
                float t = Mathf.Clamp01(distFromStart / attackPowerDistanceMax);
                modPower = Mathf.Lerp(basePower, attackPowerAtDistanceMax, t);
            }

            // Returns the modifiedp ower.
            return modPower;
        }

    }
}