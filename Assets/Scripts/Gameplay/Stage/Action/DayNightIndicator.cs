using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // A day-night indicator, which is used for a visual that shows the timee of day.
    public class DayNightIndicator : MonoBehaviour
    {
        // The action ui.
        public ActionUI actionUI;

        // The image being used.
        // TODO: update with a proper animation. Right now, the image colour changing is the only indicator.
        public Image image;

        // NOTE: the day and night colors will likely be taken out once a proper graphic is created.

        // The day color.
        private Color dayColor = Color.yellow;

        // The night color.
        private Color nightColor = Color.blue;

        // If the wind indicator should be updated automatically.
        public bool autoUpdateIndicator = true;

        // If the indicator is enabled, update it every frame.
        public bool indicatorEnabled = true;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the instance.
            if (actionUI == null)
                actionUI = ActionUI.Instance;
        }

        // Updates the indicator.
        public void UpdateIndicator()
        {
            // Gets the action manager.
            ActionManager actionManager = ActionManager.Instance;

            // If the stage is progressing, update the indicator.
            if (actionManager.IsStagePlayingAndGameUnpaused())
            {
                // The t-value for going from day to night.
                float t = actionManager.GetDayNightTimerProgress();

                // Lerps between the two colours.
                image.color = Color.Lerp(dayColor, nightColor, t);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // If the indicator is enabled and day night is enabled.
            if (indicatorEnabled && ActionManager.Instance.IsDayNightEnabled())
            {
                // If the indicator should be automatically updated, call the function.
                if (autoUpdateIndicator)
                    UpdateIndicator();
            }
            
        }
    }
}
