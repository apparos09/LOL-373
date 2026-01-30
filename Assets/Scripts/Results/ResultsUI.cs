using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace RM_EDU
{
    // The results UI.
    public class ResultsUI : MonoBehaviour
    {
        // The singleton instance.
        private static ResultsUI instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The results manager.
        public ResultsManager resultsManager;

        // The title scene.
        public string titleScene = "TitleScene";

        [Header("Final Texts")]

        // The final score text.
        public TMP_LabeledValue gameScore;

        // The final time text.
        public TMP_LabeledValue gameTime;

        // The energy total text.
        public TMP_LabeledValue gameEnergyTotal;

        [Header("Stage Infos")]

        // The results stage infos.
        public List<ResultsStageInfo> resultsStageInfos = new List<ResultsStageInfo>();

        // Constructor
        private ResultsUI()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        void Awake()
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
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // If the results manager isn't set, get the instance.
            if(resultsManager == null)
            {
                resultsManager = ResultsManager.Instance;
            }

            // If the tutorial UI exists (which it shouldn't since the game is over), destroy it.
            if(TutorialUI.Instantiated)
            {
                Destroy(TutorialUI.Instance.gameObject);
            }
        }

        // Gets the instance.
        public static ResultsUI Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<ResultsUI>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Results UI (singleton)");
                        instance = go.AddComponent<ResultsUI>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Applies the results data for the UI.
        public void ApplyResultsData(DataLogger dataLogger)
        {
            // Game Values
            gameTime.valueText.text = StringFormatter.FormatTime(dataLogger.gameTimer, true, true, false);
            gameScore.valueText.text = Mathf.CeilToInt(dataLogger.gameScore).ToString();
            gameEnergyTotal.valueText.text = Mathf.CeilToInt(dataLogger.GetWorldStageDatasEnergyTotal()).ToString();

            // Stage Infos
            for (int i = 0; i < resultsStageInfos.Count && i < dataLogger.worldStageDatas.Length; i++)
            {
                resultsStageInfos[i].ApplyWorldStageData(dataLogger.worldStageDatas[i]);
            }
        }

        // Clear the results data.
        public void ClearResultsData()
        {
            // Game Values
            gameTime.valueText.text = "-";
            gameScore.valueText.text = "-";
            gameEnergyTotal.valueText.text = "-";

            // Stage Infos
            for (int i = 0; i < resultsStageInfos.Count; i++)
            {
                resultsStageInfos[i].ClearWorldStageData();
            }
        }

        // Goes to the title scene.
        public void ToTitleScene()
        {
            resultsManager.ToTitleScene();
        }

        // Call this function to complete the game. This is called by the "finish" button.
        public void CompleteGame()
        {
            // The SDK has been initialized.
            if (LOLSDK.Instance.IsInitialized)
            {
                // Complete the game.
                LOLSDK.Instance.CompleteGame();
            }
            else
            {
                // Logs the error.
                Debug.LogError("SDK NOT INITIALIZED. RETURNING TO THE TITLE SCREEN.");

                // Return to the main menu scene.
                ToTitleScene();
            }

            // Do not return to the title scene if running through the LOL platform.
            // This is because you can't have the game get repeated in the same session.
            // ToTitleScene();
        }


        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }

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