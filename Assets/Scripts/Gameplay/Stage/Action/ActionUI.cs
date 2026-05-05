using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using util;
using UnityEngine.UI;

namespace RM_EDU
{
    // The action UI.
    public class ActionUI : StageUI
    {
        // The singleton instance.
        private static ActionUI instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("Action")]

        // The action manager.
        public ActionManager actionManager;

        // The day-night overlay.
        public DayNightOverlay dayNightOverlay;

        // The action event overlay.
        // public ActionEventOverlay actionEventOverlay;

        // The stage start dialog.
        public ActionStageStartDialog stageStartDialog;

        // The info log dialog.
        public InfoLog infoLogDialog;

        // The dialog that shows up when the stage is over.
        public ActionStageEndDialog stageEndDialog;

        [Header("Action/Top Header")]

        // The info log button.
        public Button infoLogButton;

        // The day night indicator.
        public DayNightIndicator dayNightIndicator;

        // The player user energy text.
        public TMP_Text playerUserEnergyText;

        // The player user air pollution text.
        public TMP_Text playerUserAirPollutionText;

        // The wind indicator.
        public WindIndicator windIndicator;

        [Header("Action/Bottom Header")]

        // The generator selector.
        public ActionUnitSelector generatorUnitSelector;

        // The selected unit UI.
        public ActionUnitSelectedUI selectedUnitUI;

        // The defense selector.
        public ActionUnitSelector defenseUnitSelector;

        [Header("Action/Left")]

        // The speed button.
        public ActionStageSpeedButton speedButton;

        // The unit deselect button.
        public ActionUnitDeselectButton deselectButton;

        // The unit remove button.
        public ActionUnitRemoveButton removeButton;

        // The energy block button.
        public ActionUserBlockButton blockButton;

        [Header("Action/Right")]

        // The enemy's energy bar.
        public ProgressBar playerEnemyEnergyBar;

        [Header("Action/Overlays, Notifications")]

        // The enemies approaching notification.
        public ActionStageNotification enemyApproachNotif;

        // If the enemy approach notification has been played.
        public bool playedEnemyApproachNotif = false;

