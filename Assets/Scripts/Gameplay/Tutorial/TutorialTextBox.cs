using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_EDU
{
    // The tutorial text box.
    public class TutorialTextBox : TextBox
    {
        [Header("Tutorials")]
        // The UI for the tutorial.
        public TutorialUI tutorialUI;

        // The display object.
        public GameObject displayObject;

        // The display image.
        public Image displayImage;

        [Header("Tutorials/Sprites")]
        // A transparent sprite (alpha = 0).
        public Sprite alpha0Sprite;
        
        // The two stage types: action and knowledge
        public Sprite stageTypesSprite;

        // The log and options buttons
        public Sprite logOptionsButtonsSprite;
        public Sprite logButtonSprite;
        public Sprite optionsButtonSprite;


        [Header("Tutorials/Sprites/Natural Resources")]
        // Display sprites for the natural resources.
        public Sprite biomassSprite;
        public Sprite geothermalSprite;
        public Sprite hydroSprite;
        public Sprite solarSprite;
        public Sprite waveSprite;
        public Sprite windSprite;

        public Sprite coalSprite;
        public Sprite naturalGasSprite;
        public Sprite nuclearSprite;
        public Sprite oilSprite;

        [Header("Tutorials/Sprites/Action")]

        // Action enemy player bar
        public Sprite actionEnemyEnergyBarSprite;

        // Action unit buttons
        public Sprite actionUnitButtonsSprite;
        public Sprite actionUnitSelectorArrowsSprite;
        public Sprite actionUnitSelectedSprite;

        // Defense types
        public Sprite actionDefenseTypesSprite;
        public Sprite actionDefenseLaneBlasterSprite;

        // Indicators
        public Sprite dayNightIndicatorSprite;
        public Sprite energyAirPollutionDisplaySprite;
        public Sprite windIndicatorSprite;

        // The action buttons: speed, deselect, remove, and energy block.
        public Sprite actionButtonsSprite;
        public Sprite stageSpeedSprite;
        public Sprite unitDeselectSprite;
        public Sprite unitRemoveSprite;
        public Sprite energyBlockSprite;

        [Header("Tutorials/Sprites/Knowledge")]

        public Sprite knowledgeStatementsResourcesSprite;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the tutorial UI isn't set, get the instance.
            if (tutorialUI == null)
                tutorialUI = TutorialUI.Instance;

            // Adds a callback for when the text box has been closed.
            OnTextBoxClosedAddCallback(OnTextBoxClosed);
        }

        // Returns 'true' if the display is active.
        public bool IsDisplayActive()
        {
            return displayObject.activeSelf;
        }

        // Sets the display object as active. 
        public void SetDisplayActive(bool active)
        {
            displayObject.SetActive(active);
        }

        // Refreshes display active.
        // If there is no sprite for the display image, the object is disabled.
        public void RefreshDisplayActive()
        {
            // If there's a sprite and the display is inactive, activate the display.
            if(displayImage.sprite != null && !displayObject.activeSelf)
            {
                SetDisplayActive(true);
            }
            // If there's no sprite and the display is active, hide the display.
            else if(displayImage.sprite == null && displayObject.activeSelf)
            {
                SetDisplayActive(false);
            }
        }
        
        // Sets the display image sprite.
        public void SetDisplayImageSprite(Sprite newSprite)
        {
            displayImage.sprite = newSprite;
        }

        // Clears the display image sprite.
        public void ClearDisplayImageSprite()
        {
            displayImage.sprite = null;
        }

        // Called when the page has changed.
        public override void OnPageChanged(Page newPage, int newPageIndex)
        {
            // The page has changed.
            base.OnPageChanged(newPage, newPageIndex);

            // If the new page is an EDU page.
            if(newPage is EDU_Page)
            {
                // The converted page.
                EDU_Page eduPage = (EDU_Page)newPage;

                // Saves the sprite to the display image if it should be changed.
                if(displayImage.sprite != eduPage.displaySprite)
                {
                    displayImage.sprite = eduPage.displaySprite;
                }
            }

            // Refreshes the display so that its shown if there's a sprite...
            // And hidden if there's not a sprite.
            RefreshDisplayActive();
        }

        // Called when the text box has been closed.
        protected void OnTextBoxClosed()
        {
            // Clear the diagram image and refresh the diagram object.
            ClearDisplayImageSprite();
            RefreshDisplayActive();
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}