using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // The tutorial text box.
    public class TutorialTextBox : TextBox
    {
        [Header("Tutorials")]
        // The UI for the tutorial.
        public TutorialUI tutorialUI;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the tutorial UI isn't set, get the instance.
            if (tutorialUI == null)
                tutorialUI = TutorialUI.Instance;
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}