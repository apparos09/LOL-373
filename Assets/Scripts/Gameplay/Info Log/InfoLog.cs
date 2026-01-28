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

        // The resources.
        // public List<NaturalResources.naturalResource> resourceList = new List<NaturalResources.naturalResource>();

        // The resource list as integers.
        public List<int> resourceListInt = new List<int>();

        // The defense ids.
        public List<int> defenseIdList = new List<int>();

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
                SetCurrentCategory(infoLogCategory.resources);
            }
                
        }

        // This function is called when the object becomes enabled and active.
        private void OnEnable()
        {
            UpdateEntryInfoLists();
        }

        // This function is called when the behaviour becomes disabled or inactive.
        private void OnDisable()
        {
            ClearEntryInfoLists();
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

            // The reosurce list, which is used to create the resource int list.
            List<NaturalResources.naturalResource> resourceList = new List<NaturalResources.naturalResource>();

            // Clears the lists.
            // resourceList.Clear(); // Unneeded.
            resourceListInt.Clear();
            defenseIdList.Clear();

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
                    resourceListInt.Add((int)resourceList[i]);
                }
            }
            else
            {
                // Generates a list of all generator units.
                if (actionUnitPrefabs != null)
                    resourceListInt.AddRange(actionUnitPrefabs.GenerateGeneratorPrefabIdList(false));
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

            // The entry lists have been updated.
            entryListsUpdated = true;
        }

        // clears the entry info lists.
        public void ClearEntryInfoLists()
        {
            // resourceList.Clear(); // Unneeded.
            resourceListInt.Clear();
            defenseIdList.Clear();

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
            currCategory = newCategory;

            // TODO: change the current list.
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

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}