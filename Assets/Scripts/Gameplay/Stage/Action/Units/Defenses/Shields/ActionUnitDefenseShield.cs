using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // An action unit shield.
    public class ActionUnitDefenseShield : ActionUnitDefense
    {
        // [Header("Defense/Shield")]

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the defense type is unknown, set to default value based on script.
            if (defType == defenseType.unknown)
            {
                defType = defenseType.shield;
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}