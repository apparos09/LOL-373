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

        // Update is called once per frame
        virtual protected void Update()
        {

        }
    }
}