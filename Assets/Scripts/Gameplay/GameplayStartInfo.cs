using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Holds the information needed to start a given area of the game (world, action, knowledge).
    public abstract class GameplayStartInfo : MonoBehaviour
    {
        // The resources in the knowledge stage.
        public List<KnowledgeResource> resources = new List<KnowledgeResource>();

        // Awake is called when the script is being loaded
        void Awake()
        {
            // ...
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Applies the start info to the provided manager.
        public abstract void ApplyStartInfo(GameplayManager manager);
        

    }
}