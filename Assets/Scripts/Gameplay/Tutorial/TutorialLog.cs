using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The tutorial log, which allows the player to view previous tutorials.
    public class TutorialLog : MonoBehaviour
    {
        // The gameplay UI.
        public GameplayUI gameplayUI;

        [Header("Tutorial Info")]

        // The tutorial title.
        public TMP_Text tutorialTitle;

        // The tutorial image.
        public Image tutorialImage;

        // The tutorial text.
        public TMP_Text tutorialText;

        // The tutorial page index.
        protected int tutorialPageIndex = 0;

        // The previous tutorial text page.
        public Button prevTutorialTextPage;

        // The next tutorial text page.
        public Button nextTutorialTextPage;

        // The tutorial page text.
        public TMP_Text tutorialPageText;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}