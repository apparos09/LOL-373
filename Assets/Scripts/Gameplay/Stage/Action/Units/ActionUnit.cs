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

        // The name of the unit.
        public string unitName = "";

        // The unit name key.
        public string unitNameKey = "";

        // The unit description.
        public string unitDesc = "";

        // The unit description key.
        public string unitDescKey = "";

        // The owner of this unit.
        public ActionPlayer owner;

        [Header("Visuals")]

        // The sprite renderer.
        public SpriteRenderer spriteRenderer;

        // The sprite used for this unit's icon.
        public Sprite iconSprite;

        // The sprite used for the card background of this unit.
        public Sprite cardBackgroundSprite;

        // The animator.
        public Animator animator;

        // The animations for the action unit.
        public ActionUnitAnimations unitAnimations;

        // If 'true', the death animation is enabled. Off by default.
        protected bool deathAnimationEnabled = false;

        // If 'true', animations are enabled.
        private bool animationsEnabled = true;

        [Header("Physics")]

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

        // If 'true', the unit acts as a physical entity, which can determine if other units can get past it or not.
        [Tooltip("If true, other units should stop when they physically hit this entity. If false, units can go through this entity.")]
        public bool tangible = true;

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

        // The amount of energy used when an attack is performed.
        public float attackEnergyCost = 0.0F;

        // The attack power of the unit.
        public float attackPower = 0.0F;

        // The attack speed of the unit.
        public float attackSpeed = 0.0F;

        // Gets set to 'true' if this is an entity that can attack.
        public bool attackingEnabled = true;

        // The cooldown countdown timer for attacks. This is in seconds.
        [Tooltip("The cooldown count down timer between attacks by this unit.")]
        public float attackCooldownTimer = 0.0F;

        // If 'true', the cooldown timer is used.
        protected bool useAttackCooldownTimer = true;

        // If true, attackes from this unit instantly kill the target.
        [Tooltip("If true, attacks from this unit are one hit kill.")]
        public bool oneHitKill = false;

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

        [Header("Audio")]

        // The destroyed sound effect.
        public AudioClip unitDestroyedSfx;

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

            // Visuals
            // Gets the animator.
            if(animator == null)
                animator = GetComponent<Animator>();

            // Gets the unit animations.
            if (unitAnimations == null)
                unitAnimations = GetComponent<ActionUnitAnimations>();

            // Physics
            // Gets the collider.
            if(collider == null)
                collider = GetComponent<Collider2D>();

            // Gets the rigidbody.
            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody2D>();

            // Other
            // If the LOL SDK is initialized.
            if(LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
            {
                // The name key exists, so try to get the name.
                if(unitNameKey != string.Empty)
                {
                    // Translate the name.
                    unitName = LOLManager.GetLanguageTextStatic(unitNameKey);
                }
            }

            // Sets the health to the max.
            SetHealthToMax();
        }

        // OnTriggerEnter2D is called when the Collider2D other enters this trigger (2D physics only)
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            // ...
        }

        // OnTriggerStay2D is called once per frame for every Collider2D other that is touching this trigger (2D physics only)
        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            // ...
        }
        
        // OnTriggerExit2D is called when the Collider2D other has stopped touching the trigger (2D physics only)
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            // ...
        }

        // NAME, DESCRIPTION, UNIT TYPE / RATING //

        // Gets the unit name translated.
        // If the LOLSDK isn't initialized, or the key is empty...
        // It returns the value saved in unitName.
        public string GetUnitNameTranslated()
        {
            // The result to be returned.
            string result;

            // LOL SDK Initialized and the key is set, so get the translated text
            if (LOLManager.IsLOLSDKInitialized() && unitNameKey != "")
            {
                result = LOLManager.GetLanguageTextStatic(unitNameKey);
            }
            else
            {
                result = unitName;
            }

            return result;
        }

        // Gets the unit description translated.
        public string GetUnitDescriptionTranslated()
        {
            // The result to be returned.
            string result;

            // LOL SDK Initialized and the key is set, so get the translated text.
            if (LOLManager.IsLOLSDKInitialized() && unitDescKey != "")
            {
                result = LOLManager.GetLanguageTextStatic(unitDescKey);
            }
            else
            {
                result = unitDesc;
            }

            return result;
        }

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

        // Generates an info log entry for this unit.
        public InfoLog.InfoLogEntry GenerateInfoLogEntry()
        {
            // Creates an entry.
            InfoLog.InfoLogEntry entry = new InfoLog.InfoLogEntry();

            // Sets the values.
            entry.name = GetUnitNameTranslated();
            entry.nameKey = unitNameKey;
            entry.description = GetUnitDescriptionTranslated();
            entry.descriptionKey = unitDescKey;
            entry.iconSprite = iconSprite;

            return entry;
        }

        // STATS //
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

        // Gets the attack power modified by the stat factor.
        public float GetAttackPowerStatModified()
        {
            return attackPower * statFactor;
        }

        // Gets the attack speed modified by the stat factor.
        public float GetAttackSpeedStatModified()
        {
            return attackSpeed * statFactor;
        }

        // Gets the durability stat modified by the stat factor.
        public float GetDurabilityStatModified()
        {
            return durability * statFactor;
        }

        // Gets the movement speed modified by the stat factor.
        public float GetMovementSpeedStatModified()
        {
            return movementSpeed * statFactor;
        }


        // HEALTH / VULNERABLE //
        // Sets the unit's health to its max.
        public void SetHealthToMax()
        {
            health = maxHealth;
        }

        // Returns 'true' if the unit is tangible.
        // If it's tangible, it should be treated like a physical entity.
        public bool IsTangible()
        {
            return tangible;
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
            return Mathf.Ceil(energyGenerationAmount / BASE_STAT_MAXIMUM * 10.0F);
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
            // Checks if attacking is enabled.
            if(attackingEnabled)
            {
                // Checks if the cooldown is finished to see if the unit can attack.
                bool canAttack = attackCooldownTimer <= 0.0F;

                // Used to see if there's energy for the attack (true by default in case there's no owner).
                bool hasEnergy = true;

                // Check's owner's energy amount.
                if (owner != null)
                {
                    // The owner has energy to perform the attack.
                    hasEnergy = owner.HasEnergyForAttack(this);
                }

                // Returns can attack and has energy for attack.
                return canAttack && hasEnergy;
            }
            else
            {
                // Attacking isn't enabled.
                return false;
            }
        }

        // Attacks this unit with another unit.
        public static void AttackUnit(ActionUnit attacker, ActionUnit target)
        {
            // Used to determine if vulnerability of the target should be ignored.
            bool ignoreVulnerable = false;

            // Calculates the attack power with a given target.
            float power = attacker.CalculateAttackPower(target, ignoreVulnerable);

            // If the attacker is an one hit kill attacker...
            // Kill target is one hit.
            if(attacker.oneHitKill)
            {
                power = target.health;
            }

            // Applies the power as damage to the target.
            // The attack calculation has already been applied, so don't do it again.
            target.ApplyDamage(power, ignoreVulnerable);

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
                // Calculates the power.
                power = CalculateDamage(statFactor, attackPower, target.statFactor, target.durability, false);
            }
            else
            {
                power = 0.0F;
            }

            // Gets the attack power.
            return power;
        }

        // Calculates damage based on the provided values.
        // If 'allowNegative' is true, the raw value is returned.
        // If 'allowNegative' is false, the damage rounds up to 1 if the attackerPower is greater than 0.
        public static float CalculateDamage(float attackerStatFactor, float attackerPower, float targetStatFactor, float targetDurability, bool allowNegative = false)
        {
            // 2.0 * statFactor + ((attackPower * 1.80 * statFactor) - (target.durability * 1.20 * target.statFactor))
            // The amount of damage being done.
            float damage = (5.0F * attackerStatFactor) + 
                ((attackerPower * 1.85F * attackerStatFactor) - (targetDurability * 1.15F * targetStatFactor));


            // If the damage is negative and allowNegative is false...
            if (damage <= 0.0F && !allowNegative)
            {
                // If the attacker power is greater than 0, make sure some damage is done.
                if(attackerPower > 0)
                {
                    // If the attacker power is greater than 0, do at least 1 damage.
                    damage = 1.0F;
                }
                // Attack power is 0 or less, so do no damage.
                else
                {
                    damage = 0.0F;
                }
            }

            return damage;
        }

        // Calculates damage based on attack pwoer and target durability.
        // The stat factors for the attacker and the target are both 1.
        public static float CalculateDamage(float attackerPower, float targetDurability, bool allowNegative = false)
        {
            return CalculateDamage(1, attackerPower, 1, targetDurability, allowNegative);
        }


        // Apply damage to the unit.
        // applyAttackCalc: if true, the attack calculation is applied to the damage. If false, the damage is applied with no modifications.
        //  - If applying the attack calculation, its assumed that the provided damage is the final attack power as part of that calculation.
        // ignoreVulnerable: if true, the vulnerability of the unit is ignored.
        // This does NOT call OnUnitAttacked.
        public virtual void ApplyDamage(float damage, bool ignoreVulnerable)
        {
            // If the unit is vulnerable, or if the vulnerability of this unit should be ignored.
            if(vulnerable || ignoreVulnerable)
            {
                // Reduces health by provided damage amount.
                health -= damage;
            }

            // If health is negative, clamp it at 0.
            if (health < 0.0F)
                health = 0.0F;
        }

        // Calculates the energy creation cost.
        public static float CalculateAttackEnergyCost(float attackEnergyCost)
        {
            // Divide by 10, multiply by 1.5 and round up to the nearest value.
            return Mathf.Ceil(attackEnergyCost / 10.0F * 1.5F);
        }

        // Calculates the attack cooldown time.
        public static float CalculateAttackCooldownTime(float attackSpeed, float attackSpeedMaximum)
        {
            // 1.0F + ((BASE_STAT_MAXIMUM - attackSpped) / BASE_STAT_MAXIMUM * 5.0F)
            return 1.0F + ((attackSpeedMaximum - attackSpeed) / attackSpeedMaximum * 5.0F);
        }

        // Calculates the attack cooldown time.
        public virtual float CalculateAttackCooldownTime()
        {
            // 1.0F + ((BASE_STAT_MAXIMUM - attackSpped) / BASE_STAT_MAXIMUM * 5.0F)
            // return 1.0F + ((BASE_STAT_MAXIMUM - attackSpeed) / BASE_STAT_MAXIMUM * 5.0F);
            return CalculateAttackCooldownTime(attackSpeed, BASE_STAT_MAXIMUM);
        }

        // Calculates and sets the attack cooldown time.
        public void CalculateAndSetAttackCooldownTime()
        {
            attackCooldownTimer = CalculateAttackCooldownTime();
        }

        // Called when the unit hasp performed an attack.
        // If target is null, there was no target.
        public virtual void OnUnitAttackPerformed(ActionUnit target)
        {
            // If the owner is set.
            if(owner != null)
            {
                // Reduce the owner's energy.
                owner.energy -= CalculateAttackEnergyCost(attackEnergyCost);

                // Prevent energy from being negative.
                if (owner.energy < 0.0F)
                    owner.energy = 0.0F;
            }

            // Set the attack cooldown timer to the attack speed.
            if(useAttackCooldownTimer)
            {
                // Calculates and sets the attack cooldown.
                CalculateAndSetAttackCooldownTime();

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
                // If the attacker isn't null.
                if(attacker != null)
                {
                    // If the attacker has an owner, add to its kill count.
                    if (attacker.owner != null)
                    {
                        attacker.owner.kills++;
                    }
                }

                // Health is zero.
                health = 0.0F;

                // Call kill.
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
            
            // Set health to zero.
            health = 0.0F;

            // Reduce the owner's energy by the death cost.
            if (owner != null)
                owner.energy -= energyDeathCost;

            // If animations are enabled, the death animation is enabled, there is a death animation.
            if(animationsEnabled && deathAnimationEnabled && unitAnimations.HasDeathAnimation())
            {
                // Play the death animation.
                unitAnimations.PlayDeathAnimation();
            }
            else
            {
                // Skip death animation.
                OnUnitDeath();
            }
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

        // ANIMATIONS
        // Returns 'true' if animations are enabled.
        public bool AnimationsEnabled
        {
            get { return animationsEnabled; }
        }

        // Returns 'true' if animation is available and enabled.
        public bool IsAnimationsAvailableAndEnabled()
        {
            return animator != null && animationsEnabled;
        }

        // Plays the empty state animation.
        public void PlayEmptyStateAnimation(int layer)
        {
            unitAnimations.PlayEmptyStateAnimation(layer);
        }

        // Plays the empty state animation on the base layer.
        public virtual void PlayEmptyStateAnimationBaseLayer()
        {
            unitAnimations.PlayEmptyStateAnimationBaseLayer();
        }

        // Plays the empty state animation on the overlay layer.
        public virtual void PlayEmptyStateAnimationOverlayLayer()
        {
            unitAnimations.PlayEmptyStateAnimationOverlayLayer();
        }

        // Returns 'true' if the death animation is enabled.
        public bool IsDeathAnimationEnabled()
        {
            return deathAnimationEnabled;
        }

        // Plays the death animation.
        public virtual void PlayDeathAnimation()
        {
            unitAnimations.PlayDeathAnimation();
        }

        // AUDIO //
        // Returns 'true' if the unit can play audio. This is for the world only.
        public bool CanPlayAudio()
        {
            // The result to return.
            bool result;

            // The action audio has been instantiated.
            if (ActionAudio.Instantiated)
            {
                // The SFX world source is active and enabled.
                if(ActionAudio.Instance.sfxWorldSource.isActiveAndEnabled)
                {
                    // The owner exists.
                    if (owner != null)
                    {
                        // The owner's audio is enabled.
                        result = owner.IsAudioEnabled();
                    }
                    // Owner not set, so assume audio can be played.
                    else
                    {
                        result = true;
                    }
                }
                // SFX World Source is active and enabled.
                else
                {
                    result = false;
                }
            }
            // Action audio isn't available.
            else
            {
                result = false;
            }
            
            return result;
        }

        // Plays the unit destroyed sfx.
        public void PlayUnitDestroyedSfx()
        {
            if(CanPlayAudio())
            {
                ActionAudio.Instance.PlaySoundEffectWorld(unitDestroyedSfx);
            }
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

                // TODO: check if in death animation.
                // Checks if the unit is dead.
                if(IsDead())
                {
                    // If it's a proper death, call the Kill() function.
                    Kill();
                    // OnUnitDeath();
                }

                // Check if within stage bounds.
                if(!actionManager.actionStage.InStageBounds(gameObject))
                {
                    // TODO: maybe call kill instead of OnUnitDeath() if there's a way to skip a death animation.
                    // Not in stage bounds, so destroy the unit.
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