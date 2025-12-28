using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // Action unit button page.
    public class ActionUnitSelector : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // An array of prefabs.
        public ActionUnit[,] unitPrefabs = null;

        // The prefabs row index, which determines what prefabs to currently display.
        public int currUnitPrefabsRow = -1;

        [Header("Buttons")]

        // The buttons on the page.
        public List<ActionUnitButton> unitButtons = new List<ActionUnitButton>();

        // The default number of unit buttons that're used for the action unit selector.
        public const int UNIT_BUTTONS_COUNT_DEFAULT = 5;

        // The previous page (page up) button.
        public Button prevPageButton;

        // The next page (page down) button.
        public Button nextPageButton;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the instance.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            RefreshUnitButtons();
        }

        // Sets the action unit prefabs using the provided list.
        // The program assembles the list into an array.
        // The number of columns is based on the number of buttons.
        public void SetActionUnitPrefabs(List<ActionUnit> newPrefabs)
        {
            // Gets the row count.
            // Any remaining spots in a page will be left empty.
            int rowCount = Mathf.CeilToInt(newPrefabs.Count / unitButtons.Count);

            // Gets the column count.
            int colCount = unitButtons.Count;

            // Creates the unit prefabs array.
            unitPrefabs = new ActionUnit[rowCount, colCount];

            // The index of the new prefabs list.
            int newPrefabsIndex = 0;

            // Fills in the unit prefabs 2D array with the prefabs from the list.
            for(int r = 0; r < unitPrefabs.GetLength(0); r++) // Rows
            {
                for(int c = 0; c < unitPrefabs.GetLength(1); c++) // Columns
                {
                    // Index validity check.
                    if(newPrefabsIndex >= 0 && newPrefabsIndex < newPrefabs.Count)
                    {
                        // Puts the value in the array and increases the index.
                        unitPrefabs[r, c] = newPrefabs[newPrefabsIndex];
                        newPrefabsIndex++;
                    }
                    else
                    {
                        // No prefab, so fill with null.
                        unitPrefabs[r, c] = null;
                    }
                }
            }
        }

        // Sets the row.
        public void SetRow(int newRow)
        {
            // If the unit prefabs list doesn't exist, set the row to -1 and return.
            if(unitPrefabs == null)
            {
                currUnitPrefabsRow = -1;
                return;
            }

            // Gets the prefab row index, clamping the value.
            currUnitPrefabsRow = Mathf.Clamp(newRow, 0, unitPrefabs.GetLength(0));

            // Refreshes the unit buttons.
            RefreshUnitButtons();
        }

        // Goes to the previous row.
        public void PreviousRow()
        {
            // The new row.
            int newRow = currUnitPrefabsRow - 1;

            // If the new row is now negative, loop around to the end.
            if (newRow < 0)
                newRow = unitPrefabs.GetLength(0) - 1;

            // Sets the row.
            SetRow(newRow);
        }

        // Goes to the next row.
        public void NextRow()
        {
            // The new row.
            int newRow = currUnitPrefabsRow + 1;

            // If row is now out of bounds, loop around to the start.
            if (newRow >= unitPrefabs.GetLength(0))
                newRow = 0;

            // Sets the row.
            SetRow(newRow);
        }

        // Refreshes the unit buttons.
        public void RefreshUnitButtons()
        {
            // Gets set to true if the refresh attempt was valid.
            bool valid = false;

            // The array exists.
            if(unitPrefabs != null)
            {
                // True if the row is valid.
                valid = currUnitPrefabsRow >= 0 && currUnitPrefabsRow < unitPrefabs.Length;

                // If row is valid, refresh the buttons.
                if(valid)
                {
                    // The row.
                    int r = currUnitPrefabsRow;

                    // The unit button index.
                    int unitButtonIndex = 0;

                    // Goes through all columns at the current row.
                    for (int c = 0; c < unitPrefabs.GetLength(1); c++)
                    {
                        // Gets the unit button.
                        // If a button doens't have a prefab, turn the button off.
                        ActionUnitButton unitButton = unitButtons[unitButtonIndex];

                        // The unit button exists.
                        if(unitButton != null)
                        {
                            // If there's a prefab, set it to the unit button.
                            if (unitPrefabs[r, c] != null)
                            {
                                unitButton.ClearUnitButton();
                            }
                            // No prefab, so clear the button and disable it.
                            else
                            {
                                unitButton.ClearUnitButton();
                            }
                        }
                    }
                }
            }

            // If the button refresh wasn't successful, clear the buttons.
            if(!valid)
            {
                // Goes through all the unit buttons, clearing each one.
                foreach (ActionUnitButton unitButton in unitButtons)
                {
                    unitButton.ClearUnitButton();
                }
            }

            // Checks if there are unit prefabs.
            if(unitPrefabs != null)
            {
                // The buttons loop around if they go over or under the page number.
                // As such, the buttons should be interactable as long as there's more than 1 page.
                if(unitPrefabs.GetLength(0) > 1)
                {
                    prevPageButton.interactable = true;
                    nextPageButton.interactable = true;
                }
                else
                {
                    prevPageButton.interactable = false;
                    nextPageButton.interactable = false;
                }
            }
            else
            // No unit prefabs, so make the buttons non-interactable.
            {
                prevPageButton.interactable = false;
                nextPageButton.interactable = false;
            }
               
        }

        // Refreshes the interactable component of the unit buttons.
        public void RefreshUnitButtonsInteractable()
        {
            // Refreshes all the unit button interactables.
            foreach(ActionUnitButton unitButton in unitButtons)
            {
                unitButton.RefreshUnitButtonInteractable();
            }
        }
    }
}