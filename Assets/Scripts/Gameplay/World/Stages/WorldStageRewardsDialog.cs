using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The world stage rewards dialog.
    public class WorldStageRewardsDialog : MonoBehaviour
    {
        // The world UI.
        public WorldUI worldUI;

        // The world stage the rewards are being displayed from.
        public WorldStage worldStage;

        [Header("Rewards")]

        // The selected reward unit.
        public TMP_LabeledValue selectedRewardUnit;

        // The reward buttons.
        public List<ActionUnitRewardButton> rewardButtons = new List<ActionUnitRewardButton>();

        // The reward unit prefabs list.
        public List<ActionUnit> rewardUnitPrefabs = new List<ActionUnit>();

        // The index in the reward units prefab list.
        [Tooltip("The index in the reward unit prefabs list.")]
        public int rewardUnitPrefabsIndex = 0;

        // If 'true', the rewards the player already has are ignored.
        private bool ignoreEarnedRewards = true;

        // If 'true', the dialog is closed if there are no rewards.
        private bool closeIfNoRewards = true;

        // Sets to 'true' if the rewards have been loaded.
        private bool rewardsInfoLoaded = false;

        [Header("Buttons")]

        // The previous index button.
        public Button prevIndexButton;

        // The next index button.
        public Button nextIndexButton;

        // The continue button.
        public Button continueButton;

        // Awake is called when the script instance is being loaded.
        private void Awake()
        {
            // Clears the selected unit info if no rewards have been loaded.
            if(!rewardsInfoLoaded)
            {
                ClearSelectedUnitInfo();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Gets the world UI.
            if (worldUI == null)
                worldUI = WorldUI.Instance;
        }

        // LOAD REWARDS //
        // Returns 'true' if the rewards info has been loaded.
        public bool IsRewardsInfoLoaded()
        {
            return rewardsInfoLoaded;
        }

        // Loads the rewards info.
        public void LoadRewardsInfo(WorldStage worldStage)
        {
            // Saves the world stage.
            this.worldStage = worldStage;

            // If the world stage exists, load the rewards info.
            if(worldStage != null)
            {
                // The action unit prefabs.
                ActionUnitPrefabs actionUnitPrefabs = null;

                // If the action unit prefab list is instantiated, get the instance.
                if (ActionUnitPrefabs.Instantiated)
                {
                    actionUnitPrefabs = ActionUnitPrefabs.Instance;
                }
                else
                {
                    Debug.LogWarning("The Action Unit Prefabs list hasn't been instantiated.");
                }

                // Clears the prefabs.
                rewardUnitPrefabs.Clear();

                // The reward unit ids.
                List<int> rewardUnitIds = new List<int>();

                // If there are reward units.
                if(worldStage.defenseIdRewards.Count > 0)
                    rewardUnitIds.AddRange(worldStage.defenseIdRewards);


                // If earned rewards should be ignored, remove them from the list.
                if(ignoreEarnedRewards)
                {
                    // Removes defense units the player already has.
                    // NOTE: make sure this function is called BEFORE the player...
                    // Has been given the rewards, so that this script knows what...
                    // Rewards have been earned.
                    DataLogger dataLogger = DataLogger.Instance;

                    // Goes through the defense ids in the data logger and removes them...
                    // From the rewards list.
                    for(int i = 0; i < dataLogger.defenseIds.Count; i++)
                    {
                        // If the defense id is in the reward units list, remove it.
                        if(rewardUnitIds.Contains(dataLogger.defenseIds[i]))
                        {
                            // Removes the defense unit id from the reward units list.
                            rewardUnitIds.Remove(dataLogger.defenseIds[i]);
                        }
                    }
                }


                // Goes through the reward units to load the prefabs.
                for(int i = 0; i < rewardUnitIds.Count; i++)
                {
                    // Gets the defense prefab.
                    // NOTE: this uses the index and not the id, but these should provide the same result.
                    ActionUnitDefense defensePrefab = actionUnitPrefabs.GetDefensePrefab(rewardUnitIds[i]);

                    // Adds the defense prefab to the list.
                    if (defensePrefab != null)
                        rewardUnitPrefabs.Add(defensePrefab);
                }

                // Set the index to 0, since the reward buttons are loaded from the start...
                // Of the list.
                rewardUnitPrefabsIndex = 0;

                // Load in the unit prefabs.
                for(int i = 0; i < rewardButtons.Count; i++)
                {
                    // If there's a reward unit prefab, apply the info.
                    if(i < rewardUnitPrefabs.Count)
                    {
                        rewardButtons[i].ApplyUnitPrefabInfo(rewardUnitPrefabs[i]);
                    }
                    // No prefab, so clear the button.
                    else
                    {
                        rewardButtons[i].ClearUnitPrefabInfo();
                    }
                }

                // Refresh the page buttons interactable.
                RefreshUnitPrefabIndexButtonsInteractable();

                // If the dialog should be closed if there are no rewards...
                // And there are no reward unit prefabs, close the dialog.
                if(closeIfNoRewards && rewardUnitPrefabs.Count <= 0)
                {
                    CloseDialog();
                }
            }
            // No world stage, so clear the info.
            else
            {
                // Clears the rewards dialog info.
                ClearInfo();
            }

            // The rewards have been loaded.
            rewardsInfoLoaded = true;
        }

        // UNIT PREFABS INDEX //
        // Sets the reward unit prefabs index, which acts as the current page.
        public void SetRewardUnitPrefabsIndex(int index)
        {
            // Sets the new index.
            rewardUnitPrefabsIndex = Mathf.Clamp(index, 0, rewardUnitPrefabs.Count - 1);
        
            // The current prefab index for loading information.
            int currPrefabIndex = rewardUnitPrefabsIndex;

            // Loads the info for the buttons.
            for(int i = 0; i < rewardButtons.Count; i++)
            {
                // If the prefab exists, apply the info.
                if (rewardUnitPrefabs[currPrefabIndex] != null)
                {
                    rewardButtons[i].ApplyUnitPrefabInfo(rewardUnitPrefabs[currPrefabIndex]);
                }
                // Prefab is null, so clear info.
                else
                {
                    rewardButtons[i].ClearUnitPrefabInfo();
                }

                // Increase the current prefab index.
                currPrefabIndex++;

                // If the prefab index has passed the end of the list...
                // Loop around to the beginning.
                if(currPrefabIndex >= rewardUnitPrefabs.Count)
                {
                    currPrefabIndex = 0;
                }
            }
        }

        // Goes to the previous reward unit prefabs index.
        public void PreviousRewardUnitPrefabsIndex()
        {
            // The current index minus 1.
            int index = rewardUnitPrefabsIndex - 1;

            // Loop around to the end.
            if (index < 0)
                index = rewardUnitPrefabs.Count - 1;

            // Sets the index.
            SetRewardUnitPrefabsIndex(index);
        }

        // Goes to the next reward unit prefabs index.
        public void NextRewardUnitPrefabsIndex()
        {
            // The current index plus 1.
            int index = rewardUnitPrefabsIndex + 1;

            // Loop around to the start.
            if (index >= rewardUnitPrefabs.Count)
                index = 0;

            // Sets the index.
            SetRewardUnitPrefabsIndex(index);
        }

        // Refreshes the page button interactables.
        public void RefreshUnitPrefabIndexButtonsInteractable()
        {
            // If there are more prefabs than there are buttons, there are multiple pages.
            if(rewardUnitPrefabs.Count > rewardButtons.Count)
            {
                prevIndexButton.interactable = true;
                nextIndexButton.interactable = true;
            }
            // Only one page, so disable buttons.
            else
            {
                prevIndexButton.interactable = false;
                nextIndexButton.interactable = false;
            }
        }

        // SELECTION //
        // Applies the selected unit information from the reward button.
        public void ApplySelectedUnitInfo(ActionUnitRewardButton rewardButton)
        {
            // If the reward button has a prefab.
            if(rewardButton.HasUnitPrefab())
            {
                selectedRewardUnit.headerText.text = rewardButton.unitPrefab.GetUnitNameTranslated();
                selectedRewardUnit.valueText.text = rewardButton.unitPrefab.GetUnitDescriptionTranslated();
            }
            // No prefab, so clear.
            else
            {
                ClearSelectedUnitInfo();
            }
        }

        // Clears the selected unit information.
        public void ClearSelectedUnitInfo()
        {
            selectedRewardUnit.headerText.text = "-";
            selectedRewardUnit.valueText.text = "-";
        }

        // Clears the info.
        public void ClearInfo()
        {
            // Clears all the reward buttons.
            foreach(ActionUnitRewardButton rewardButton in rewardButtons)
            {
                rewardButton.ClearUnitPrefabInfo();
            }

            // Clears the selected unit info.
            ClearSelectedUnitInfo();

            // Clears the prefabs list and sets the index to 0.
            rewardUnitPrefabs.Clear();
            rewardUnitPrefabsIndex = 0;

            // The rewards info haven't been loaded.
            rewardsInfoLoaded = false;
        }

        // Closes the dialog.
        public void CloseDialog()
        {
            WorldUI.Instance.CloseWorldStageRewardDialog();
        }
    }
}