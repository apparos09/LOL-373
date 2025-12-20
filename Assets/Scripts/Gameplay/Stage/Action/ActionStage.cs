using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action stage.
    public class ActionStage : MonoBehaviour
    {
        // The action manager.
        public ActionManager actionManager;

        // The ID number of the stage, which determines what stage to load.
        public int idNumber = 0;

        // If 'true', the ID number is automatically set from the manager.
        // This is off by default since the manager should be initialized first.
        public bool autoSetIdNumber = false;

        // If 'true', the map is generated in the Start() function.
        public bool generateMapOnStart = false;

        // The action tile prefabs.
        public ActionTilePrefabs actionTilePrefabs;

        // The action stage list.
        public ActionStageList actionStageList;

        [Header("Stage")]

        // The tile parent.
        public GameObject tileParent;

        // The action tiles.
        public List<ActionTile> tiles = new List<ActionTile>();

        // Start is called before the first frame update
        void Start()
        {
            // Gets the action manager.
            if (actionManager == null)
                actionManager = ActionManager.Instance;

            // Gets the instance.
            if (actionTilePrefabs == null)
                actionTilePrefabs = ActionTilePrefabs.Instance;

            // Gets the instance.
            if(actionStageList == null)
                actionStageList = ActionStageList.Instance;

            // Gets the ID number.
            if(autoSetIdNumber)
            {
                idNumber = actionManager.idNumber;
            }

            // If the map should be generated in the Start() function.
            if(generateMapOnStart)
            {
                GenerateMap();
            }
        }

        // Generates a map using the set id.
        // If there are existing tiles, they are destroyed.
        public void GenerateMap()
        {
            // If there are tiles.
            if(tiles.Count > 0)
            {
                // Goes through all the tiles.
                foreach(ActionTile tile in tiles)
                {
                    // If the tile exists, destroy it.
                    if(tile != null)
                    {
                        Destroy(tile.gameObject);
                    }
                }

                // TODO: destroy the assets on the tiles.
            }

            // Clear the list of tiles.
            tiles.Clear();

            // TODO: create the tiles.
        }

        // Generates a map using the provided ID number. Saves the provided as the ID number.
        public void GenerateMap(int idNumber)
        {
            // Sets the id number.
            this.idNumber = idNumber;

            // Generates the map.
            GenerateMap();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}