using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static RM_EDU.ActionUnit;

namespace RM_EDU
{
    // Indicates the strength of the wind.
    public class WindIndicator : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // NOTE: this is a temporary display.

        // The image.
        public Image image;

        // These colours are used as placeholders.
        private Color noWindColor = Color.blue;
        private Color maxWindColor = Color.red;

        // If the indicator is enabled, update it every frame.
        public bool indicatorEnabled = true;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the action UI.
            if (actionUI == null)
                actionUI = ActionUI.Instance;
        }

        
        // Update is called once per frame
        void Update()
        {
            // If the indicator is enabled.
            if (indicatorEnabled)
            {
                // Gets the instance.
                ActionManager actionManager = ActionManager.Instance;

                // If the wind is enabled.
                if (actionManager.IsWindEnabled())
                {
                    // Gets the wind rating as a percentage.
                    float t = actionManager.GetCurrentWindRatingAsAPercentage();

                    // Lerps between the two colours.
                    image.color = Color.Lerp(noWindColor, maxWindColor, t);
                }
            }
        }
    }
}