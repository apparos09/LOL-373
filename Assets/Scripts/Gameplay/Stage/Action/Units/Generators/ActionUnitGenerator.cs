using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace RM_EDU
{
    // A generator unit, which is used to generate power.
    public class ActionUnitGenerator : ActionUnitUser
    {
        // Used to determine what time of day the generator can be used.
        public enum timeConstraint { allDay, dayOnly, nightOnly };

        [Header("Generator")]

        // The resource this generator uses.
        public NaturalResources.naturalResource resource;

        // The energy generation timer. This is set to the energyGenerationSpeed when counting down.
        [Tooltip("The timer for generating energy. When the timer hits 0, energy is generated.")]
        public float energyGenerationTimer = 0.0F;

        // The usage of the generator based on the day.
        [Tooltip("The time of day where the generator can generate energy.")]
        public timeConstraint timeUsage = timeConstraint.allDay;

        // If 'true', the generator can only generate energy when there's wind.
        [Tooltip("If true, the generator uses wind to generate energy.")]
        public bool useWindToGenEnergy = false;

        // The total amount of energy generated.
        [Tooltip("The total amount of energy generated.")]
        public float energyGenerationTotal = 0.0F;

        // The energy generaiton limit.
        [Tooltip("The limit on how much energy can be generated.")]
        public float energyGenerationLimit = -1.0F;

        // If 'true', the generator can only generate a certain amount of energy.
        [Tooltip("Is true if the generator has an energy generation limit.")]
        public bool useEnergyGenLimit = false;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Set the generation timer to max.
            SetEnergyGenerationTimerToMax();
        }

        // Gets the unit type.
        public override unitType GetUnitType()
        {
            return unitType.generator;
        }

        // Gets the display name for the unit's card.
        public override string GetUnitCardDisplayName()
        {
            return GetNaturalResourceNameAbbreviation();
        }

        // Gets the natural resource name.
        public string GetNaturalResourceName()
        {
            return NaturalResources.GetNaturalResourceName(resource);
        }

        // Gets the natural resource name key.
        public static string GetNaturalResourceNameKey(NaturalResources.naturalResource resource)
        {
            return NaturalResources.GetNaturalResourceNameKey(resource);
        }

        // Gets the natural resource name abbreviation.
        public string GetNaturalResourceNameAbbreviation()
        {
            return NaturalResources.GetNaturalResourceNameAbbreviation(resource);
        }

        // Gets the natural resource name key.
        public static string GetNaturalResourceNameAbbreviationKey(NaturalResources.naturalResource resource)
        {
            return NaturalResources.GetNaturalResourceNameAbbreviationKey(resource);
        }

        // Gets the maximum of the energy generation timer.
        public virtual float GetEnergyGenerationTimerMax()
        {
            // The value to be returned.
            float value;

            // If the speed is 0, set the value to 0.
            if(energyGenerationSpeed == 0)
            {
                value = 0.0F;
            }
            else
            {
                // The current energy generation speed.
                float currEnergyGenSpeed;

                // If uses wind to generate energy, set the speed based on that.
                if(useWindToGenEnergy)
                {
                    // The incrementer for the speed.
                    // The higher the speed, the shorter the time.
                    float speedInc = BASE_STAT_MAXIMUM / 5;

                    // Checks the wind rating to see the speed.
                    switch (actionManager.GetCurrentWindRating())
                    {
                        default:
                        case statRating.unknown:
                        case statRating.noneMinus:
                        case statRating.none:
                            // NOTE: the function CanGenerateEnergy() should prevent the generator from...
                            // Reaching this function to set the energy generation speed.
                            currEnergyGenSpeed = energyGenerationSpeed; // Default
                            break;

                        case statRating.veryLow:
                            currEnergyGenSpeed = speedInc * 1;
                            break;

                        case statRating.low:
                            currEnergyGenSpeed = speedInc * 2;
                            break;

                        case statRating.medium:
                            currEnergyGenSpeed = speedInc * 3;
                            break;

                        case statRating.high:
                            currEnergyGenSpeed = speedInc * 4;
                            break;

                        case statRating.veryHigh:
                        case statRating.maximum:
                        case statRating.maximumPlus:
                            currEnergyGenSpeed = speedInc * 5;
                            break;
                    }
                }
                // Fixed generation time max.
                else
                {
                    currEnergyGenSpeed = energyGenerationSpeed;
                }

                // Calculates the value based on the current energy generation speed.
                // Calculation: 1 sec + 10 sec * ((maxSpeed - speed) / maxSpeed)
                value = 1.0F + 10.0F * (BASE_STAT_MAXIMUM - Mathf.Abs(currEnergyGenSpeed)) / BASE_STAT_MAXIMUM;

                // Bounds check.
                if (value < 0)
                    value = 0.0F;
            }

            // Returns the value.
            return value;
        }

        // Sets the energy generation timer to max.
        public virtual void SetEnergyGenerationTimerToMax()
        {
            energyGenerationTimer = GetEnergyGenerationTimerMax();
        }

        // Returns 'true' if the generator can generate energy.
        public bool CanGenerateEnergy()
        {
            // Gets set to 'true' if the generator can operate at the current time.
            bool validTime = true;

            // If the day usage is not all day, check if the time of day is right.
            if (timeUsage != timeConstraint.allDay)
            {
                // Checks the time of day to see if the generator is usable.
                switch (timeUsage)
                {
                    case timeConstraint.dayOnly:
                        validTime = actionManager.IsDayTime();
                        break;

                    case timeConstraint.nightOnly:
                        validTime = actionManager.IsNightTime();
                        break;
                }
            }

            // Gets set to 'true' if the wind check was valid.
            // If the generator doesn't use wind, this will always be true.
            bool windValid = true;

            // Checks if wind shoudl be used to generate energy.
            if (useWindToGenEnergy)
            {
                windValid = actionManager.IsWindBlowing();
            }

            // Gets set to 'true' if the energy generation limit has been reached.
            bool energyGenLimitReached = false;

            // If the energy gen limit is being used.
            if (useEnergyGenLimit)
            {
                energyGenLimitReached = energyGenerationTotal >= energyGenerationLimit;
            }

            // Calculates the result.
            bool result = validTime && windValid && !energyGenLimitReached;

            // Returns the result.
            return result;
        }

        // Generates the energy and resets the generation timer.
        public virtual float GenerateEnergy()
        {
            // The energy being generated.
            float energy = CalculateEnergyGenerationAmount();

            // Add to the energy generation total.
            energyGenerationTotal += energy;

            // Sets the timer to max.
            SetEnergyGenerationTimerToMax();

            // If the energy generation limit has been reached...
            // Call the appropriate function.
            if (HasEnergyGenerationLimitBeenReached())
            {
                OnEnergyGenerationLimitReached();
            }

            return energy;
        }

        // Has an energy generation limit.
        public bool HasEnergyGenerationLimit()
        {
            return useEnergyGenLimit;
        }

        // Returns 'true' if the energy generation limit has been reached.
        public bool HasEnergyGenerationLimitBeenReached()
        {
            return useEnergyGenLimit && energyGenerationTotal >= energyGenerationLimit;
        }

        // Called when the energy generation limit has been reached.
        // This is only called at the time of the limit being reached.
        public virtual void OnEnergyGenerationLimitReached()
        {
            // ...
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
            // If the stage is playing and the game is unpaused.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // If energy can be generated, run the timer for generating energy.
                if (CanGenerateEnergy())
                {
                    // Reduce timer.
                    energyGenerationTimer -= Time.deltaTime;

                    // Time to generate new energy.
                    if (energyGenerationTimer <= 0.0F)
                    {
                        // Set timer to 0.
                        energyGenerationTimer = 0.0F;

                        //  The new energy that was created.
                        float newEnergy = GenerateEnergy();

                        // Increase the energy of the owner.
                        if (owner != null)
                        {
                            owner.energy += newEnergy;
                        }
                    }
                }
                else
                {
                    // Keep the timer at max if no energy can be generated.
                    energyGenerationTimer = GetEnergyGenerationTimerMax();
                }
            }
        }

    }
}