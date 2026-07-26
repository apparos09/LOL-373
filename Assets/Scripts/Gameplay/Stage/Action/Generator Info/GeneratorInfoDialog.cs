using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_EDU
{
    // The generator info dialog, which provides information on an action unit generator.
    public class GeneratorInfoDialog : MonoBehaviour
    {
        // The generator info struct.
        public class GeneratorInfo
        {
            // The generator prefab and its diagram sprite.
            public ActionUnitGenerator generatorPrefab;
            public Sprite diagramSprite;

            // The notes for the generator.
            public List<string> notes;
        }

        // The action UI.
        public ActionUI actionUI;

        // The generator infos.
        public List<GeneratorInfo> generatorInfos;

        // The current generator info index.
        protected int currGeneratorInfoIndex = 0;

        [Header("Diagram")]

        // The diagram image.
        public Image diagramImage;

        // The sprite for when the diagram displays nothing.
        public Sprite diagramNoneSprite;

        [Header("Stats")]

        // The resource text.
        public TMP_Text resourceNameText;

        // The energy cost value.
        public TMP_LabeledValue energyCostValue;

        // The energy generation amount progess bar.
        public ProgressBar energyGenAmountBar;

        // The energy generation speed progerss bar.
        public ProgressBar energyGenSpeedBar;

        [Header("Stats/Tiles")]

        // The land options toggle.
        public TileOptionToggle landToggle;

        // The river options toggle.
        public TileOptionToggle riverToggle;

        // The sea options toggle.
        public TileOptionToggle seaToggle;

        // The symbol options toggle.
        public TileOptionToggle symbolToggle;

        [Header("Stats/Notes")]

        // The notes text.
        public TMP_Text notesText;

        // The previous notes page button text.
        public Button prevNotesPageButton;

        // The next notes page button text.
        public Button nextNotesPageButton;

        // The notes page text.
        public TMP_Text notesPageText;

        [Header("Generator Selector")]

        // The previous generator page button text.
        public Button prevGeneratorPageButton;

        // The next generator page button text.
        public Button nextGeneratorPageButton;

        // The generator text.
        public TMP_Text generatorPageText;

        // Start is called before the first frame update
        void Start()
        {
            // Finds the action UI if it's not set.
            if(actionUI == null)
                actionUI = ActionUI.Instance;
        }

        // The current generator info. Returns null if index is invalid.
        public GeneratorInfo GetCurrentGeneratorInfo()
        {
            // Checks for index validity.
            if (currGeneratorInfoIndex >= 0 && currGeneratorInfoIndex < generatorInfos.Count)
            {
                return generatorInfos[currGeneratorInfoIndex];
            }
            // None.
            else
            {
                return null;
            }
        }

        // Clears the generator info UI
        public void ClearGeneratorInfoUI()
        {
            // Clears the diagram.
            diagramImage.sprite = diagramNoneSprite;

            // Clears te energy cost value.
            energyCostValue.valueText.text = "-";

            // Clears the progess bars.
            energyGenAmountBar.SetValue(0, false);
            energyGenSpeedBar.SetValue(0, false);

            // Clears the tiles.
            landToggle.toggle.isOn = false;
            riverToggle.toggle.isOn = false;
            seaToggle.toggle.isOn = false;
            symbolToggle.toggle.isOn = false;

            // Clears the notes.
            notesText.text = "-";
            // TODO: resets the notes page.

            // TODO: Resets the generator page.
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
