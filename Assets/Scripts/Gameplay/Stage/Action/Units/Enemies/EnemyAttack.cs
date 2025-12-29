using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The enemy attack.
    public class EnemyAttack : MonoBehaviour
    {
        // The action unit enemy this enemy attack belongs to.
        public ActionUnitEnemy unitEnemy;

        // The collider for this attack.
        public new Collider2D collider;

        // The rigidbody of the enemy attack.
        public new Rigidbody2D rigidbody;

        // Start is called before the first frame update
        void Start()
        {
            // Autosets the unit enemy. The attack should be a child of the enemy it belongs to.
            if (unitEnemy == null)
                unitEnemy = GetComponentInParent<ActionUnitEnemy>();

            // Autosets the collider if it's not set.
            if(collider == null)
                collider = GetComponent<Collider2D>();

            // Tries to automatically get the rigid body.
            if(rigidbody == null)
                rigidbody = GetComponent<Rigidbody2D>();

            // If the unit enemy exists, make the attack be unable to collide with the enemy.
            if (unitEnemy != null)
                IgnoreUnitEnemyCollider(true);
        }

        // OnTriggerEnter2D is called when the Collider2D enters the trigger (2D physics only).
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // The action user unit from this collider.
            ActionUnitUser colUserUnit;

            // Tries to get the user collision unit.
            if(collision.TryGetComponent(out colUserUnit))
            {
                // If there is a user unit, try to damage it.
                unitEnemy.AttackUnit(colUserUnit);
            }
        }

        // Ignores the collider of the enemy this attack belongs to.
        public void IgnoreUnitEnemyCollider(bool ignore = true)
        {
            // If the enemy exists.
            if(unitEnemy != null)
            {
                // If the enemy and the attack colliders exist.
                if(unitEnemy.collider != null && collider != null)
                {
                    Physics2D.IgnoreCollision(unitEnemy.collider, collider, ignore);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}