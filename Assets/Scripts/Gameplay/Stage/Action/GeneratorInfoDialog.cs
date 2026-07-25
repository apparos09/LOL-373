using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The generator info dialog, which provides information on an action unit generator.
    public class GeneratorInfoDialog : MonoBehaviour
    {
        // // The generator info struct.
        // public struct GeneratorInfo
        // {
        // 
        // }

        // The gameplay UI.
        public GameplayUI gameUI;

        // Start is called before the first frame update
        void Start()
        {
            // Finds the gameplay UI if it's not set.
            if(gameUI == null)
                gameUI = FindObjectOfType<GameplayUI>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
