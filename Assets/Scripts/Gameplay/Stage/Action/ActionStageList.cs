using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Holds all the data for the action stage.
    public class ActionStageList : MonoBehaviour
    {
        // The stage generation data.
        // Since new data is generated everytime a stage data function is called...
        // This is a class, not a struct.
        public class StageGenerationData
        {
            // The map the player will use.
            public string[,] map = null;

            // The overlays on the map.
            // For visual simplicity, this is an int instead of the enum it represents.
            public int[,] overlays = null;

            // The wind ratings.
            public ActionUnit.statRating[] windRatings = null;
        }

        // TODO: create a stage struct that contains the map and wind settings.

        // The singleton instance.
        private static ActionStageList instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // Constructor
        private ActionStageList()
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
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // ...
        }

        // Gets the instance.
        public static ActionStageList Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<ActionStageList>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Action Stage List (singleton)");
                        instance = go.AddComponent<ActionStageList>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Stage maps are 16x7.
        // The lane blasters are all in the first column, so the usable space for the player is 15x7.
        // For the tile set, the two digit number determines the tile type, and the letter on the end determines the version used (e.g., "00A").
        /*
         * Tileset:
         *  0: nothing
         *  1: land
         *  2: river
         *  3: sea
         */

        // Generates a map based on the provided value. If there's no map to match that value, null is returned.
        public static StageGenerationData GenerateStageMap(int idNumber)
        {
            // The data to be returned.
            StageGenerationData data;

            // Checks the value to see what map to return.
            switch(idNumber)
            {
                default:
                    data = new StageGenerationData();
                    break;
                    
                case 0:
                    data = GenerateStageDataDebug();
                    break;
            }

            return data;
        }

        // Generates and returns empty stage data.
        private static StageGenerationData GenerateStageDataEmpty()
        {
            // An empty map to use as a base to make other function.
            string[,] map = new string[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" },
                { "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A", "00A" }
            };

            // An empty tile overlay.
            int[,] overlays = new int[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

            // Wind array of no wind.
            ActionUnit.statRating[] windRatings = new ActionUnit.statRating[ActionManager.WIND_RATINGS_COUNT_DEFAULT];

            // The data object to return.
            StageGenerationData data = new StageGenerationData();

            // Setting data.
            data.map = map;
            data.overlays = overlays;
            data.windRatings = windRatings;

            return data;
        }

        // Generates and returns the debug stage map.
        public static StageGenerationData GenerateStageDataDebug()
        {
            // A debug stage map.
            string[,] map = new string[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { "04A", "01A", "02A", "03A", "03A", "02A", "01A", "00A", "01A", "02A", "03A", "03A", "02A", "01A", "04A" },
                { "04A", "01A", "02A", "03A", "03A", "02A", "01A", "00A", "01A", "02A", "03A", "03A", "02A", "01A", "04A" },
                { "04A", "01A", "02A", "03A", "03A", "02A", "01A", "00A", "01A", "02A", "03A", "03A", "02A", "01A", "04A" },
                { "04A", "01A", "02A", "03A", "03A", "02A", "01A", "00A", "01A", "02A", "03A", "03A", "02A", "01A", "04A" },
                { "04A", "01A", "02A", "03A", "03A", "02A", "01A", "00A", "01A", "02A", "03A", "03A", "02A", "01A", "04A" },
                { "04A", "01A", "02A", "03A", "03A", "02A", "01A", "00A", "01A", "02A", "03A", "03A", "02A", "01A", "04A" },
                { "04A", "01A", "02A", "03A", "03A", "02A", "01A", "00A", "01A", "02A", "03A", "03A", "02A", "01A", "04A" }
            };

            // An debug map overlay.
            int[,] overlays = new int[ActionStage.MAP_ROW_COUNT_DEFAULT, ActionStage.MAP_COLUMN_COUNT_DEFAULT] {
                { 0, 2, 0, 0, 0, 0, 6, 0, 3, 0, 0, 0, 0, 4, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 3, 0, 0, 0, 0, 6, 0, 2, 0, 0, 0, 0, 5, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 4, 0, 0, 0, 0, 5, 0, 2, 0, 0, 0, 0, 6, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 5, 0, 0, 0, 0, 4, 0, 3, 0, 0, 0, 0, 6, 0 }
            };

            // The wind ratings.
            ActionUnit.statRating[] windRatings = new ActionUnit.statRating[ActionManager.WIND_RATINGS_COUNT_DEFAULT]
                {ActionUnit.statRating.none, ActionUnit.statRating.veryLow, ActionUnit.statRating.veryHigh };

            // The data object to return.
            StageGenerationData data = new StageGenerationData();

            // Setting data.
            data.map = map;
            data.overlays = overlays;
            data.windRatings = windRatings;

            return data;
        }


        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}