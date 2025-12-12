using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;

namespace RM_EDU
{
    // The natural resources list.
    public class NaturalResources : MonoBehaviour
    {
        // Natural Resource Enum.
        public enum naturalResource { unknown, biomass, hydro, geothermal, solar, wave, wind, coal, oil, naturalGas, nuclear }

        // The singleton instance.
        private static NaturalResources instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // Constructor
        private NaturalResources()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Gets the instance.
        public static NaturalResources Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<NaturalResources>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Natural Resources (singleton)");
                        instance = go.AddComponent<NaturalResources>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Checks if the natural resource is renewable.
        public static bool IsRenewable(naturalResource res)
        {
            // Bool to be returned to see if the natural resource is renewable.
            bool renewable;

            // Checks all types to see which ones are rneewable and which ones aren't.
            switch(res)
            {
                case naturalResource.biomass:
                case naturalResource.hydro:
                case naturalResource.geothermal:
                case naturalResource.solar:
                case naturalResource.wave:
                case naturalResource.wind:
                    renewable = true;
                    break;

                case naturalResource.coal:
                case naturalResource.oil:
                case naturalResource.naturalGas:
                case naturalResource.nuclear:
                    renewable = false;
                    break;

                default:
                    renewable = false;
                    break;
            }

            return renewable;
        }

        // Checks if the natural resource is nonrenewable.
        public static bool IsNonrenewable(naturalResource res)
        {
            // Bool to be returned to see if the natural resource is nonrenewable.
            bool nonrenewable;

            // Checks all types to see which ones are rneewable and which ones aren't.
            switch (res)
            {
                case naturalResource.biomass:
                case naturalResource.hydro:
                case naturalResource.geothermal:
                case naturalResource.solar:
                case naturalResource.wave:
                case naturalResource.wind:
                    nonrenewable = false;
                    break;

                case naturalResource.coal:
                case naturalResource.oil:
                case naturalResource.naturalGas:
                case naturalResource.nuclear:
                    nonrenewable = true;
                    break;

                default:
                    nonrenewable = false;
                    break;
            }

            return nonrenewable;
        }

        // Returns 'true' if the natural resource causes air pollution when used to generate power.
        public static bool CausesAirPollution(naturalResource res)
        {
            // The result to be returned.
            bool result;

            switch (res)
            {
                case naturalResource.biomass:
                    result = false;
                    break;

                case naturalResource.hydro:
                    // Can cause the breakdown of plants which releases methane into the atmosphere.
                    result = true;
                    break;

                case naturalResource.geothermal:
                case naturalResource.solar:
                case naturalResource.wave:
                case naturalResource.wind:
                    result = false;
                    break;

                case naturalResource.coal:
                case naturalResource.oil:
                case naturalResource.naturalGas:
                    // Releases pollutants into the air when used to generate enegry.
                    result = true;
                    break;

                case naturalResource.nuclear:
                    result = false;
                    break;

                default:
                    result = false;
                    break;

            }
            
            // Returns the result.
            return result;
        }
    }
}