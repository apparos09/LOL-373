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
            // The generator prefab, resource, and its diagram sprite.
            public ActionUnitGenerator generatorPrefab;
            public NaturalResources.naturalResource resource;
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

            // The notes and notes keys for the generator.
            public List<string> notes;
            public List<string> notesKeys;

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
                newInfo.resource = generator.resource;
                newInfo.diagramSprite = TutorialUI.Instance.textBox.GetNaturalResourceDiagramSprite(newInfo.resource);

                // Name, energy cost, energy gen amount, and energy gen speed.
                newInfo.name = generator.GetUnitNameTranslated();
                newInfo.energyCost = generator.energyCreationCost;
                newInfo.energyGenAmount = generator.energyGenerationAmount;
                newInfo.energyGenSpeed = generator.energyGenerationSpeed;

                // Tiles
                newInfo.validTiles = new List<ActionTile.actionTile>(generator.validTiles);

                // Notes.
                newInfo.notes = generator.GetGeneratorNotesTranslated();
                newInfo.notesKeys = generator.GenerateNotesKeys();

                // Returns the new info.
                return newInfo;
            }
        }

        // The action UI.
        public ActionUI actionUI;

        // The generator infos.
        public List<GeneratorInfo> generatorInfos = new List<GeneratorInfo>();

        // The current generator info index.
        protected int generatorInfoIndex = 0;

        // Loads up the generator infos on start.
        public bool loadGeneratorInfosOnStart = true;

        // If 'true', the tutorial text is used for the generation info notes...
        // Instead of what's actually listed.
        private bool useTutorialsForGenInfoNotes = true;

        // Set to 'true' when generator infos have been loaded.
        private bool generatorInfosLoaded = false;

        // If 'true', all dialog boxes are closed when this dialog box is closed.
        // If 'false', only this dialog box is closed.
        [Tooltip("Closes all dialog boxes when this one is closed if true. If false, only close this dialog box.")]
        public bool closeAllDialogsOnClose = false;

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
                // Generates the generator info.
                GeneratorInfo genInfo = GeneratorInfo.GenerateGeneratorInfo(playerUser.generatorPrefabs[i]);

                // If the tutorial text should be used instead of the built-in notes.
                if(useTutorialsForGenInfoNotes)
                {
                    // Gets the tutorial info.
                    Tutorials.TutorialInfo tutorialInfo = Tutorials.Instance.GetNaturalResourceTutorialInfo(genInfo.resource);
                    
                    // Clears the notes and keys.
                    genInfo.notes.Clear();
                    genInfo.notesKeys.Clear();

                    // Goes throguh all pages.
                    foreach (Page page in tutorialInfo.pages)
                    {
                        // Convert the page.
                        EDU_Page eduPage = page as EDU_Page;

                        // Add the text as the note, and the text language key as the note key.
                        genInfo.notes.Add(eduPage.text);
                        genInfo.notesKeys.Add(eduPage.textLanguageKey);
                    }
                }

                // Adds the generator info.
                generatorInfos.Add(genInfo);
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
            resourceNameText.text = genInfo.name;
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
            switch (genInfo.resource)
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

            // There are notes to display.
            if(currInfo.notes.Count > 0)
            {
                // Gets the notes page index clamped.
                notesPageIndex = Mathf.Clamp(index, 0, currInfo.notes.Count - 1);

                // Sets the notes text and updates the page.
                notesText.text = currInfo.notes[index];
                notesPageText.text = (index + 1).ToString() + "/" + currInfo.notes.Count.ToString();
            }
            // No notes, so clear the page.
            else
            {
                ClearNotesPage();
            }
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
            // If 'true', close all dialogs.
            if(closeAllDialogsOnClose)
            {
                actionUI.CloseAllDialogs();

                // Gets the action manager and aciton audio.
                ActionManager actionManager = ActionManager.Instance;
                ActionAudio actionAudio = ActionAudio.Instance;

                // If all dialogs should be closed, that means this was called at the start of a stage.
                // As such, check if the stage start dialog is being used. If so, and the stage isn't playing...
                // Open the stage start dialog back up.
                if (actionManager.IsUsingStageStartDialog() && !actionManager.IsStagePlaying())
                {
                    actionUI.OpenStageStartDialog();
                }
            }
            // False, so only close this dialog.
            else
            {
                actionUI.CloseGeneratorInfoDialog();
            }
        }

    }
}
