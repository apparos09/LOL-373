using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // Stores and shows information relevant to the game.
    public class InfoLog : MonoBehaviour
    {
        // An info log entry.
        public class InfoLogEntry
        {
            // The name of the info entry.
            public string name;

            // The description of the info entry.
            public string description;

            // The icon sprite for the info log entry.
            public Sprite iconSprite;
        }

        // The info log categories.
        public enum infoLogCategory { resources, generators, defenses }

        // The number of categories.
        public const int CATEGORY_COUNT = 3;

        // The gameplay UI.
        public GameplayUI gameUI;

        // The data logger, which is used to reference what information to show.
        public DataLogger dataLogger;

        // The current category.
        protected infoLogCategory currCategory = infoLogCategory.resources;

        // The current entry.
        protected InfoLogEntry currEntry = null;

        // The list of current entries loaded.
        protected List<InfoLogEntry> currEntries = new List<InfoLogEntry>();

        // The current entry index.
        protected int currEntryIndex = 0;

        // Resource entries.
        protected List<InfoLogEntry> resourceEntries = new List<InfoLogEntry>();

        // Generator entries.
        protected List<InfoLogEntry> generatorEntries = new List<InfoLogEntry>();

        // Defense entires.
        protected List<InfoLogEntry> defenseEntries = new List<InfoLogEntry>();

        // Gets set to 'true' if the entry lists have been updated.
        private bool entryListsUpdated = false;

        [Header("Category Section")]

        // The category text.
        public TMP_Text categoryText;

        // The previous category button.
        public Button prevCategoryButton;

        // The next category button.
        public Button nextCategoryButton;

        [Header("Entries Section")]

        // The entry buttons.
        public List<InfoLogEntryButton> entryButtons = new List<InfoLogEntryButton>();

        // The previous entry page button.
        public Button prevEntryPageButton;

        // The next entry page button.
        public Button nextEntryPageButton;

        [Header("Info Section")]

        // The info icon image.
        public Image infoIconImage;

        // The info name text.
        public TMP_Text infoNameText;

        // The info description text.
        public TMP_Text infoDescText;

        // Start is called before the first frame update
        void Start()
        {
            // If the game UI isn't set, set it.
            if (gameUI == null)
                gameUI = FindObjectOfType<GameplayUI>();

            // Gets the instance.
            if (dataLogger == null)
                dataLogger = DataLogger.Instance;

            // Updates the entry info lists.
            if (!entryListsUpdated)
            {
                UpdateEntryInfoLists();
            }
                
        }

        // This function is called when the object becomes enabled and active.
        private void OnEnable()
        {
            // TODO: dynamically update based on changes to the generators or defense ids.
            // The lists haven't been updated, so update them.
            if(!entryListsUpdated)
            {
                UpdateEntryInfoLists();
            }
            
        }

        // This function is called when the behaviour becomes disabled or inactive.
        private void OnDisable()
        {
            // ClearEntryInfoLists();
        }

        // ENTRIES //

        // Updates the entry information.
        public void UpdateEntryInfoLists()
        {
            // The data logger is null, so set the instance.
            if (dataLogger == null)
                dataLogger = DataLogger.Instance;

            // The action unit prefabs.
            ActionUnitPrefabs actionUnitPrefabs = null;

            // If the action unit prefab list is instantiated, get the instance.
            if(ActionUnitPrefabs.Instantiated)
            {
                actionUnitPrefabs = ActionUnitPrefabs.Instance;
            }
            else
            {
                Debug.LogWarning("The Action Unit Prefabs list hasn't been instantiated.");
            }

            // The list of resources being used.
            List<NaturalResources.naturalResource> resourceList = new List<NaturalResources.naturalResource>();

            // The list of generator ids, which are the resources converted to integers.
            List<int> generatorIdList = new List<int>();

            // The list of defense ids being used.
            List<int> defenseIdList = new List<int>();

            // If the data logger has natural resources, get those.
            if(dataLogger.HasUsedNaturalResources())
            {
                resourceList.AddRange(dataLogger.usedResources);
            }
            // No resources in data logger list, so grab all resources.
            else
            {
                resourceList.AddRange(NaturalResources.GenerateNaturalResourceTypeList(false));
            }

            // There are resources, so grab the generators.
            if(resourceList.Count > 0)
            {
                // Converts the resoures to integers for ids.
                for(int i = 0; i < resourceList.Count; i++)
                {
                    generatorIdList.Add((int)resourceList[i]);
                }
            }
            else
            {
                // Generates a list of all generator units.
                if (actionUnitPrefabs != null)
                    generatorIdList.AddRange(actionUnitPrefabs.GenerateGeneratorPrefabIdList(false));
            }

            // If the data logger has defense ids, use those.
            if (dataLogger.HasActionDefenseUnits())
            {
                defenseIdList.AddRange(dataLogger.defenseIds);
            }
            // No defense ids in data logger list, so grab all defense ids.
            else
            {
                // The action unit prefabs object exists, so generate the defense id list.
                if (actionUnitPrefabs != null)
                    defenseIdList.AddRange(actionUnitPrefabs.GenerateDefensePrefabIdList(false, true));
            }


            // Creating Entries
            // Natural Resources
            foreach(NaturalResources.naturalResource resource in resourceList)
            {
                resourceEntries.Add(NaturalResources.GenerateInfoLogEntry(resource));
            }

            // Generator and Defense
            if(actionUnitPrefabs != null)
            {
                // Generator Units
                // Clears the generator entries.
                generatorEntries.Clear();

                // Gets the generator information.
                foreach (int generatorId in generatorIdList)
                {
                    // Gets the prefab. The id should match the index.
                    ActionUnitGenerator generatorPrefab = actionUnitPrefabs.GetGeneratorPrefab(generatorId);

                    // Get the info log entry.
                    if(generatorPrefab != null)
                        generatorEntries.Add(generatorPrefab.GenerateInfoLogEntry());
                }


                // Defense Units
                // Clears the current entries.
                defenseEntries.Clear();

                // Gets the defense information.
                foreach (int defenseId in defenseIdList)
                {
                    // Gets the prefab. The id should match the index.
                    ActionUnitDefense defensePrefab = actionUnitPrefabs.GetDefensePrefab(defenseId);

                    // Get the info log entry.
                    if (defensePrefab != null)
                        defenseEntries.Add(defensePrefab.GenerateInfoLogEntry());
                }

            }

            // The entry lists have been updated.
            entryListsUpdated = true;

            // Sets the current category.
            SetCurrentCategory(currCategory);
        }

        // clears the entry info lists.
        public void ClearEntryInfoLists()
        {
            // Clears the current entries.
            currEntries.Clear();

            // Clears the entries.
            resourceEntries.Clear();
            generatorEntries.Clear();
            defenseEntries.Clear();

            // Updates the buttons.
            UpdateEntryButtons();

            // Clears the info.
            ClearInfo();

            // The lists have been cleared, so now they must be updated.
            entryListsUpdated = false;
        }

        // CATEGORY

        // Returns the current category.
        public infoLogCategory GetCurrentCategory()
        {
            return currCategory;
        }

        // Sets the current category.
        public void SetCurrentCategory(infoLogCategory newCategory)
        {
            // Set the new category.
            currCategory = newCategory;

            // Sets the category name.
            categoryText.text = GetCurrentCategoryName();

            // The current entry index.
            currEntryIndex = 0;

            // Updates the current entry list.
            UpdateCurrentEntryListByCategory();
        }


        // Gets the current category as an integer.
        public int GetCurrentCategoryAsInt()
        {
            return (int)currCategory;
        }

        // Sets the current category as an int, clamped.
        public void SetCurrentCategoryAsInt(int newCategory)
        {
            // Gets the new category enum.
            infoLogCategory newCategoryEnum = (infoLogCategory)(Mathf.Clamp(newCategory, 0, CATEGORY_COUNT - 1));

            // Sets the current category.
            SetCurrentCategory(newCategoryEnum);
        }

        // Goes to the previous category.
        public void PreviousCategory()
        {
            // Increase the category.
            int prevCategory = (int)currCategory - 1;

            // Below 0, so loop to end.
            if (prevCategory < 0)
                prevCategory = CATEGORY_COUNT - 1;

            // Set the next category.
            SetCurrentCategory((infoLogCategory)prevCategory);
        }

        // Goes to the next category.
        public void NextCategory()
        {
            // Increase the category.
            int nextCategoryInt = (int)currCategory + 1;

            // Exceeded limit, so loop to start.
            if (nextCategoryInt >= CATEGORY_COUNT)
                nextCategoryInt = 0;

            // Set the next category.
            SetCurrentCategory((infoLogCategory)nextCategoryInt);

        }

        // Gets the category name.
        public static string GetCategoryName(infoLogCategory category)
        {
            // The result to return.
            string result;

            // Checks if the LOL SDK has been initialized.
            if(LOLManager.IsLOLSDKInitialized())
            {
                // Gets the key.
                string key = GetCategoryNameKey(category);

                // Gets the translated text.
                if (key != "")
                {
                    result = LOLManager.GetLanguageTextStatic(key);
                }
                // Blank since the key couldn't be found.
                else
                {
                    result = "";
                }

            }
            else
            {
                // Resulting category name.
                switch (category)
                {
                    default:
                        result = "Unknown";
                        break;

                    case infoLogCategory.resources:
                        result = "Natural Resources";
                        break;

                    case infoLogCategory.generators:
                        result = "Generators";
                        break;

                    case infoLogCategory.defenses:
                        result = "Defenses";
                        break;
                }
            }

            return result;
        }

        // Gets the name of the current category.
        public string GetCurrentCategoryName()
        {
            return GetCategoryName(currCategory);
        }

        // Gets the category name key.
        public static string GetCategoryNameKey(infoLogCategory category)
        {
            // The key.
            string key;

            // Key to return.
            switch(category)
            {
                default:
                    key = "kwd_unknown";
                    break;

                case infoLogCategory.resources:
                    key = "kwd_naturalResources";
                    break;

                case infoLogCategory.generators:
                    key = "kwd_generators";
                    break;

                case infoLogCategory.defenses:
                    key = "kwd_defenses";
                    break;

            }

            return key;
        }

        // Gets the name key of the current category.
        public string GetCurrentCategoryNameKey()
        {
            return GetCategoryNameKey(currCategory);
        }

        // Updates the category text with the current category name.
        public void UpdateCategoryText()
        {
            categoryText.text = GetCurrentCategoryName();
        }


        // ENTRIES, INFO
        // Updates the list using the current category.
        public void UpdateCurrentEntryListByCategory(bool updateButtons = true)
        {
            // TODO: maybe use a reference to the lists instead of copying them?

            // Clears the current entries.
            currEntries.Clear();

            // Checks the current category to set the current list.
            switch(currCategory)
            {
                default:
                case infoLogCategory.resources:
                    currEntries.AddRange(resourceEntries);
                    break;

                case infoLogCategory.generators:
                    currEntries.AddRange(generatorEntries);
                    break;

                case infoLogCategory.defenses:
                    currEntries.AddRange(defenseEntries);
                    break;
            }

            // If buttons should be updated.
            if(updateButtons)
            {
                UpdateEntryButtons();
            }
            
        }


        // Updates the entry buttons.
        public void UpdateEntryButtons()
        {
            // The entry index.
            int entryIndex = currEntryIndex;

            // The button index.
            int buttonIndex = 0;

            // While the button index and entry index are within the bounds.
            while(buttonIndex < entryButtons.Count && entryIndex < currEntries.Count)
            {
                // Use the current button and entry.
                if(buttonIndex < entryButtons.Count && entryIndex < currEntries.Count)
                {
                    // Gets the button.
                    InfoLogEntryButton entryButton = entryButtons[buttonIndex];

                    // Gets the entry.
                    InfoLogEntry entry = currEntries[entryIndex];

                    // Applies the entry to the entry button.
                    entryButton.ApplyEntryInfo(entry);

                    // Increases the button index and entry index.
                    buttonIndex++;
                    entryIndex++;
                }
                // No buttons left, so break.
                else
                {
                    break;
                }
            }

            // While there are buttons left, clear the buttons.
            while(buttonIndex < entryButtons.Count)
            {
                entryButtons[buttonIndex].ClearEntryInfo();
                buttonIndex++;
            }
        }

        // Clears the entry buttons.
        public void ClearEntryButtons()
        {
            // Clears all the entry buttons.
            foreach(InfoLogEntryButton entryButton in entryButtons)
            {
                entryButton.ClearEntryInfo();
            }
        }

        // Sets the info using the provided entry.
        public void SetInfo(InfoLogEntry entry)
        {
            // Sets the current entry.
            currEntry = entry;

            // If the entry exists.
            if(currEntry != null)
            {
                infoNameText.text = entry.name;
                infoDescText.text = entry.description;
                infoIconImage.sprite = entry.iconSprite;
            }
            else
            {
                ClearInfo();
            }
        }

        // Clears the info.
        public void ClearInfo()
        {
            currEntry = null;

            infoNameText.text = "";
            infoDescText.text = "";
            infoIconImage.sprite = null;
        }


        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}