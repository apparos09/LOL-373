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

        // The stages in the area.
        public List<WorldStage> stages = new List<WorldStage>();

        // Start is called before the first frame update
        void Start()
        {
            // If the world manager isn't set, grab the instance.
            if(worldManager == null)
            {
                worldManager = WorldManager.Instance;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}