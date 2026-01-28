using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The info log entry button.
    public class InfoLogEntryButton : MonoBehaviour
    {
        // The info log this entry button belongs to.
        public InfoLog infoLog;

        // The button this entry is attached to.
        public Button button;

        // The  text for the log entry button.
        public TMP_Text text;

        // Start is called before the first frame update
        void Start()
        {
            // If not set, try to get the info log in the parent.
            if (infoLog == null)
                infoLog = GetComponentInParent<InfoLog>();

            // Gets the button component.
            if(button == null)
                button = GetComponent<Button>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}