using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // The tutorial text box.
    public class TutorialTextBox : TextBox
    {
        // The UI for the tutorial.
        public TutorialUI tutorialUI;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the tutorial UI isn't set, try to find it.
            if(tutorialUI == null)
                tutorialUI = FindObjectOfType<TutorialUI>();
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}