using LoLSDK;
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
        // Ordered by renewable then nonrenewable, then alphabetically.
        public enum naturalResource { unknown, biomass, hydro, geothermal, solar, wave, wind, coal, naturalGas, nuclear, oil }

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

        // Gets the natural resource name.
        public static string GetNaturalResourceName(NaturalResources.naturalResource res)
        {
            // The result to be returned.
            string result;

            // Gets the key for translation.
            string key = GetNaturalResourceNameKey(res);

            // Checks the resource type to know what name to return.
            switch (res)
            {
                case naturalResource.unknown:
                    result = LOLManager.IsLOLSDKInitialized() ? LOLManager.Instance.GetLanguageText(key) : "Unknown";
                    break;

                case naturalResource.biomass:
                    result = LOLManager.IsLOLSDKInitialized() ? LOLManager.Instance.GetLanguageText(key) : "Biomass";
                    break;

                case naturalResource.hydro:
                    result = LOLManager.IsLOLSDKInitialized() ? LOLManager.Instance.GetLanguageText(key) : "Hydro";
                    break;

                case naturalResource.geothermal:
                    result = LOLManager.IsLOLSDKInitialized() ? LOLManager.Instance.GetLanguageText(key) : "Geothermal";
                    break;

                case naturalResource.solar:
                    result = LOLManager.IsLOLSDKInitialized() ? LOLManager.Instance.GetLanguageText(key) : "Solar";
                    break;

                case naturalResource.wave:
                    result = LOLManager.IsLOLSDKInitialized() ? LOLManager.Instance.GetLanguageText(key) : "Wave";
                    break;

                case naturalResource.wind:
                    result = LOLManager.IsLOLSDKInitialized() ? LOLManager.Instance.GetLanguageText(key) : "Wind";
                    break;

                case naturalResource.coal:
                    result = LOLManager.IsLOLSDKInitialized() ? LOLManager.Instance.GetLanguageText(key) : "Coal";
                    break;

                case naturalResource.naturalGas:
                    result = LOLManager.IsLOLSDKInitialized() ? LOLManager.Instance.GetLanguageText(key) : "Natural Gas";
                    break;

                case naturalResource.nuclear:
                    result = LOLManager.IsLOLSDKInitialized() ? LOLManager.Instance.GetLanguageText(key) : "Nuclear";
                    break;

                case naturalResource.oil:
                    result = LOLManager.IsLOLSDKInitialized() ? LOLManager.Instance.GetLanguageText(key) : "Oil";
                    break;

                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }
        
        // Gets the key for the natural resource, which is used for the language file.
        public static string GetNaturalResourceNameKey(NaturalResources.naturalResource res)
        {
            // The result to be returned.
            string result;

            // Checks the resource type to know what key to return.
            switch(res)
            {
                case naturalResource.unknown:
                    result = "nrs_ukn";
                    break;

                case naturalResource.biomass:
                    result = "nrs_bms";
                    break;

                case naturalResource.hydro:
                    result = "nrs_hdo";
                    break;

                case naturalResource.geothermal:
                    result = "nrs_gtl";
                    break;

                case naturalResource.solar:
                    result = "nrs_slr";
                    break;

                case naturalResource.wave:
                    result = "nrs_wve";
                    break;

                case naturalResource.wind:
                    result = "nrs_wnd";
                    break;

                case naturalResource.coal:
                    result = "nrs_col";
                    break;

                case naturalResource.naturalGas:
                    result = "nrs_ngs";
                    break;

                case naturalResource.nuclear:
                    result = "nrs_nlr";
                    break;

                case naturalResource.oil:
                    result = "nrs_oil";
                    break;

                default:
                    result = string.Empty;
                    break;
            }

            return result;
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
                case naturalResource.naturalGas:
                case naturalResource.nuclear:
                case naturalResource.oil:
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
                case naturalResource.naturalGas:
                case naturalResource.nuclear:
                case naturalResource.oil:
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
                case naturalResource.naturalGas:
                case naturalResource.oil:
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