        // Constructor
        private ActionUI()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected override void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Gets the instance.
            if (actionManager == null)
                actionManager = ActionManager.Instance;
        }

        // Gets the instance.
        public static ActionUI Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<ActionUI>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Action UI (singleton)");
                        instance = go.AddComponent<ActionUI>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // DAY-NIGHT OVERLAY
        // Updates the day-night overlay using the provided t-value.
        public void UpdateDayNightEffect(float t)
        {
            dayNightOverlay.UpdateOverlay(t);
        }

        // PLAYER UI
        // Updates the player user's UI.
        public void UpdatePlayerUserUI()
        {
            // Gets the user.
            ActionPlayerUser playerUser = actionManager.playerUser;

            // If the player exists, update the energy and air pollution text.
            if(playerUser != null)
            {
                // The new energy and air pollution text.
                string newEnergyText = Mathf.Floor(playerUser.energy).ToString();
                string newAirPollutionText = Mathf.Floor(playerUser.airPollution).ToString();

                // If the current player user energy text is different, change it to the new value.
                if (playerUserEnergyText.text != newEnergyText)
                    playerUserEnergyText.text = newEnergyText;

                // If the current player user air pollution text is different, change it to the new value.
                if (playerUserAirPollutionText.text != newAirPollutionText)
                    playerUserAirPollutionText.text = newAirPollutionText;
            }
            // The player user couldn't be found, so clear the text.
            else
            {
                // The default text.
                string defaultText = "-";

                // If the player user energy text isn't set to the default text, set it.
                if (playerUserEnergyText.text != defaultText)
                    playerUserEnergyText.text = defaultText;

                // If the player air pollution text isn't set to the default text, set it.
                if(playerUserAirPollutionText.text != defaultText)
                    playerUserAirPollutionText.text = defaultText;
            }
        }

        // Updates the player enemy UI.
        public void UpdatePlayerEnemyUI()
        {
            // Gets the enemy.
            ActionPlayerEnemy playerEnemy = actionManager.playerEnemy;

            // If the enemy exists, update the bar.
            if (playerEnemy != null)
            {
                // Since the player enemy's energy amount is constantly changing...
                // It's probably fine to update this every frame.

                // Calculates the energy percent and applies it to the energy bar.
                float energyPercent = playerEnemy.energy / playerEnemy.energyMax;
                playerEnemyEnergyBar.SetValueAsPercentage(energyPercent);
            }
        }

        // SELECT UNIT //
        // Refreshes the interactable component of the unit buttons.
        public void RefreshUnitButtonsInteractable()
        {
            generatorUnitSelector.RefreshUnitButtonsInteractable();
            defenseUnitSelector.RefreshUnitButtonsInteractable();
        }

        // Sets the selected unit info to the select UI.
        public void SetSelectedUnitInfoToSelect()
        {
            selectedUnitUI.ClearActionUnitInfo();
        }

        // Sets the selected unit info to the remove UI.
        public void SetSelectedUnitInfoToRemove()
        {
            selectedUnitUI.ApplyRemoveActionUnitInfo(removeButton);
        }

        // Updates the selected unit info.
        public void SetSelectedUnitInfo(ActionUnit unit)
        {
            selectedUnitUI.ApplyActionUnitInfo(unit);
        }

        // Updates the selected unit info.
        public void SetSelectedUnitInfo(ActionUnitButton unitButton)
        {
            selectedUnitUI.ApplyActionUnitInfo(unitButton);
        }

        // Clears the selected unit.
        // NOTE: this does not deselect the unit from the player.
        public void ClearSelectedUnitInfo()
        {
            selectedUnitUI.ClearActionUnitInfo();
        }

        // Deselects the player's selected unit.
        public void DeselectSelectedUnit()
        {
            actionManager.playerUser.ClearSelectedActionUnitPrefab();
        }

        // ENERGY BLOCK //
        // Returns 'true' if the player user is blocking their attack energy (defense units cannot use energy if true).
        public bool IsUserBlockingAttackEnergy()
        {
            return actionManager.playerUser.IsBlockingAttackEnergy();
        }

        // Sets whether to block attack energy or not.
        public void SetBlockUserAttackEnergy(bool value)
        {
            actionManager.playerUser.SetBlockingAttackEnergy(value);
        }

        // Starts blocking the user attack energy.
        public void BlockUserAttackEnergy()
        {
            SetBlockUserAttackEnergy(true);
        }

        // Stops blocking the user attack energy.
        public void UnblockUserAttackEnergy()
        {
            SetBlockUserAttackEnergy(false);
        }

        // Toggle blocking acction energy.
        public void ToggleBlockUserAttackEnergy()
        {
            SetBlockUserAttackEnergy(!actionManager.playerUser.blockingAttackEnergy);
        }

        // NOTIFICATIONS //
        // Plays the enemies approaching notification.
        public void PlayEnemiesApproachingNotification()
        {
            enemyApproachNotif.PlayBlinkingAnimation();
            playedEnemyApproachNotif = true;
        }

        // DIALOGS //
        // Generates a list of dialogs.
        public override List<GameObject> GenerateDialogList()
        {
            // Gets the base list.
            List<GameObject> dialogList = base.GenerateDialogList();

            // Adds the rest of the dialogs.
            dialogList.Add(stageStartDialog.gameObject);
            dialogList.Add(infoLogDialog.gameObject);
            dialogList.Add(stageEndDialog.gameObject);

            return dialogList;
        }

        // Opens the stage tart dialog.
        public void OpenStageStartDialog()
        {
            OpenDialog(stageStartDialog.gameObject, true);
            stageStartDialog.StopStage();
        }

        // Closes the stage start dialog.
        public void CloseStageStartDialog()
        {
            CloseDialog(stageStartDialog.gameObject);
            // StartStage() is called by button in Stage Start Dialog.
        }

        // Returns 'true' if the info log is open.
        public bool IsInfoLogDialogOpen()
        {
            return infoLogDialog.gameObject.activeSelf;
        }

        // Opens the info log dialog.
        public void OpenInfoLogDialog(bool closeOtherDialogs)
        {
            OpenDialog(infoLogDialog.gameObject, closeOtherDialogs);
        }

        // Closes the info log dialog.
        public void CloseInfoLogDialog()
        {
            CloseDialog(infoLogDialog.gameObject);
        }

        // Opens the stage end dialog.
        public void OpenStageEndDialog()
        {
            OpenDialog(stageEndDialog.gameObject, true);
            stageEndDialog.AutoSetStageEndOutcome();
        }

        // Closes the stage end dialog.
        public void CloseStageEndDialog()
        {
            CloseDialog(stageEndDialog.gameObject);
        }

        // FINISH
        // Resets the stage.
        public override void ResetStage()
        {
            // Resets the stage and closes all dialogs.
            actionManager.ResetStage();
            CloseAllDialogs();
        }


        // Called to finish the stage.
        public override void FinishStage()
        {
            actionManager.FinishStage();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If the stage is playing and the game is unpaused.
            if(actionManager.IsStagePlayingAndGameUnpaused())
            {
                // Updates the players.
                UpdatePlayerUserUI();
                UpdatePlayerEnemyUI();
            }
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }

    }
}