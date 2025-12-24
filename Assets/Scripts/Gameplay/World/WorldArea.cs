using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // An area in the world.
    public class WorldArea : MonoBehaviour
    {
        // The world manager.
        public WorldManager worldManager;

        // A game object used for positioning the camera.
        // This world position is used to change the camera's position.
        public GameObject cameraPos;

        // The sprite for the area.
        public SpriteRenderer background;

        // The stages in the area.
        public List<WorldStage> stages = new List<WorldStage>();

        // The area complete event.
        public AreaCompleteEvent areaCompleteEvent;

        // Start is called before the first frame update
        void Start()
        {
            // If the world manager isn't set, grab the instance.
            if(worldManager == null)
            {
                worldManager = WorldManager.Instance;
            }

            // If not set, try to get component.
            if (areaCompleteEvent == null)
                areaCompleteEvent = GetComponent<AreaCompleteEvent>();

            // Finds all the stage children and puts them into the list if the list is empty.
            if (stages.Count <= 0)
            {
                stages.Clear();
                stages.AddRange(GetComponentsInChildren<WorldStage>());
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}