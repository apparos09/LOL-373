using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using util;

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

        // The day night indicator.
        public DayNightIndicator dayNightIndicator;

        // The player user energy text.
        public TMP_Text playerUserEnergyText;

        // The enemy's energy bar.
        public ProgressBar playerEnemyEnergyBar;

        // The window that shows up when the stage is over.
        public GameObject stageEndWindow;

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

            // Close the stage end window.
            stageEndWindow.SetActive(false);
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

        // Updates the player user's UI.
        public void UpdatePlayerUserUI()
        {
            // Gets the user.
            ActionPlayerUser playerUser = actionManager.playerUser;

            // If the player exists, update the energy text.
            if(playerUser != null)
            {
                playerUserEnergyText.text = Mathf.Floor(playerUser.energy).ToString();
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
                // Calculates the energy percent and applies it to the energy bar.
                float energyPercent = playerEnemy.energy / playerEnemy.energyMax;
                playerEnemyEnergyBar.SetValueAsPercentage(energyPercent);
            }
        }

        // Opens the stage end window.
        public void SetStageEndWindowActive(bool active)
        {
            stageEndWindow.SetActive(active);
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

    }
}