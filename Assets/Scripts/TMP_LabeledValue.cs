using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RM_EDU
{
    // A labeled value.
    public class TMP_LabeledValue : MonoBehaviour
    {
        // The header text.
        public TMP_Text headerText;

        // The value text.
        public TMP_Text valueText;
        
        // Gets the header text string.
        public string GetHeaderTextString()
        {
            return headerText.text;
        }

        // Gets the value text string.
        public string GetValueTextString()
        {
            return valueText.text;
        }

        // Clears the text.
        public void ClearTextStrings()
        {
            headerText.text = string.Empty;
            valueText.text = string.Empty;
        }

    }
}