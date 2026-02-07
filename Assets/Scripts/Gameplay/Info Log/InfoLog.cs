using System;
using System.Collections.Generic;
using TMPro;
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

            // The name key.
            public string nameKey;

            // The description of the info entry.
            public string description;

            // The description key.
            public string descriptionKey;

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

        // TODO: don't clear the current entries if no changes have happened.

        // The list of current entries loaded.
        protected List<InfoLogEntry> currEntries = new List<InfoLogEntry>();

        // Resource entries.
        protected List<InfoLogEntry> resourceEntries = new List<InfoLogEntry>();

        // Generator entries.
        protected List<InfoLogEntry> generatorEntries = new List<InfoLogEntry>();

        // Defense entires.
        protected List<InfoLogEntry> defenseEntries = new List<InfoLogEntry>();

        // Gets set to 'true' if the entry lists have been updated.
        private bool entryListsUpdated = false;

        // If 'true', the info log is reset (cleared) on disable.
        // If 'false', the game does some quick checks to see if the unlocked resources and defense ids...
        // Have changed, which if so causes the info log to reset. If these checks notice no changes...
        // The info log is left as is.
        private bool resetInfoLogOnDisable = false;

        // If 'true', all entries are unlocked automatically.
        private bool unlockAllEntries = false;

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

        // The entry button page text.
        public TMP_Text entryButtonsPageText;

        // The entry button page index.
        protected int entryButtonPageIndex = 0;

        // The previous entry page button.
        public Button prevEntryPageButton;

        // The next entry page button.
        public Button nextEntryPageButton;

        [Header("Info Section")]

        // The info icon image.
        public Image infoIconImage;

        // The default icon sprite for the info log.
        public Sprite defaultIconSprite;

        // A sprite that's a single, empty image.
        [Tooltip("An empty image sprite.")]
        public Sprite alpha0Sprite;

        // The info name text.
        public TMP_Text infoNameText;

        // The info description text.
        public TMP_Text infoDescText;

        // The info description scroll view.
        public ScrollRect infoDescScrollView;

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
                
            // Clears the info by default.
            ClearInfo();
        }

        // This function is called when the object becomes enabled and active.
        private void OnEnable()
        {
            // The lists are marked as already been updated, so see if they need a refresh.
            // This only updates the lists if the list elements have changed.
            if (entryListsUpdated)
            {
                TryRefreshInfoLog();
            }
            // The lists haven't been updated, so update them.
            else
            {
                UpdateEntryInfoLists();
            }
        }

        // This function is called when the behaviour becomes disabled or inactive.
        private void OnDisable()
        {
            // If the info log should be reset when the info log is disabled.
            if(resetInfoLogOnDisable)
            {
                ResetInfoLog();
            }
        }

        // If 'true', all entries are unlocked automatically.
        public bool UnlockAllEntries
        {
            get { return unlockAllEntries; }
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
            if(!unlockAllEntries && dataLogger.HasUsedNaturalResources())
            {
                resourceList.AddRange(dataLogger.usedResources);
            }
            // No resources in data logger list, so grab all resources.
            else
            {
                resourceList.AddRange(NaturalResources.GenerateNaturalResourceTypeList(false));
            }

            // There are resources, so grab the generators.
            if(!unlockAllEntries && resourceList.Count > 0)
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
            if (!unlockAllEntries && dataLogger.HasActionDefenseUnits())
            {
                defenseIdList.AddRange(dataLogger.defenseIds);

                // Defense 1 (Lane Blaster) isn't given as an option to the player, since those units...
                // Are placed automatically.
                // However, they should be in the defense id list, so that id is insrted regardless.
                if(!defenseIdList.Contains(1))
                {
                    // Inserts the id at the beginning of the list.
                    defenseIdList.Insert(0, 1);
                }
            }
            // No defense ids in data logger list, so grab all defense ids.
            else
            {
                // The action unit prefabs object exists, so generate the defense id list.
                if (actionUnitPrefabs != null)
                    defenseIdList.AddRange(actionUnitPrefabs.GenerateDefensePrefabIdList(false, true));
            }

            // Sorts all three lists to make sure the order stays consistent.
            // Probably unnecessary.
            resourceList.Sort();
            generatorIdList.Sort();
            defenseIdList.Sort();

            // Creating Entries
            // Natural Resources
            foreach(NaturalResources.naturalResource resource in resourceList)
            {
                // resourceEntries.Add(NaturalResources.GenerateInfoLogEntry(resource));

                // Use default image.
                InfoLogEntry newEntry = NaturalResources.GenerateInfoLogEntry(resource);

                // If the Natural Resources class has been instantiated, get the symbol.
                // Since this is a sprite, the class must be instantiated beforehand.
                if(NaturalResources.Instantiated)
                    newEntry.iconSprite = NaturalResources.Instance.GetNaturalResourceSymbol(resource);

                // If the icon sprite is null, use the default sprite icon.
                if(newEntry.iconSprite == null)
                    newEntry.iconSprite = defaultIconSprite; // TODO: replace with proper image.
                
                resourceEntries.Add(newEntry);
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

        // clears the entry info lists, which also clears the info.
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
            // Read the TTS if it's available.
            SetCurrentCategory(newCategory, true);
        }

        // Sets the current category.
        // speakCategory: if true, the category name is read with TTS if TTS is available.
        public void SetCurrentCategory(infoLogCategory newCategory, bool speakCategory)
        {
            // Set the new category.
            currCategory = newCategory;

            // Sets the category name.
            categoryText.text = GetCurrentCategoryName();

            // Updates the current entry list.
            UpdateCurrentEntryListByCategory();

            // Sets the entry buttons page index to 0.
            SetEntryButtonsPageIndex(0);

            // If TTS should be used to read the category...
            // Text-to-speech is usable, and text-to-speech is enabled...
            // Read the category.
            if (speakCategory && LOLManager.IsTextToSpeechUsableAndEnabled())
            {
                // Gets the speak key.
                string speakKey = GetCurrentCategoryNameKey();

                // Speak the text.
                if (speakKey != "")
                    SpeakText(speakKey);
            }
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

        // Resets the current category, making it 0.
        public void ResetCurrentCategory()
        {
            // Since the category is being reset, don't read its name with TTS.
            SetCurrentCategory(0, false);
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


        // ENTRIES
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
            int entryIndex = GetCurrentEntryIndexInEntryButtonsPages();

            // The button index.
            int buttonIndex = 0;

            // If entry index is -1, then that means there are no entries to show.
            // If that's the case, don't load any entries.
            if(entryIndex >= 0)
            {
                // While the button index and entry index are within the bounds.
                while (buttonIndex < entryButtons.Count && entryIndex < currEntries.Count)
                {
                    // Use the current button and entry.
                    if (buttonIndex < entryButtons.Count && entryIndex < currEntries.Count)
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
            }

            // While there are buttons left, clear the buttons.
            while(buttonIndex < entryButtons.Count)
            {
                entryButtons[buttonIndex].ClearEntryInfo();
                buttonIndex++;
            }

            // If the entry buttons page count is greater than 1, activate the page buttons.
            if(GetEntryButtonsPageCount() > 1)
            {
                prevEntryPageButton.interactable = true;
                nextEntryPageButton.interactable = true;
            }
            // Deactivate page buttons.
            else
            {
                prevEntryPageButton.interactable = false;
                nextEntryPageButton.interactable = false;
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

        // Get Entry Button Pages
        // Gets the entry button page count.
        public int GetEntryButtonsPageCount()
        {
            // The entry count.
            int entryCount = currEntries.Count;

            // The entry button count.
            int entryButtonCount = entryButtons.Count;

            // The number of entry pages.
            int entryPages;

            // If there are no buttons, there's only one page.
            if (entryButtonCount > 0)
                entryPages = Mathf.CeilToInt((float)entryCount / entryButtons.Count);
            else
                entryPages = 1;

            // Returns the number of pages.
            return entryPages;
        }

        // Gets the entry buttons page index.
        public int GetEntryButtonsPageIndex()
        {
            return entryButtonPageIndex;
        }

        // Sets the entry buttons page using the provided index.
        public void SetEntryButtonsPageIndex(int pageIndex)
        {
            // Gets the entry buttons page count.
            int pageCount = GetEntryButtonsPageCount();

            // Clamps the page index within the proper bounds.
            entryButtonPageIndex = Mathf.Clamp(pageIndex, 0, pageCount - 1);

            // Updates the entry buttons page text.
            entryButtonsPageText.text = (entryButtonPageIndex + 1).ToString() + "/" + pageCount.ToString();

            // Updates the entry buttons.
            UpdateEntryButtons();
        }

        // Gets the current entry index in the entry buttons pages.
        public int GetCurrentEntryIndexInEntryButtonsPages()
        {
            // Calculates the current entry index based on the number of buttons.
            // e.g., if there's 5 buttons and 3 pages, each page can display up to 5 entries.
            //  - On page 2 (index 1), the current entry index would be 5.
            //  - Page 0 would display entries (0-4), and Page 1 would display entries (5-9).
            int entryIndex = entryButtonPageIndex * entryButtons.Count;

            // Clamp within the bounds of the current entries list (minus 1 because index).
            // Originally it was 'entryIndex = Mathf.Clamp(entryIndex, 0, entryButtons.Count);'...
            // But that was an error.
            entryIndex = Mathf.Clamp(entryIndex, 0, currEntries.Count - 1);

            return entryIndex;
        }

        // Goes to the previous entry buttons page.
        public void PreviousEntryButtonsPage()
        {
            // Page count and index.
            int pageCount = GetEntryButtonsPageCount();
            int pageIndex = entryButtonPageIndex - 1;

            // Bounds check.
            if (pageIndex < 0)
                pageIndex = pageCount - 1;

            // Set the page.
            SetEntryButtonsPageIndex(pageIndex);
        }

        // Goes to the next entry buttons page.
        public void NextEntryButtonsPage()
        {
            // Page count and index.
            int pageCount = GetEntryButtonsPageCount();
            int pageIndex = entryButtonPageIndex + 1;

            // Bounds check.
            if (pageIndex >= pageCount)
                pageIndex = 0;

            // Set the page.
            SetEntryButtonsPageIndex(pageIndex);
        }

        // INFO
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

                // Sets the scroll view value to the top.
                infoDescScrollView.verticalScrollbar.value = 1;

                // If text-to-speech is usable and enabled.
                if (entry.descriptionKey != "" && LOLManager.IsTextToSpeechUsableAndEnabled())
                {
                    // Read the description.
                    SpeakText(entry.descriptionKey);
                }
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
            infoIconImage.sprite = alpha0Sprite;

            // Sets the scroll view value to the top.
            infoDescScrollView.verticalScrollbar.value = 1;
        }

        // INFO LOG (General)
        // Refreshes the info log.
        public void RefreshInfoLog()
        {
            // Resets the info log.
            ResetInfoLog();

            // Updates the entry info lists.
            UpdateEntryInfoLists();
        }

        // Refreshes the info log if the information has changed.
        // If the information hasn't changed, then the refresh isn't done.
        public bool TryRefreshInfoLog()
        {
            // The result to return. Set to true if the info log was refreshed.
            bool refreshed;

            // If the entry lists are marked as updated, check for a refresh.
            if(entryListsUpdated)
            {
                // If the data loggger exists, check for changes.
                if (DataLogger.Instantiated)
                {
                    // Gets the instance.
                    DataLogger dataLogger = DataLogger.Instance;

                    // Checks if the resource count and defense id counts have changed.
                    // Since the lane blaster is included in the info log, but isn't selectable by the player...
                    // A plus 1 is done to the defense id count from the data logger.
                    bool resCountSame = generatorEntries.Count == dataLogger.usedResources.Count;
                    bool defCountSame = defenseEntries.Count == (dataLogger.defenseIds.Count + 1);

                    // Notably, if the lists were changed but kept their counts consistent, the game...
                    // Would consider the lists unchanged. However, this case should never happen...
                    // So leaving it like this should be fine.
                    // If the counts have remained unchanged, don't refresh.
                    if (resCountSame && defCountSame)
                    {
                        refreshed = false;
                    }
                    // Some of these counts have changed, so refresh.
                    else
                    {
                        RefreshInfoLog();
                        refreshed = true;
                    }
                }
                // Data logger doesn't exist, so refresh anyway.
                else
                {
                    RefreshInfoLog();
                    refreshed = true;
                }
            }
            // The enty lists haven't been updated, so refresh.
            else
            {
                RefreshInfoLog();
                refreshed = true;
            }

            return refreshed;
        }

        // Resets the info log, clearing the information.
        public void ResetInfoLog()
        {
            // Clears the entry info lists.
            // Also updates the entry buttons.
            ClearEntryInfoLists();

            // Resets the current category.
            // This updates the entry buttons again, but that should be fine.
            ResetCurrentCategory();
        }

        // TEXT-TO-SPEECH
        // Speaks text if Text-to-Speech is usable and enabled.
        public void SpeakText(string key)
        {
            // If text-to-speech is usable and enabled, speak the text.
            if(key != "" && LOLManager.IsTextToSpeechUsableAndEnabled())
            {
                LOLManager.Instance.SpeakText(key);
            }
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}