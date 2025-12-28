using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RM_EDU
{
    // The action unit card.
    public class ActionUnitCard : MonoBehaviour
    {
        // The card background.
        public Image unitCardBackground;

        // The unit name text.
        public TMP_Text unitNameText;

        // The icon image.
        public Image unitIconImage;

        // The unit energy cost text.
        public TMP_Text energyCostText;

        // Awake is called when the script instance is being loaded.
        private void Awake()
        {
            // Clears the action unit info.
            ClearActionUnitInfo();
        }

        // // Start is called before the first frame update
        // void Start()
        // {
        //     // ...
        // }

        // Applies information from an action unit button.
        public void ApplyActionUnitButtonInfo(ActionUnitButton unitButton)
        {
            unitCardBackground.sprite = unitButton.button.image.sprite;
            unitNameText.text = unitButton.unitNameText.text;
            unitIconImage.sprite = unitButton.unitIconImage.sprite;
            energyCostText.text = unitButton.energyCostText.text;
        }

        // Clears the information displayed for the icon.
        public void ClearActionUnitInfo()
        {
            unitCardBackground.sprite = null;
            unitNameText.text = "";
            unitIconImage.sprite = null;
            energyCostText.text = "";
        }
    }
}