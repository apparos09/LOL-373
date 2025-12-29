using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The defense unit for the player.
    public class ActionUnitDefense : ActionUnitUser
    {

        // // Start is called before the first frame update
        // void Start()
        // {
        // 
        // }
        // 
        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }

        // Gets the unit type.
        public override unitType GetUnitType()
        {
            return unitType.defense;
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
    }
}