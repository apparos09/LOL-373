using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

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

        // The action tile array, which also determines the map size.
        // (r, w) = (y, x)
        public ActionTile[,] tiles = new ActionTile[7, 16];

        // The origin of the tile. By default, it's the middle of the tile.
        private Vector2 tileOrigin = new Vector2(0.5F, 0.5F);

        // The tile sprite size in pixels (length, width)
        private Vector2 tileSize = new Vector2(1.28F, 1.28F);

        // The original of the map.
        // (0.5, 0.5) is the centre, (1, 1) is the top right corner and (0, 0) is the bottom left corner.
        protected Vector2 mapOrigin = new Vector2(0.5F, 0.5F);

        // Start is called before the first frame update
        void Start()
        {
            // Gets the action manager.
            if (actionManager == null)
                actionManager = ActionManager.Instance;

            // If the action stage isn't set, set it to this.
            if (actionManager.actionStage == null)
                actionManager.actionStage = this;

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
            get 
            {
                // return mapSize; 
                // (row, col) = (y, x)
                return new Vector2(tiles.GetLength(1), tiles.GetLength(0));
            }
        }

        // Gets the maximum amount of tiles in the map.
        public int MapTileCountMax
        {
            get { return tiles.Length; }
        }

        // Gets the map's row count.
        public int MapRowCount
        {
            get { return tiles.GetLength(0); }
        }

        // Get's the map's column count.
        public int MapColumnCount
        {
            get { return tiles.GetLength(1); }
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
            // Gets the map based on the id number.
            string[,] map = ActionStageList.GenerateStageMap(idNumber);

            // The map doesn't exist.
            if(map == null)
            {
                Debug.LogError("No map data was found.");
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
                    char tileVersion = tileId.Length == 3 ? char.ToUpper(tileId[2]) : 'A';

                    // The tile prefab and the new tile.
                    ActionTile tilePrefab = actionTilePrefabs.GetPrefab(tileNumber);
                    ActionTile newTile = null;

                    // If the tile prefab exists, create the tile.
                    if (tilePrefab != null)
                        newTile = Instantiate(tilePrefab, transform);

                    // If the new tile doesn't exist, go to the next tile.
                    // Tile 0 is an empty space.
                    if (newTile == null)
                    {
                        // Destroys the tile at the index if it eixsts.
                        if (tiles[r, c] != null)
                        {
                            Destroy(tiles[r, c].gameObject);
                            tiles[r, c] = null;
                        }

                        // Next.
                        continue;
                    }    

                    // Applies the parent.
                    if(tileParent != null)
                        newTile.transform.parent = tileParent.transform;

                    // Sets the tile version.
                    newTile.tileVersion = tileVersion;

                    // Sets the map position of the new tile.
                    // Row = Y, Col = X
                    newTile.mapPos.x = c;
                    newTile.mapPos.y = r;

                    // Sets the new local position.
                    Vector3 newLocalPos = ConvertMapPositionToWorldUnits(c, r);
                    newTile.transform.localPosition = newLocalPos;

                    // The tile colour.
                    Color tileColor = Color.white;

                    // Sets the tile's colour, which is used to make a checkerboardp attern.
                    // The bottom left corner (0, 0) of the map is black (darkenend tile).
                    if(r % 2 == 0) // Black
                    {
                        // 0 = black, 1 = white
                        tileColor = c % 2 == 0 ? ActionTile.DarkenedColor : ActionTile.NormalColor;
                    }
                    else // White
                    {
                        // 0 = white, 1 = black
                        tileColor = c % 2 == 0 ? ActionTile.NormalColor: ActionTile.DarkenedColor;
                    }

                    // Sets the tile colour.
                    newTile.baseSpriteRenderer.color = tileColor;

                    // If there's already a tile in this array index, destroy it so it can be replaced.
                    if (tiles[r, c] != null)
                    {
                        Destroy(tiles[r, c].gameObject);
                        tiles[r, c] = null; 
                    }

                    // Add the tile to the array.
                    tiles[r, c] = newTile; // New
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

            // The map size.
            Vector2 mapSize = MapSize;

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

            // The origin of the tile in pixels. This is the tile origin based on the size of the tile in pixels.
            Vector2 tilePixelOrigin = tileSize * tileOrigin;

            // Adjust the tile's world position by its tile origin.
            // The base calculation assumes the tile origin is its bottom-left corner.
            mapPosLocal += tilePixelOrigin;

            // Returns the map position in the world.
            return mapPosLocal;
        }

        // Convert the map position to world units.
        public Vector2 ConvertMapPositionToWorldUnits(float mapPosX, float mapPosY)
        {
            // Convert to vector 2 to use other function.
            return ConvertMapPositionToWorldUnits(new Vector2(mapPosX, mapPosY));
        }

        // Gets the map lower bound in world units.
        // If 'useTileCorner' is true, the lower bounds is the edge of the tile.
        // If 'useTileCorner' is false, it goes by the tile position, which is the tile's centre (origin).
        public Vector2 GetMapLowerBoundsInWorldUnits(bool useTileCorner = true)
        {
            // Result.
            Vector2 result = ConvertMapPositionToWorldUnits(0, 0);

            // If using the tile corner, reduce it by the tile size / 2.
            // It's /2 because the tile origin is the tile's centre.
            if (useTileCorner)
                result -= TileSize / 2.0F;

            return result;
        }

        // Gets the map upper bound in world units.
        // If 'useTileCorner' is true, the upper bounds is the edge of the tile.
        // If 'useTileCorner' is false, it goes by the tile position, which is the tile's centre (origin).
        public Vector2 GetMapUpperBoundsInWorldUnits(bool useTileCorner = true)
        {
            // Result
            Vector2 result = ConvertMapPositionToWorldUnits(MapSize);

            // If using the tile corner, increase it by the tile size / 2.
            // It's /2 because the tile origin is the tile's centre.
            if (useTileCorner)
                result += TileSize / 2.0F;

            return result;
        }

        // Returns 'true' if the map contains the provided tile and the tile's saved mapPos.
        // If it doesn't, or if the tile isn't in the array, it returns false.
        public bool MapContainsTileAtIndex(ActionTile tile)
        {
            // The tile's row and column positions
            int tileRow = tile.GetMapRowPosition();
            int tileCol = tile.GetMapColumnPosition();

            // The result.
            bool result;

            // If the row and column values are valid.
            if (tileRow >= 0 && tileRow < tiles.GetLength(0) && tileCol >= 0 && tileCol < tiles.GetLength(1))
            {
                // If the tile is saved at the index, the check is valid.
                if (tiles[tileRow, tileCol] == tile)
                {
                    result = true;
                }
                else // Not in space, so check is false.
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        // Calculates the color of the tile at a given index.
        public Color CalculateTileColorInMap(int row, int col)
        {
            // The row and column counts.
            int rowCount = MapRowCount;
            int colCount = MapColumnCount;

            // The colour that will be returned.
            Color color;

            // If the index is valid, just send the color white.
            if (row >= 0 && row < rowCount && col >= 0 && col < colCount)
            {
                // Determines if the normal colour should be returned.
                // The "white" color is just the normal color.
                // The "black" color is just the darkened color.
                bool useNormal;

                // The bottom left corner (0, 0) is black.
                // 0 = black, 1 = white.
                if (row % 2 == 0) // Black
                {
                    // 0 = black, 1 = white
                    useNormal = col % 2 == 0 ? false : true;
                }
                else // White
                {
                    // 0 = white, 1 = black
                    useNormal = col % 2 == 0 ? true : false;
                }

                // Gets the color.
                color = useNormal ? ActionTile.NormalColor : ActionTile.DarkenedColor;
            }
            else
            {
                // Invalid index, so just return the colour white.
                Debug.LogAssertion("Index invalid. Returning Color.white.");
                color = Color.white;
            }

            // Returns the color.
            return color;
        }

        // Gets a vector representing the provided world position in reference to the map.
        // If a value for an axis is (0), it means the position is within the stage.
        // If it's a 1, it's outside the stage on the positive end of that axis.
        // If it's a -1, it's outisde the stage on the negative end of that axis.
        public Vector3 GetMapWorldPositionReferenceVector(Vector3 worldPos)
        {
            // The map bounds.
            Vector3 mapBoundsLower = GetMapLowerBoundsInWorldUnits();
            Vector3 mapBoundsUpper = GetMapUpperBoundsInWorldUnits();

            // The reference vector.
            Vector3 refVec = new Vector3(
                worldPos.x < mapBoundsLower.x ? -1 : worldPos.x > mapBoundsUpper.x ? 1 : 0,
                worldPos.y < mapBoundsLower.y ? -1 : worldPos.y > mapBoundsUpper.y ? 1 : 0,
                worldPos.z < mapBoundsLower.z ? -1 : worldPos.z > mapBoundsUpper.z ? 1 : 0
                );

            // Returns the vector.
            return refVec;
        }

        // Gets the stage map data. Returns null if there's no data.
        public string[,] GetStageMapData()
        {
            // Gets the id number.
            int idNumber = GetIdNumber();

            // The map to be returned.
            string[,] map = ActionStageList.GenerateStageMap(idNumber);

            // Return map.
            return map;
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}