using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_EDU
{
    // The action stage end dialog.
    public class ActionStageEndDialog : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The dialog box image.
        public Image dialogImage;

        // The default image color.
        public Color defaultImageColor = Color.grey;

        // The user won color.
        public Color userWonImageColor = Color.yellow;

        // The user lost color.
        public Color userLostImageColor = Color.red;

        // The stage end message text.
        public TMP_Text stageEndMessageText;

        // The stage over message.
        public const string STAGE_OVER_MESSAGE_KEY = "asg_msg_stageOver";

        // The player user has won message key.
        public const string PLAYER_USER_WON_MESSAGE_KEY = "asg_msg_userWon";

        // The player user lost message key.
        public const string PLAYER_USER_LOST_MESSAGE_KEY = "asg_msg_userLost";

        [Header("Stats")]

        // The stage time.
        public TMP_LabeledValue stageTime;

        // The stage score.
        public TMP_LabeledValue stageScore;

        // The stage user energy total.
        public TMP_LabeledValue stageEnergyTotal;

        // The stage air pollution.
        public TMP_LabeledValue stageAirPollution;

        [Header("Buttons")]

        // The buttons used when the player user wins.
        public GameObject userWonButtons;

        // The buttons used when the player user loses.
        public GameObject userLostButtons;

        // The world button.
        public Button worldButton;

        // The reset/restart button.
        public Button resetButton;

        // The finish button.
        public Button finishButton;

        // Start is called before the first frame update
        void Start()
        {
            // Sets the action UI.
            if (actionUI == null)
                actionUI = ActionUI.Instance;
        }

        // // This function is called when the behaviour becomes disabled or inactive.
        // private void OnDisable()
        // {
        //     // Sets to the default outcome.
        //     SetStageEndOutcomeDefault();
        // }

        // Sets the stage end outcome.
        // 0 = unknown
        // 1 = player user won
        // 2 = player user lost.
        public void SetStageEndOutcome(int outcome, bool updateStats = true)
        {
            // The number of outcomes.
            int outcomeCount = 3;

            // The message.
            string message = "";

            // Valid outcome number.
            int validOutcome = Mathf.Clamp(outcome, 0, outcomeCount - 1);

            // Both on by default for safety.
            userWonButtons.gameObject.SetActive(true);
            userLostButtons.gameObject.SetActive(true);

            // The outcome is valid.
            switch (validOutcome)
            {
                default:
                case 0: // Default
                    message = GetStageEndDefaultMessage();
                    dialogImage.color = defaultImageColor;

                    userWonButtons.gameObject.SetActive(false);
                    userLostButtons.gameObject.SetActive(true);
                    
                    break;

                case 1: // Player (User) Won
                    message = GetStageEndUserWonMessage();
                    dialogImage.color = userWonImageColor;

                    userWonButtons.gameObject.SetActive(true);
                    userLostButtons.gameObject.SetActive(false);

                    break;

                case 2: // Player (User) Lost
                    message = GetStageEndUserLostMessage();
                    dialogImage.color = userLostImageColor;

                    userWonButtons.gameObject.SetActive(false);
                    userLostButtons.gameObject.SetActive(true);

                    break;
            }

            // Sets the message.
            stageEndMessageText.text = message;

            // Update the stage end stats.
            if(updateStats)
            {
                UpdateStageEndStats();
            }
        }

        // Sets the stage end outcome to unknown.
        public void SetStageEndOutcomeDefault()
        {
            SetStageEndOutcome(0);
        }

        // Sets the stage end outcome to user won.
        public void SetStageEndOutcomeUserWon()
        {
            SetStageEndOutcome(1);
        }

        // Sets the stage end outome to user lost.
        public void SetStageEndOutcomeUserLost()
        {
            SetStageEndOutcome(2);
        }

        // Automatically sets the stage end outcome using the action manager.
        public void AutoSetStageEndOutcome()
        {
            // Grabs the action manager instance.
            ActionManager actionManager = ActionManager.Instance;

            // Checks the winner to know what to set the dialog to.
            if (actionManager.PlayerUserWon())
            {
                SetStageEndOutcomeUserWon();
            }
            else if (actionManager.PlayerEnemyWon())
            {
                SetStageEndOutcomeUserLost();
            }
            else
            {
                SetStageEndOutcomeDefault();
            }
        }

        // MESSAGES
        // Gets the stage end unknown message.
        public string GetStageEndDefaultMessage()
        {
            // If the LOLSDK is set, use the translated message.
            if (LOLManager.IsLOLSDKInitialized())
            {
                return LOLManager.GetLanguageTextStatic(STAGE_OVER_MESSAGE_KEY);
            }
            // Use default message.
            else
            {
                return "The stage is over!";
            }
        }

        // Gets the user won message.
        public string GetStageEndUserWonMessage()
        {
            // If the LOLSDK is set, use the translated message.
            if (LOLManager.IsLOLSDKInitialized())
            {
                return LOLManager.GetLanguageTextStatic(PLAYER_USER_WON_MESSAGE_KEY);
            }
            // Use default message.
            else
            {
                return "You've won! The enemies have run out of power!";
            }
        }

        // Gets the user lost message.
        public string GetStageEndUserLostMessage()
        {
            // If the LOLSDK is set, use the translated message.
            if (LOLManager.IsLOLSDKInitialized())
            {
                return LOLManager.GetLanguageTextStatic(PLAYER_USER_LOST_MESSAGE_KEY);
            }
            // Use default message.
            else
            {
                return "The enemies have made it into the base!";
            }
        }

        // Updates the stage end stats.
        public void UpdateStageEndStats()
        {
            // Gets the action manager instance.
            ActionManager actionManager = ActionManager.Instance;

            // Updates the values.
            stageTime.valueText.text = StringFormatter.FormatTime(actionManager.stageTimer, false, true, false);
            stageScore.valueText.text = Mathf.CeilToInt(actionManager.GetStageScore()).ToString();
            stageEnergyTotal.valueText.text = Mathf.CeilToInt(actionManager.GetStageEnergyTotal()).ToString();
            stageAirPollution.valueText.text = Mathf.CeilToInt(actionManager.GetStageAirPollution()).ToString();
        }

        // Goes to the world scene.
        public void LoadWorldScene()
        {
            ActionUI.Instance.LoadWorldScene();
        }

        // Called to finish the stage.
        public void FinishStage()
        {
            ActionUI.Instance.FinishStage();
        }

        // Resets the stage.
        public void ResetStage()
        {
            ActionUI.Instance.ResetStage();
        }

    }
}