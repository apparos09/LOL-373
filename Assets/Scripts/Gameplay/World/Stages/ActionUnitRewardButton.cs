using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The action unit reward button.
    public class ActionUnitRewardButton : MonoBehaviour
    {
        // The world UI.
        public WorldUI worldUI;

        // The rewards dialog.
        public WorldStageRewardsDialog rewardsDialog;

        [Header("Button, Info")]
        // The button.
        public Button button;

        // The default button image sprite.
        public Sprite buttonDefaultImageSprite = null;

        // The button image.
        public Image buttonImage;

        // The unit name text.
        public TMP_Text unitNameText;

        // The sprite for having an alpha value of 0.
        public Sprite alpha0Sprite;

        // The icon image.
        public Image unitIconImage;

        // The unit energy cost text.
        public TMP_Text energyCostText;

        // The unit prefab.
        public ActionUnit unitPrefab;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the world UI.
            if (worldUI == null)
                worldUI = WorldUI.Instance;

            // If the rewards dialog isn't set, grab it in the instance.
            if(rewardsDialog == null)
                rewardsDialog = GetComponentInParent<WorldStageRewardsDialog>();

            // If the button is null, get the component.
            if (button == null)
                button = GetComponent<Button>();

            // Adds a Select() call on the button.
            if (button != null)
            {
                // Sets the default image.
                if (buttonDefaultImageSprite == null)
                    buttonDefaultImageSprite = button.image.sprite;

                // Listener for the tutorial toggle.
                button.onClick.AddListener(delegate
                {
                    Select();
                });
            }
        }

        // Called when the button has been pressed.
        public virtual void Select()
        {
            rewardsDialog.ApplySelectedUnitInfo(this);
        }

        // Refresh the interactability of the button.
        public void RefreshButtonInteractable()
        {
            button.interactable = unitPrefab != null;
        }

        // Returns 'true' if the unit button has a unit prefab.
        public bool HasUnitPrefab()
        {
            return unitPrefab != null;
        }

        // Sets the prefab.
        public void SetPrefab(ActionUnit newPrefab)
        {
            unitPrefab = newPrefab;
            ApplyPrefabInfo();
        }

        // Applies the prefab information.
        public void ApplyPrefabInfo()
        {
            // Checks if the prefab exists.
            if (unitPrefab != null)
            {
                // Icon image, display name, and energy cost.
                button.image.sprite = unitPrefab.cardBackgroundSprite;
                unitIconImage.sprite = unitPrefab.iconSprite;
                unitNameText.text = unitPrefab.GetUnitCardDisplayName();
                energyCostText.text = unitPrefab.energyCreationCost.ToString();

                // Refresh the unit button to set the interactable.
                RefreshButtonInteractable();
            }
            // The prefab is null, so clear the unit button.
            else
            {
                ClearPrefabInfo();
            }
        }

        // Applies the prefab info.
        public void ApplyPrefabInfo(ActionUnit newPrefab)
        {
            unitPrefab = newPrefab;
            ApplyPrefabInfo();
        }

        // Clears the prefab information.
        public void ClearPrefabInfo()
        {
            // Visual information.
            button.image.sprite = buttonDefaultImageSprite;
            unitIconImage.sprite = alpha0Sprite;
            unitNameText.text = "-";
            energyCostText.text = "-";


            // The button is no longer interactable.
            button.interactable = false;

            // Clears prefab.
            unitPrefab = null;
        }
    }
}