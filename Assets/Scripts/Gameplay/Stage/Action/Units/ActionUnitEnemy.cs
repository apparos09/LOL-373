using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action enemy unit.
    public class ActionUnitEnemy : ActionUnit
    {
        [Header("Enemy")]
        // The enemy player.
        public ActionPlayerEnemy playerEnemy;

        // The move speed of the enemy.
        public float moveSpeed = 1.0F;

        // The enemy's movement direction.
        // Enemies go from left to right.
        private Vector3 moveDirec = Vector3.left;

        // The row the action unit is in.
        // TODO: is this needed?
        public int row = -1;

        // The amount of energy the enemy loses when a death occurs.
        public float deathEnergyCost = 1;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Gets the unit type.
        public override unitType GetUnitType()
        {
            return unitType.enemy;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the stage is playing and the game is unpaused.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // Moves the enemy.
                transform.Translate(moveDirec * moveSpeed * Time.deltaTime);
            }
        }
    }
}