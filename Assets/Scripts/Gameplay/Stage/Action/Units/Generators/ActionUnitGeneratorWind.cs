using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Action Unit Generator - Wind
    public class ActionUnitGeneratorWind : ActionUnitGenerator
    {
        // Start is called just before any of the Update methods is called the first time
        protected override void Start()
        {
            base.Start();

            // If the resource is unknown, set it to wind.
            if (resource == NaturalResources.naturalResource.unknown)
            {
                resource = NaturalResources.naturalResource.wind;
            }

            // Uses the wind to generate energy.
            if(!useWindToGenEnergy)
                useWindToGenEnergy = true;
        }
    }
}