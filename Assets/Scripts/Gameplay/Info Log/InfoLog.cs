using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Stores and shows information relevant to the game.
    public class InfoLog : MonoBehaviour
    {
        // The data logger, which is used to reference what information to show.
        public DataLogger dataLogger;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the instance.
            if (dataLogger == null)
                dataLogger = DataLogger.Instance;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}