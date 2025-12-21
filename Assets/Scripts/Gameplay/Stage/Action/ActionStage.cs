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

        // The tile sprite size in pixels (length, width)
        private Vector2 tileSize = new Vector2(1.28F, 1.28F);

        // The original of the map.
        // (0.5, 0.5) is the centre, (1, 1) is the top right corner and (0, 0) is the bottom left corner.
        protected Vector2 mapOrigin = new Vector2(0.5F, 0.5F);

        // The size of the action stage map in tile units (length, width).
        private Vector2 mapSize = new Vector2(24, 24);

        // Start is called before the first frame update
        void Start()
        {
            // Gets the action manager.
            if (actionManager == null)
                actionManager = ActionManager.Instance;

            // Gets the instance.
            // NOTE: this uses instance for ease of access, but it should NOT be set this way.
            // This is because the prefabs aren't set if you aren't using a pre-built object.
            if (actionTilePrefabs == null)
                actionTilePrefabs = ActionTilePrefabs.Instance;

            // Gets the instance.
            if(actionStageList == null)
                actionStageList = ActionStageList.Instance;

            // If the map should be generated in the Start() function.
            if(generateMapOnStart)
            {
                GenerateMap();
            }
        }

        // Gets the tile size.
        public Vector2 TileSize
        {
            get { return tileSize; }
        }

        // Gets the map size.
        public Vector2 MapSize
        {
            get { return mapSize; }
        }

        // Gets the id number, which is taken from the action manager.
        public int GetIdNumber()
        {
            return actionManager.idNumber;
        }

        // Generates a map using the id number from the action manager.
        public void GenerateMap()
        {
            // Generates the map using the id number from the action manager.
            GenerateMap(actionManager.idNumber);
        }

        // Generates a map using the provided ID number. Saves the provided as the ID number.
        public void GenerateMap(int idNumber)
        {
            // If there are tiles.
            if (tiles.Count > 0)
            {
                // Goes through all the tiles.
                foreach (ActionTile tile in tiles)
                {
                    // If the tile exists, destroy it.
                    if (tile != null)
                    {
                        Destroy(tile.gameObject);
                    }
                }

                // TODO: destroy the assets on the tiles.
            }

            // Clear the list of tiles.
            tiles.Clear();

            // TODO: create the tiles.
            string[,] map = ActionStageList.GenerateStageMapDebug();

            // The map doesn't exist.
            if(map == null)
            {
                Debug.LogError("No map data was fine.");
                return;
            }

            // Goes through all the rows and columns, generating the tiles.
            // Row
            for(int r = 0; r < map.GetLength(0); r++)
            {
                // Column
                for(int c = 0; c < map.GetLength(1); c++)
                {
                    // The tile id from the map.
                    // Format: ##A (Number-Number-Letter).
                    string tileId = map[r, c];

                    // The tile number, which is the first 2 digits.
                    int tileNumber = int.Parse(tileId.Substring(0, 2));

                    // The tile verison.
                    // If it's the right length (3), the last character is used as the version.
                    // If the length is wrong, it defaults to the first tile version (A).
                    // This accounts for cases where there's only the two number digits (00), though this should never happen.
                    char tileVersion = tileId.Length == 3 ? tileId[2] : 'A';

                    // Generates the new tile.
                    ActionTile newTile = actionTilePrefabs.InstantiatePrefab(tileNumber);

                    // If the new tile doesn't exist, go to the next tile.
                    // Tile 0 is a blank tile.
                    if (newTile == null)
                        continue;

                    // Applies the parent.
                    if(tileParent != null)
                        newTile.transform.parent = tileParent.transform;

                    // Sets the map position of the new tile.
                    newTile.mapPos.x = c;
                    newTile.mapPos.y = r;

                    // Sets the new local position.
                    Vector3 newLocalPos = ConvertMapPositionToWorldUnits(c, r);
                    newTile.transform.localPosition = newLocalPos;

                    // Add the tile to the list.
                    tiles.Add(newTile);
                }
            }
        }

        // Converts the provided map tile position to local position in world units.
        // Argument "mapPos" is the tile position in the map.
        public Vector2 ConvertMapPositionToWorldUnits(Vector2 mapPos)
        {
            // NOTE: the term "local" refers to the local position of the tile in world units.
            // So say, if the tile is a child of another object, the result is the local position of that tile...
            // In reference to its parent.

            // The map size in pixels.
            Vector2 mapPixelSize = mapSize * tileSize;

            // The map origin position offset.
            // The origin(0.5, 0.5) is considered the centre, so a calculation is done to offset it.
            // e.g., if the origin was (0.4, 0.4), the calculation would be (0.4, 0.4) - (0.5, 0.5) = (-0.1, -0.1).
            // This would result in tiles shifting over by 10% to the bottom left.
            Vector2 mapOriginPosOffset = mapOrigin - new Vector2(0.5F, 0.5F);

            // Calculates the map origin position given the map size.
            // It applies the origin position offset.
            Vector2 mapOriginPosLocal = mapPixelSize * (mapOrigin + mapOriginPosOffset);

            // The map position is a percentage.
            Vector2 mapPosPercent = mapPos / mapSize;

            // The map position in the local position.
            Vector2 mapPosLocal = mapPixelSize * mapPosPercent;

            // Adjusts the map position in the world based on the map's origin in the world.
            mapPosLocal -= mapOriginPosLocal;

            // Returns the map position in the world.
            return mapPosLocal;
        }

        // Convert the map position to world units.
        public Vector2 ConvertMapPositionToWorldUnits(float mapPosX, float mapPosY)
        {
            // Convert to vector 2 to use other function.
            return ConvertMapPositionToWorldUnits(new Vector2(mapPosX, mapPosY));
        }

        // Gets the stage map data. Returns null if there's no data.
        public string[,] GetStageMapData()
        {
            // Gets the id number.
            int idNumber = GetIdNumber();

            // The map to be returned.
            string[,] map = null;

            // Generates the map.
            switch(idNumber)
            {
                default:
                case 0:
                    map = ActionStageList.GenerateStageMapDebug();
                    break;
            }

            return map;
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}