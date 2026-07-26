using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
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

            // The name of the generator.
            public string name;

            // The energy cost of the generator.
            public float energyCost;

            // The energy generation amount and speed.
            public float energyGenAmount;
            public float energyGenSpeed;

            // The valid tiles.
            public List<ActionTile.actionTile> validTiles;

            // The notes for the generator.
            public List<string> notes;

            // Generates generator info using the provided resource.
            public static GeneratorInfo GenerateGeneratorInfo(NaturalResources.naturalResource resource)
            {
                // Gets the generator info using the provided resource.
                return GenerateGeneratorInfo(ActionUnitPrefabs.Instance.GetGeneratorPrefabByResource(resource));
            }

            // Generates a generator info object from the provided generator.
            public static GeneratorInfo GenerateGeneratorInfo(ActionUnitGenerator generator)
            {
                // Object
                GeneratorInfo newInfo = new GeneratorInfo();

                // Values
                // Prefab and diagram.
                newInfo.generatorPrefab = generator;
                newInfo.diagramSprite = TutorialUI.Instance.textBox.GetNaturalResourceDiagramSprite(generator.resource);

                // Name, energy cost, energy gen amount, and energy gen speed.
                newInfo.name = generator.GetUnitNameTranslated();
                newInfo.energyCost = generator.energyCreationCost;
                newInfo.energyGenAmount = generator.energyGenerationAmount;
                newInfo.energyGenSpeed = generator.energyGenerationSpeed;

                // Tiles
                newInfo.validTiles = new List<ActionTile.actionTile>(generator.validTiles);

                // Notes.
                newInfo.notes = generator.GetGeneratorNotesTranslated();

                // Returns the new info.
                return newInfo;
            }
        }

        // The action UI.
        public ActionUI actionUI;

        // The generator infos.
        public List<GeneratorInfo> generatorInfos;

        // The current generator info index.
        protected int generatorInfoIndex = 0;

        // Loads up the generator infos on start.
        public bool loadGeneratorInfosOnStart = true;

        // Set to 'true' when generator infos have been loaded.
        private bool generatorInfosLoaded = false;

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

        // The notes page index.
        protected int notesPageIndex = 0;

        // The previous notes page button text.
        public Button prevNotesPageButton;

        // The next notes page button text.
        public Button nextNotesPageButton;

        // The notes page text.
        public TMP_Text notesPageText;

        [Header("Generator Info Selector")]

        // The previous generator page button text.
        public Button prevGeneratorInfoButton;

        // The next generator page button text.
        public Button nextGeneratorInfoButton;

        // The generator text.
        public TMP_Text generatorInfoPageText;

        // Start is called before the first frame update
        void Start()
        {
            // Finds the action UI if it's not set.
            if(actionUI == null)
                actionUI = ActionUI.Instance;

            // If the generator infos should be loaded on start and they haven't been loaded already.
            if(loadGeneratorInfosOnStart && !generatorInfosLoaded)
            {
                LoadGeneratorInfos();
            }
        }

        // This function is called when the object becomes enabled and active.
        private void OnEnable()
        {
            // If the generators haven't been loaded.
            if (!generatorInfosLoaded)
            {
                LoadGeneratorInfos();
            }
        }

        // This function is called when the behaviour becomes disabled and inactive.
        private void OnDisable()
        {
            // Set to the first generator in the list.
            SetCurrentGeneratorInfo(0); 
        }

        // GENERATOR INFO
        // Returns 'true' if the generator infos haven't been loaded.
        public bool IsGeneratorInfosLoaded()
        {
            return generatorInfosLoaded;
        }

        // Loads the generator infos.
        public void LoadGeneratorInfos()
        {
            // Clears the infos.
            ClearGeneratorInfos();

            // Gets the action manager and the player user.
            ActionManager actionManager = ActionManager.Instance;
            ActionPlayerUser playerUser = actionManager.playerUser;
            
            // Goes through all the generator prefabs.
            for(int i = 0; i < playerUser.generatorPrefabs.Count; i++)
            {
                // Generates a generator info from the player user's prefab.
                generatorInfos.Add(GeneratorInfo.GenerateGeneratorInfo(playerUser.generatorPrefabs[i]));
            }

            // Sets the current generator info index.
            SetCurrentGeneratorInfo(0);

            // The generator infos have been loaded.
            generatorInfosLoaded = true;
        }

        // Clears the generator infos.
        public void ClearGeneratorInfos()
        {
            // Clear info.
            generatorInfos.Clear();
            generatorInfoIndex = 0;
            ClearGeneratorInfo();

            // Infos no lonegr loaded.
            generatorInfosLoaded = false;
        }

        // Generates generator info using the provided resource.
        public static GeneratorInfo GenerateGeneratorInfo(NaturalResources.naturalResource resource)
        {
            return GeneratorInfo.GenerateGeneratorInfo(resource);
        }

        // Generates generator info using the provided generator.
        public static GeneratorInfo GenerateGeneratorInfo(ActionUnitGenerator generator)
        {
            return GeneratorInfo.GenerateGeneratorInfo(generator);
        }

        // INFO
        // Gets the generator info count.
        public int GetGeneratorInfoCount()
        {
            return generatorInfos.Count;
        }

        // The current generator info. Returns null if index is invalid.
        public GeneratorInfo GetCurrentGeneratorInfo()
        {
            // Checks for index validity.
            if (generatorInfoIndex >= 0 && generatorInfoIndex < generatorInfos.Count)
            {
                return generatorInfos[generatorInfoIndex];
            }
            // None.
            else
            {
                return null;
            }
        }

        // Sets the current generator info by the provided index.
        public void SetCurrentGeneratorInfo(int index)
        {
            // Gets the index.
            generatorInfoIndex = Mathf.Clamp(generatorInfoIndex, 0, generatorInfos.Count - 1);

            // Sets the info and sets the UI.
            generatorInfoIndex = index;

            // Gets the current info.
            GeneratorInfo genInfo = generatorInfos[index];

            // Sets the sprite.
            diagramImage.sprite = genInfo.diagramSprite;

            // Sets the name and energy cost.
            resourceNameText.text = genInfo.generatorPrefab.name;
            energyCostValue.valueText.text = genInfo.energyCost.ToString();

            // Sets the energy amount and speed.
            energyGenAmountBar.SetValueAsPercentage(genInfo.energyGenAmount / ActionUnit.BASE_STAT_MAXIMUM);
            energyGenSpeedBar.SetValueAsPercentage(genInfo.energyGenSpeed / ActionUnit.BASE_STAT_MAXIMUM);

            // Sets the valid tiles.
            landToggle.toggle.isOn = false;
            riverToggle.toggle.isOn = false;
            seaToggle.toggle.isOn = false;
            symbolToggle.toggle.isOn = false;

            // Goes through all the valid tiles.
            for (int j = 0; j < genInfo.validTiles.Count; j++)
            {
                // Checks valid tile to see what to turn on.
                switch (genInfo.validTiles[j])
                {
                    case ActionTile.actionTile.land:
                        landToggle.toggle.isOn = true;
                        break;

                    case ActionTile.actionTile.river:
                        riverToggle.toggle.isOn = true;
                        break;

                    case ActionTile.actionTile.sea:
                        seaToggle.toggle.isOn = true;
                        break;
                }
            }

            // Detrmines if the symbol toggle is on based on the resource.
            switch (genInfo.generatorPrefab.resource)
            {
                default:
                    symbolToggle.toggle.isOn = false;
                    break;

                case NaturalResources.naturalResource.geothermal:
                case NaturalResources.naturalResource.coal:
                case NaturalResources.naturalResource.naturalGas:
                case NaturalResources.naturalResource.nuclear:
                case NaturalResources.naturalResource.oil:
                    symbolToggle.toggle.isOn = true;
                    break;
            }

            // Notes the notes to page 0.
            SetNotesPageIndex(0);

            // Sets the generator page text.
            generatorInfoPageText.text = (generatorInfoIndex + 1).ToString() + "/" + generatorInfos.Count.ToString();
        }

        // Goes to the previous generator info.
        public void PreviousGeneratorInfo()
        {
            // Info count and index.
            int infoCount = generatorInfos.Count;
            int infoIndex = generatorInfoIndex - 1;

            // Bounds check.
            if (infoIndex < 0)
                infoIndex = infoCount - 1;

            // Set the page.
            SetCurrentGeneratorInfo(infoIndex);
        }

        // Goes to the next generator info.
        public void NextGeneratorInfo()
        {
            // Info count and index.
            int infoCount = generatorInfos.Count;
            int infoIndex = generatorInfoIndex + 1;

            // Bounds check.
            if (infoIndex >= infoCount)
                infoIndex = 0;

            // Set the page.
            SetCurrentGeneratorInfo(infoIndex);
        }

        // Clears the generator info UI
        public void ClearGeneratorInfo()
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

            // Clears the notes and resets the notes page.
            ClearNotesPage();

            // Resets the generator page index and text.
            generatorInfoIndex = 0;
            generatorInfoPageText.text = "-";
        }

        // NOTES
        // Gets the current generator info notes page count.
        public int GetCurrentGeneratorInfoNotesCount()
        {
            GeneratorInfo currInfo = GetCurrentGeneratorInfo();
            return currInfo.notes.Count;
        }

        // Sets the notes page index.
        public void SetNotesPageIndex(int index)
        {
            // Gets the current info.
            GeneratorInfo currInfo = GetCurrentGeneratorInfo();

            // Gets the notes page index clamped.
            notesPageIndex = Mathf.Clamp(index, 0, currInfo.notes.Count - 1);

            // Sets the notes text and updates the page.
            notesText.text = currInfo.notes[index];
            notesPageText.text = (index + 1).ToString() + "/" + currInfo.notes.Count.ToString();
        }

        // Goes to the previous notes page.
        public void PreviousNotesPage()
        {
            // Page count and index.
            int pageCount = GetCurrentGeneratorInfoNotesCount();
            int pageIndex = notesPageIndex - 1;

            // Bounds check.
            if (pageIndex < 0)
                pageIndex = pageCount - 1;

            // Set the page.
            SetNotesPageIndex(pageIndex);
        }

        // Goes to the next notes page.
        public void NextNotesPage()
        {
            // Page count and index.
            int pageCount = GetCurrentGeneratorInfoNotesCount();
            int pageIndex = notesPageIndex + 1;

            // Bounds check.
            if (pageIndex >= pageCount)
                pageIndex = 0;

            // Set the page.
            SetNotesPageIndex(pageIndex);
        }

        // Clears the notes page.
        public void ClearNotesPage()
        {
            notesText.text = "-";
            notesPageIndex = 0;
            notesPageText.text = "-";
        }

        // Closes this dialog.
        public void CloseGeneratorInfoDialog()
        {
            actionUI.CloseGeneratorInfoDialog();
        }

    }
}
