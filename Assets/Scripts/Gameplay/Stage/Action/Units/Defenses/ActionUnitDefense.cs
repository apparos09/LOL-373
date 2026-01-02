using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The defense unit for the player.
    public class ActionUnitDefense : ActionUnitUser
    {
        // [Header("Defense")]



        // // Start is called before the first frame update
        // void Start()
        // {
        // 
        // }

        // OnTriggerEnter2D is called when the Collider2D other enters this trigger (2D physics only)
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
        }

        // OnTriggerStay2D is called once per frame for every Collider2D other that is touching this trigger (2D physics only)
        protected override void OnTriggerStay2D(Collider2D collision)
        {
            base.OnTriggerStay2D(collision);

            // If this unit can attack.
            if (CanAttack())
            {
                // Checks if colliding with an enemy unit.
                ActionUnitEnemy enemyUnit;

                // Tries to get the component.
                if (collision.TryGetComponent(out enemyUnit))
                {
                    // Perform an attack.
                    PerformAttack();
                }
            }
        }

        // Gets the unit type.
        public override unitType GetUnitType()
        {
            return unitType.defense;
        }

        // Returns 'true' if the defense unit has a target to attack.
        // By default, this checks if there are any enemies in the same row as this defense unit.
        public virtual bool HasTarget()
        {
            // Result to be returned.
            bool hasTarget = false;

            // On a tile.
            if (tile != null)
            {
                // TODO: enable limit on range.

                // Gets the tile's row.
                int row = tile.GetMapRowPosition();

                // If the row is valid.
                if(row >= 0 && row < actionManager.actionStage.MapRowCount)
                {
                    // The defense has a target.
                    hasTarget = actionManager.actionStage.IsEnemyInRowRightOfPosition(row, transform.position, true, false);
                }

            }

            return hasTarget;
        }

        // Performs an attack.
        public virtual void PerformAttack()
        {
            OnUnitAttackPerformed();
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
            base.Update();

            // If the action unit can attack.
            if(CanAttack())
            {
                // If the defense has a target.
                if(HasTarget())
                {
                    // Performs an attack.
                    PerformAttack();
                }
            }
        }
    }
}