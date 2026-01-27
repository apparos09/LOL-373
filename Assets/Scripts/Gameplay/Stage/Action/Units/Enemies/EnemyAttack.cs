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

        // The animator for the enemy attack.
        public Animator animator;

        // The target the enemy attack is being applied to.
        // public ActionUnit target;

        // Start is called before the first frame update
        void Start()
        {
            // Autosets the unit enemy. The attack should be a child of the enemy it belongs to.
            if (unitEnemy == null)
                unitEnemy = GetComponentInParent<ActionUnitEnemy>();

            // Sets animator if not set already.
            if(animator == null)
                animator = GetComponent<Animator>();
        }

        // TARGET

        // // Returns 'true' if the enemy attack has a target.
        // public bool HasTarget()
        // {
        //     return target != null;
        // }
        // 
        // // Sets the new target.
        // public void SetTarget(ActionUnit newTarget, bool updatePos = true)
        // {
        //     target = newTarget;
        // }

        // POSITIONING
        // Sets to the target position.
        public void SetToTargetPosition(Vector3 target, Vector3 offset)
        {
            transform.position = target + offset;
        }

        // Sets to the target position.
        public void SetToTargetPosition(Vector3 target)
        {
            SetToTargetPosition(target, Vector3.zero);
        }

        // Sets to the target position.
        public void SetToTargetPosition(GameObject target, Vector3 offset)
        {
            SetToTargetPosition(target.transform.position, offset);
        }

        // Sets to the target position.
        public void SetToTargetPosition(GameObject target)
        {
            SetToTargetPosition(target.transform.position, Vector3.zero);
        }

        // Sets the attack's local position to zero.
        public void SetLocalPositionToZero()
        {
            transform.localPosition = Vector3.zero;
        }
    }
}