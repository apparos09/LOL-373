using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action stage unit.
    public abstract class ActionUnit : MonoBehaviour
    {
        // The unit type.
        public enum unitType { unknown, generator, defense, enemy }

        // The rating a stat can have.
        // Stat noneMinus means the stat is lower than 0.
        // Stat maxPlus means the stat is at its above its maximum.
        /*
         * Ranges are listed below. 
         * NOTE: In some cases None Minus and None might be grouped together. Very High, Maximum, and Maximum Plus may also get grouped together.
         *  - None Minus: <0
         *  - None: 0
         *  - Very Low: 1-20
         *  - Low: 21-40
         *  - Meidum: 41-60
         *  - High: 61-80
         *  - Very High: 81-99
         *  - Maximum: 100
         *  - Maximum Plus: >100
         */
        public enum statRating { unknown, noneMinus, none, veryLow, low, medium, high, veryHigh, maximum, maximumPlus }

        // The action manager.
        public ActionManager actionManager;

        // The ID number of the action unit.
        public int idNumber = 0;

        // The sprite renderer.
        public SpriteRenderer spriteRenderer;

        // The sprite used for this unit's icon.
        public Sprite iconSprite;

        // The sprite used for the card background of this unit.
        public Sprite cardBackgroundSprite;

        // The animator.
        public Animator animator;

        // The collider.
        public new Collider2D collider;

        // The rigid body.
        public new Rigidbody2D rigidbody;

        [Header("Health")]

        // The unit's health.
        [Tooltip("The unit's current health. This is NOT effected by statFactor.")]
        public float health = 0.0F;

        // The unit's max health.
        [Tooltip("The unit's maximum health. This is NOT effected by statFactor.")]
        public float maxHealth = 100.0F;

        // Gets set to 'true' if the unit is vulnerable.
        public bool vulnerable = true;

        [Header("Unit Stats")]

        // The stat factor, which can be multiplied by a stat. This may be left unapplied based on the unit.
        [Tooltip("A factor that modifies certain stats. Whether or not a stat uses the stat factor depends on the unit.")]
        public float statFactor = 1.0F;

        // The energy creation cost of the unit. This is how much energy it takes to create a unit.
        public float energyCreationCost = 0.0F;

        // The energy death cost of the unit. This is the amount of energy it costs when a unit dies.
        public float energyDeathCost = 0.0F;

        // The energy generation amount. This is the BASE amount of energy generated.
        // A dedicated function should be used to calculate the amount in real time based on the stage conditions.
        [Tooltip("The base stat for energy amount generated. May differ based on the current map conditions.")]
        public float energyGenerationAmount = 0.0F;

        // The energy generation speed.
        public float energyGenerationSpeed = 0.0F;

        // The attack power of the unit.
        public float attackPower = 0.0F;

        // The attack speed of the unit.
        public float attackSpeed = 0.0F;

        // Gets set to 'true' if this is an entity that can attack.
        protected bool attackingEnabled = true;

        // The cooldown countdown timer for attacks. This is in seconds.
        [Tooltip("The cooldown count down timer between attacks by this unit.")]
        public float attackCooldownTimer = 0.0F;

        // If 'true', the cooldown timer is used.
        protected bool useAttackCooldownTimer = true;

        // The durability of the unit.
        public float durability = 0.0F;

        // The movement speed of the unit.
        public float movementSpeed = 0.0F;

        // The air pollution caused by this 
        public float airPollution = 0.0F;

        // The recommended maximum value a base stat can have.
        // Note that some stats may go past this maximum.
        // This also doesn't account for applying the statFactor value to a stat.
        public const float BASE_STAT_MAXIMUM = 100.0F;

        // Awake is called when the script instance is being loaded
        protected virtual void Awake()
        {
            // ...
        }

        // Start is called before the first frame update
        virtual protected void Start()
        {
            // Gets the action manager instance.
            if (actionManager == null)
                actionManager = ActionManager.Instance;

            // Gets the collider.
            if(collider == null)
                collider = GetComponent<Collider2D>();

            // Gets the rigidbody.
            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody2D>();

            // Sets the health to the max.
            SetHealthToMax();
        }

        // OnTriggerEnter2D is called when the Collider2D other enters this trigger (2D physics only)
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            
        }

        // // OnTriggerStay2D is called once per frame for every Collider2D other that is touching this trigger (2D physics only)
        // protected virtual void OnTriggerStay2D(Collider2D collision)
        // {
        //     
        // }
        // 
        // // OnTriggerExit2D is called when the Collider2D other has stopped touching the trigger (2D physics only)
        // protected virtual void OnTriggerExit2D(Collider2D collision)
        // {
        //     
        // }

        // UNIT TYPE / RATING //
        // Gets the action unit type.
        public abstract unitType GetUnitType();

        // Gets the unit type name.
        public static string GetUnitTypeName(unitType type)
        {
            // Gets the key.
            string key = GetUnitTypeNameKey(type);

            // The name to return.
            string name;

            // If the SDK is initialized, get the text from it.
            // If it isn't, manually set it.
            if(LOLManager.IsLOLSDKInitialized())
            {
                name = LOLManager.Instance.GetLanguageText(key);
            }
            else
            {
                // Checks the type to know what to return.
                switch (type)
                {
                    default:
                        name = "";
                        break;

                    case unitType.unknown:
                        name = "Unknown";
                        break;

                    case unitType.generator:
                        name = "Generator";
                        break;

                    case unitType.defense:
                        name = "Defense";
                        break;

                    case unitType.enemy:
                        name = "Enemy";
                        break;
                }
            }

            return name;    
        }

        // Gets the unit type name.
        public string GetUnitTypeName()
        {
            return GetUnitTypeName(GetUnitType());
        }

        // Gets the unit type name key.
        public static string GetUnitTypeNameKey(unitType type)
        {
            // The name key to be returned.
            string key;

            // Checks the type to know what to return.
            switch(type)
            {
                default:
                    key = "";
                    break;

                case unitType.unknown:
                    key = "kwd_unknown";
                    break;

                case unitType.generator:
                    key = "kwd_generator";
                    break;

                case unitType.defense:
                    key = "kwd_defense";
                    break;

                case unitType.enemy:
                    key = "kwd_enemy";
                    break;
            }

            return key;
        }

        // Gets the display name for the unit's card.
        public virtual string GetUnitCardDisplayName()
        {
            return GetUnitTypeName();
        }

        // Gets the stat rating, using the base stat maximum for comparison.
        public statRating GetStatRating(float stat)
        {
            return GetStatRating(stat, BASE_STAT_MAXIMUM);
        }

        // Gets the stat rating using the provided stat maximum.
        public statRating GetStatRating(float stat, float statMax)
        {
            // The threshold stats are compared to.
            // The stat maximum is 100.
            float threshold = statMax / 5.0F;

            // The rating to be returned.
            statRating rating = statRating.unknown;

            // Checks the rating to give.
            if(stat > statMax) // 100+ (Above Max)
            {
                rating = statRating.maximumPlus;
            }
            else if(stat == statMax) // 100 (Max)
            {
                rating = statRating.maximum;
            }
            else if (stat >= threshold * 4 + 1) // 81-99
            {
                rating = statRating.veryHigh;
            }
            else if (stat >= threshold * 3 + 1) // 61-80
            {
                rating = statRating.high;
            }
            else if (stat >= threshold * 2 + 1) // 41-60
            {
                rating = statRating.medium;
            }
            else if (stat >= threshold * 1 + 1) // 21-40
            {
                rating = statRating.low;
            }
            else if (stat >= threshold * 0 + 1) // 1-20
            {
                rating = statRating.veryLow;
            }
            else if(stat == 0) // 0 (None)
            {
                rating = statRating.none;
            }
            else if(stat < 0) // -0 (Below 0) 
            {
                rating = statRating.noneMinus;
            }
                
            // Returns the rating.
            return rating;
        }

        // Converts a stat rating to a string.
        public string StatRatingToString(statRating statRating)
        {
            // NOTE: these aren't translated since they aren't displayed in the actual game.

            // The result to be returned.
            string result;

            // Goes through each rating, checking what string to return.
            switch(statRating)
            {
                default:
                    result = string.Empty;
                    break;

                case statRating.unknown:
                    result = "Unknown";
                    break;

                case statRating.noneMinus:
                    result = "None-";
                    break;

                case statRating.none:
                    result = "None";
                    break;

                case statRating.veryLow:
                    result = "Very Low";
                    break;

                case statRating.low:
                    result = "Low";
                    break;

                case statRating.medium:
                    result = "Medium";
                    break;

                case statRating.high:
                    result = "High";
                    break;

                case statRating.veryHigh:
                    result = "Very High";
                    break;

                case statRating.maximum:
                    result = "Maximum";
                    break;

                case statRating.maximumPlus:
                    result = "Maximum+";
                    break;
            }

            return result;
        }

        // HEALTH / VULNERABLE //
        // Sets the unit's health to its max.
        public void SetHealthToMax()
        {
            health = maxHealth;
        }

        // Returns 'true' if the unit is vulnerable to attack.
        public bool IsVulnerable()
        {
            return vulnerable;
        }

        // ENERGY //
        // Calculates the energy generation amount.
        // Override this function if the calculation should be changed.
        public virtual float CalculateEnergyGenerationAmount()
        {
            // Ceil(Amount Stat / Stat Maximum * 10)
            return Mathf.Ceil(energyGenerationAmount / BASE_STAT_MAXIMUM * 100.0F);
        }

        // TILE //
        // Returns 'true' if the unit can use the tile.
        public abstract bool UsableTile(ActionTile tile);

        // ATTACK / DAMAGE //
        // Returns 'true' if the entity has the function for attacking.
        // Use CanAttack to see if an attack is available at this exact moment.
        public bool IsAttackingEnabled()
        {
            return attackingEnabled;
        }

        // Returns 'true' if the action unit is capable of attacking.
        // This checks if the attack cooldown timer is 0.
        public virtual bool CanAttack()
        {
            return attackingEnabled && attackCooldownTimer <= 0.0F;
        }

        // Attacks this unit with another unit.
        public static void AttackUnit(ActionUnit attacker, ActionUnit target)
        {
            // Calculates the attack power with a given target.
            float power = attacker.CalculateAttackPower(target, false);

            // Applies the power as damage to the target.
            target.ApplyDamage(power);

            // Called when a unit has been attacked.
            attacker.OnUnitAttackPerformed(target);
            target.OnUnitAttacked(attacker);
        }

        // The provided unit attacks this unit.
        public virtual void AttackUnit(ActionUnit attacker)
        {
            AttackUnit(attacker, this);
        }

        // Calculates the attack power of this unit based on the provided target.
        // ignoreVulnerable: if true, the vulnerability of the unit is ingored.
        //  - If false, the attack power is 0 if the target is invulnerable.
        public virtual float CalculateAttackPower(ActionUnit target, bool ignoreVulnerable)
        {
            // Calculates the base power.
            float power;

            // Gets set to 'true' if the attack is valid.
            // If vulnerability shouldn't be ignored, check if the target can take damage.
            bool valid = (ignoreVulnerable) ? true : target.vulnerable;

            // If the attack is valid, do the calculation.
            if(valid)
            {
                // 2.0 * statFactor + ((attackPower * 1.25 * statFactor) - (target.durability * 1.5 * target.statFactor))
                power = (2.0F * statFactor) + ((attackPower * 1.25F * statFactor) - (target.durability * 1.5F * target.statFactor));

                // If the power is negative, set the power to 1.
                if (power < 0.0F)
                {
                    power = 1.0F;
                }
            }
            else
            {
                power = 0.0F;
            }

            // Gets the attack power.
            return power;
        }

        // Apply damage to the unit.
        // This does NOT call OnUnitAttacked.
        public void ApplyDamage(float damage)
        {
            // Reduces health by provided damage amount.
            health -= damage;

            // If health is negative, clamp it at 0.
            if (health < 0.0F)
                health = 0.0F;
        }

        // Called when the unit hasp performed an attack.
        // If target is null, there was no target.
        public virtual void OnUnitAttackPerformed(ActionUnit target)
        {
            // Set the attack cooldown timer to the attack speed.
            if(useAttackCooldownTimer)
            {
                // Calculates the attack cooldown.
                attackCooldownTimer = 1.0F + ((BASE_STAT_MAXIMUM - attackSpeed) / BASE_STAT_MAXIMUM * 5.0F);

                // If teh attakc cooldown is negative, set it to 1.
                if (attackCooldownTimer < 0.0F)
                    attackCooldownTimer = 1.0F;
            }
        }

        // Called when the unit has performed an attack.
        public virtual void OnUnitAttackPerformed()
        {
            OnUnitAttackPerformed(null);
        }

        // Called when the unit has been attacked.
        public virtual void OnUnitAttacked(ActionUnit attacker)
        {
            // If the unit is now dead, call the kill function.
            if(health <= 0.0F)
            {
                health = 0.0F;
                Kill();
            }
        }

        // Called when a unit has been attacked, setting the attacker to 'null'.
        public virtual void OnUnitAttacked()
        {
            OnUnitAttacked(null);
        }

        // Kills the unit.
        public virtual void Kill()
        {
            // TODO: add animation.
            health = 0.0F;
            OnUnitDeath();
        }

        // Called when a unit has died/been destroyed.
        public virtual void OnUnitDeath()
        {
            // Destroys this unit.
            Destroy(gameObject);
        }

        // Returns 'true' if the unit's health is less than it's max health.
        public bool IsDead()
        {
            return health <= 0.0F;
        }

        // Update is called once per frame
        virtual protected void Update()
        {
            // The stage is operating.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // If the attack cooldown timer should be used.
                if(useAttackCooldownTimer)
                {
                    // If the cooldown timer is not 0, reduce it.
                    if(attackCooldownTimer > 0.0F)
                    {
                        attackCooldownTimer -= Time.deltaTime;

                        // Clamp to 0.
                        if (attackCooldownTimer < 0.0F)
                            attackCooldownTimer = 0.0F;
                    }
                }

                // TODO: check if in dearth animation.
                // Checks if the unit is dead.
                if(IsDead())
                {
                    OnUnitDeath();
                }
            }
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // ...
        }
    }
}