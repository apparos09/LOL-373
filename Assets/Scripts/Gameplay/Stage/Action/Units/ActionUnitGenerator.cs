using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A generator unit, which is used to generate power.
    public class ActionUnitGenerator : ActionUnitUser
    {
        // The resource this generator uses.
        public NaturalResources.naturalResource resource;

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
            return unitType.generator;
        }
    }
}