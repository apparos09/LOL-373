using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action tile prefabs.
    public class ActionTilePrefabs : MonoBehaviour
    {
        // The singleton instance.
        private static ActionTilePrefabs instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The action tile prefabs.
        // The tile at index 0 is blank since it's meant to be an empty tile.
        public List<ActionTile> tilePrefabs = new List<ActionTile>();

        // Sprites used for tile overlays. The id numbers match up with the sprites in the list.
        public List<Sprite> tileOverlaySprites = new List<Sprite>();

        // Constructor
        private ActionTilePrefabs()
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

        // Gets the instance.
        public static ActionTilePrefabs Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<ActionTilePrefabs>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Action Tile Prefabs (singleton)");
                        instance = go.AddComponent<ActionTilePrefabs>();
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

        // Gets a prefab. This does NOT instantiate the prefab.
        public ActionTile GetActionTilePrefab(int index)
        {
            // Index validity check.
            if(index >= 0 && index < tilePrefabs.Count)
            {
                return tilePrefabs[index];
            }
            else
            {
                return null;
            }
        }

        // Gets the tile by the tile type.
        public ActionTile GetActionTilePrefabByTileType(ActionTile.actionTile type)
        {
            return GetActionTilePrefab((int)type);
        }

        // Instantiates and returns a prefab.
        public ActionTile InstantiateActionTilePrefab(int index)
        {
            // The prefab and the new tile.
            ActionTile prefab = GetActionTilePrefab(index);
            ActionTile newTile = null;

            // If the prefab isn't equal to null, create an instance.
            if (prefab != null)
            {
                newTile = Instantiate(prefab);
            }

            // Returns the new tile.
            return newTile;
        }

        // Instantiates the tile by the tile type.
        public ActionTile InstantiateActionTilePrefabByTileType(ActionTile.actionTile type)
        {
            return InstantiateActionTilePrefab((int)type);
        }

        // Gets the overlay sprite based on the index.
        public Sprite GetActionTileOverlaySprite(int index)
        {
            // Gets the overlay sprite based on the provided index.
            if (index >= 0 && index < tileOverlaySprites.Count)
            {
                return tileOverlaySprites[index];
            }
            else
            {
                return null;
            }
        }

        // Gets the overlay sprite based on the provided type.
        // The type number should match up with the index.
        public Sprite GetActionTileOverlaySprite(ActionTile.actionTileOverlay overlayType)
        {
            return GetActionTileOverlaySprite((int)overlayType);
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