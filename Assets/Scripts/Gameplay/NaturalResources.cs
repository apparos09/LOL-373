using LoLSDK;
using System.Collections;
using System.Collections.Generic;
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

        // Returns a list of all the natural resource types.
        // If 'includeUnknown' is true, the unknown type is included. If it's false, unknown isn't included.
        public static List<naturalResource> GenerateNaturalResourceTypeList(bool includeUnknown)
        {
            // Makes a list of types.
            List<naturalResource> typeList = new List<naturalResource>()
            {
                naturalResource.unknown,
                naturalResource.biomass,
                naturalResource.hydro, 
                naturalResource.geothermal, 
                naturalResource.solar, 
                naturalResource.wave, 
                naturalResource.wind, 
                naturalResource.coal,
                naturalResource.naturalGas,
                naturalResource.nuclear, 
                naturalResource.oil
            };

            // If unknown shouldn't be included, remove unknown.
            if(!includeUnknown)
            {
                typeList.Remove(naturalResource.unknown);
            }

            return typeList;
        }

        // Gets the natural resource name.
        public static string GetNaturalResourceName(naturalResource res)
        {
            // The result to be returned.
            string result;

            // Gets the key for translation.
            string key = GetNaturalResourceNameKey(res);

            // If the LOL manager has been initialized and the key exists.
            if(LOLManager.IsLOLSDKInitialized() && key != "")
            {
                result = LOLManager.Instance.GetLanguageText(key);
            }
            // Either the LOL SDK isn't initialized or the key is empty, so manually fill the values.
            else
            {
                // Checks the resource type to know what name to return.
                switch (res)
                {
                    case naturalResource.unknown:
                        result = "Unknown";
                        break;

                    case naturalResource.biomass:
                        result = "Biomass";
                        break;

                    case naturalResource.hydro:
                        result = "Hydro";
                        break;

                    case naturalResource.geothermal:
                        result = "Geothermal";
                        break;

                    case naturalResource.solar:
                        result = "Solar";
                        break;

                    case naturalResource.wave:
                        result = "Wave";
                        break;

                    case naturalResource.wind:
                        result = "Wind";
                        break;

                    case naturalResource.coal:
                        result = "Coal";
                        break;

                    case naturalResource.naturalGas:
                        result = "Natural Gas";
                        break;

                    case naturalResource.nuclear:
                        result = "Nuclear";
                        break;

                    case naturalResource.oil:
                        result = "Oil";
                        break;

                    default:
                        result = string.Empty;
                        break;
                }
            }
         

            return result;
        }
        
        // Gets the key for the natural resource, which is used for the language file.
        public static string GetNaturalResourceNameKey(naturalResource res)
        {
            // The result to be returned.
            string result;

            // Checks the resource type to know what key to return.
            switch(res)
            {
                case naturalResource.unknown:
                    result = "kwd_unknown"; // Uses keyword "Unknown".
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

        // Gets the natural resource name.
        public static string GetNaturalResourceNameAbbreviation(naturalResource res)
        {
            // The result to be returned.
            string result;

            // Gets the key for translation.
            string key = GetNaturalResourceNameAbbreviationKey(res);

            // If the LOL manager has been initialized and the key exists.
            if (LOLManager.IsLOLSDKInitialized() && key != "")
            {
                result = LOLManager.Instance.GetLanguageText(key);
            }
            // Either the LOL SDK isn't initialized or the key is empty, so manually fill the values.
            else
            {
                // Checks the resource type to know what name abbreviation to return.
                switch (res)
                {
                    case naturalResource.unknown:
                        result = "UKN";
                        break;

                    case naturalResource.biomass:
                        result = "BMS";
                        break;

                    case naturalResource.hydro:
                        result = "HDO";
                        break;

                    case naturalResource.geothermal:
                        result = "GTL";
                        break;

                    case naturalResource.solar:
                        result = "SLR";
                        break;

                    case naturalResource.wave:
                        result = "WVE";
                        break;

                    case naturalResource.wind:
                        result = "WND";
                        break;

                    case naturalResource.coal:
                        result = "COL";
                        break;

                    case naturalResource.naturalGas:
                        result = "NGS";
                        break;

                    case naturalResource.nuclear:
                        result = "NLR";
                        break;

                    case naturalResource.oil:
                        result = "OIL";
                        break;

                    default:
                        result = string.Empty;
                        break;
                }
            }


            return result;
        }

        // Gets the key for the natural resource abbreviation, which is used for the language file.
        public static string GetNaturalResourceNameAbbreviationKey(naturalResource res)
        {
            // The result to be returned.
            string result;

            // Checks the resource type to know what key to return.
            switch (res)
            {
                case naturalResource.unknown:
                    result = "kwd_unknown_abv"; // Uses keyword "Unknown".
                    break;

                case naturalResource.biomass:
                    result = "nrs_bms_abv";
                    break;

                case naturalResource.hydro:
                    result = "nrs_hdo_abv";
                    break;

                case naturalResource.geothermal:
                    result = "nrs_gtl_abv";
                    break;

                case naturalResource.solar:
                    result = "nrs_slr_abv";
                    break;

                case naturalResource.wave:
                    result = "nrs_wve_abv";
                    break;

                case naturalResource.wind:
                    result = "nrs_wnd_abv";
                    break;

                case naturalResource.coal:
                    result = "nrs_col_abv";
                    break;

                case naturalResource.naturalGas:
                    result = "nrs_ngs_abv";
                    break;

                case naturalResource.nuclear:
                    result = "nrs_nlr_abv";
                    break;

                case naturalResource.oil:
                    result = "nrs_oil_abv";
                    break;

                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        // Gets the color assigned to this natural resource.
        public static Color GetNaturalResourceColor(naturalResource res)
        {
            // The color to be returned.
            Color color;

            // TODO: implement.

            // Checks the resource type to know what key to return.
            switch (res)
            {
                default:
                case naturalResource.unknown:
                    color = Color.white;
                    break;

                case naturalResource.biomass:
                    color = Color.white;
                    break;

                case naturalResource.hydro:
                    color = Color.white;
                    break;

                case naturalResource.geothermal:
                    color = Color.white;
                    break;

                case naturalResource.solar:
                    color = Color.white;
                    break;

                case naturalResource.wave:
                    color = Color.white;
                    break;

                case naturalResource.wind:
                    color = Color.white;
                    break;

                case naturalResource.coal:
                    color = Color.white;
                    break;

                case naturalResource.naturalGas:
                    color = Color.white;
                    break;

                case naturalResource.nuclear:
                    color = Color.white;
                    break;

                case naturalResource.oil:
                    color = Color.white;
                    break;
            }

            return color;
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