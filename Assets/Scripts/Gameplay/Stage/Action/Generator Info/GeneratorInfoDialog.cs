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

        // The action UI.
        public ActionUI actionUI;

        // Start is called before the first frame update
        void Start()
        {
            // Finds the action UI if it's not set.
            if(actionUI == null)
                actionUI = ActionUI.Instance;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
