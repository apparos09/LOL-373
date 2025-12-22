using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action enemy unit.
    public class ActionUnitEnemy : ActionUnit
    {
        [Header("Enemy")]
        // The move speed of the enemy.
        public float moveSpeed = 1.0F;

        // The row the action unit is in.
        public int row = -1;

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
        }
    }
}