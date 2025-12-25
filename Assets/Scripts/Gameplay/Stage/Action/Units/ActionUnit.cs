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
        // noneMinus means the stat is lower than 0.
        // maxPlus means the stat is at its above its maximum.
        public enum statRating { unknown, noneMinus, none, veryLow, low, medium, high, veryHigh, maximum, maxPlus }

        // The action manager.
        public ActionManager actionManager;

        // The ID number of the action unit.
        public int idNumber = 0;

        // The sprite renderer.
        public SpriteRenderer spriteRenderer;

        // The animator.
        public Animator animator;

        // The collider.
        public new Collider2D collider;

        // The rigid body.
        public new Rigidbody2D rigidbody;

        [Header("Unit Stats")]

        // The stat factor, which is multipled by each stat. This is multipled by a stat for a given entity.
        // Note that some entites may not use this factor for certain stats.
        public float statFactor = 1.0F;

        // The energy creation cost of the unit. This is how much energy it takes to create a unit.
        public float energyCreationCost = 0.0F;

        // The energy death cost of the unit. This is the amount of energy it costs when a unit dies.
        public float energyDeathCost = 0.0F;

        // The energy generation amount. 
        public float energyGenerationAmount = 0.0F;

        // The energy generation speed.
        public float energyGenerationSpeed = 0.0F;

        // The attack power of the unit.
        public float attackPower = 0.0F;

        // The attack speed of the unit.
        public float attackSpeed = 0.0F;

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

        // Gets the action unit type.
        public abstract unitType GetUnitType();

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
                rating = statRating.maxPlus;
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

        // Kills the unit.
        public virtual void Kill()
        {
            OnUnitDeath();
        }

        // Called when a unit has died/been destroyed.
        public virtual void OnUnitDeath()
        {
            // ...
        }

        // Update is called once per frame
        virtual protected void Update()
        {

        }
    }
}