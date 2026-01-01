using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Action Unit Generator - Natural Gas
    public class ActionUnitGeneratorNaturalGas : ActionUnitGenerator
    {
        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // If the resource is unknown, set it to natural gas.
            if (resource == NaturalResources.naturalResource.unknown)
            {
                resource = NaturalResources.naturalResource.naturalGas;
            }

            // Uses energy cycles.
            if (!useEnergyCycles)
                useEnergyCycles = true;
        }
    }
}