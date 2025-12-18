using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace RM_EDU
{
    // The gameplay manager.
    public class GameplayManager : MonoBehaviour
    {
        // The game UI.
        public GameplayUI gameUI;

        // The game speed.
        // This is only used for the Action Stages.
        private float gameTimeScale = 1.0F;

        // The timer for the game.
        public float gameTimer = 0;

        // The game score.
        public float gameScore = 0;

        // Pauses the timer if true.
        private bool gamePaused = false;

        // The mouse touch object.
        public MouseTouchInput mouseTouch;

        // The tutorials object.
        public Tutorials tutorials;

        // The world scene.
        public string worldScene = "WorldScene";

        // The action scene.
        public string actionScene = "ActionScene";

        // The knowledge scene.
        public string knowledgeScene = "KnowledgeScene";

        // If 'true', the loading screen is enabled.
        public bool loadingScreenEnabled = true;

        // The data logger.
        public DataLogger dataLogger;

        // If 'true', the data logger is used saving and loading data.
        public bool useDataLogger = true;

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // ...
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the game UI isn't set, try to find it.
            if (gameUI == null)
                gameUI = FindObjectOfType<GameplayUI>();

            // If the data logger isn't set, get the instance.
            if (dataLogger == null)
                dataLogger = DataLogger.Instance;

            // TODO: add in when tutorial is created.
            // Sets the tutorials object.
            // if (tutorials == null)
            //     tutorials = Tutorials.Instance;
            // 
            // 
            // // If the gameUI is set, check for the tutorial text box.
            // if (gameUI != null)
            // {
            //     // If the tutorial is set.
            //     if(gameUI.tutorialUI != null)
            //     {
            //         // If the tutorial text box is set...
            //         if (gameUI.tutorialUI.textBox != null)
            //         {
            //             // Adds the callbakcs from the tutorial text box.
            //             // I don't think I need to remove them.
            //             gameUI.AddTutorialTextBoxCallbacks(this);
            //         }
            //     }
            // }
        }


        // GAME TIME SCALE AND PAUSING //
        // Gets the game time scale.
        public float GetGameTimeScale()
        {
            return gameTimeScale;
        }

        // Sets the game time scale.
        public void SetGameTimeScale(float value)
        {
            gameTimeScale = value;
            Time.timeScale = gameTimeScale;
        }

        // Resets the time scale.
        public void ResetGameTimeScale()
        {
            gameTimeScale = 1.0F;
            Time.timeScale = gameTimeScale;
        }

        // Returns 'true' if the game time scale is 1.0.
        public bool IsGameTimeScaleNormal()
        {
            return gameTimeScale == 1.0F;
        }

        // Returns 'true' if the game is paused.
        public bool IsGamePaused()
        {
            return gamePaused;
        }

        // Sets if the game should be paused.
        public virtual void SetGamePaused(bool paused)
        {
            gamePaused = paused;

            // If the game is paused.
            if (gamePaused)
            {
                // NOTE: this does not change the time scale.
                Time.timeScale = 0.0F;
            }
            else // If the game is not paused.
            {
                // If the tutorial is not running, set the time scale to 1.0F.
                if (!IsTutorialRunning())
                {
                    // Gets the game time scale.
                    float gts = GetGameTimeScale();

                    // If the time scale is 0, reset the time scale.
                    if (gts == 0)
                    {
                        ResetGameTimeScale();
                        gts = GetGameTimeScale();
                    }

                    // Set the game time scale.
                    Time.timeScale = gts;
                }
            }
        }

        // Pauses the game.
        public virtual void PauseGame()
        {
            SetGamePaused(true);
        }

        // Unpauses the game.
        public virtual void UnpauseGame()
        {
            SetGamePaused(false);
        }

        // Toggles if the game is paused or not.
        public virtual void TogglePausedGame()
        {
            SetGamePaused(!gamePaused);
        }


        // TUTORIAL //
        // Checks if the game is using the tutorial.
        public bool IsUsingTutorial()
        {
            // The result.
            bool result;

            // If the game settings is instantiated.
            if (GameSettings.Instantiated)
            {
                result = GameSettings.Instance.UseTutorial;
            }
            else
            {
                // Not instantiated, so return false by default.
                result = false;
            }


            return result;
        }

        // Set if the tutorial will be used.
        public void SetUsingTutorial(bool value)
        {
            GameSettings.Instance.UseTutorial = value;
        }

        // Returns 'true' if the tutorial is available to be activated.
        public bool IsTutorialAvailable()
        {
            return gameUI.IsTutorialAvailable();
        }

        // Checks if the text box is open.
        public bool IsTutorialTextBoxOpen()
        {
            return gameUI.IsTutorialTextBoxOpen();
        }

        // Checks if the tutorial is running.
        public bool IsTutorialRunning()
        {
            // Check this function.
            return gameUI.IsTutorialRunning();

        }

        // Starts a tutorial using the provided pages.
        public virtual void StartTutorial(List<Page> pages)
        {
            gameUI.StartTutorial(pages);
        }

        // Called when a tutorial is started.
        public virtual void OnTutorialStart()
        {
            gameUI.OnTutorialStart();
        }

        // Called when a tutorial is ended.
        public virtual void OnTutorialEnd()
        {
            gameUI.OnTutorialEnd();
        }

        // SCENES
        // Loads the scene and cheks the loading screen member variable to see if the loading screen should be used.
        public virtual void LoadScene(string scene)
        {
            LoadScene(scene, loadingScreenEnabled);
        }

        // Loads a scene, which checks if a loading screen should be used.
        public virtual void LoadScene(string scene, bool useLoadingScreen)
        {
            SceneManager.LoadScene(scene);

            // TODO: implement loading screen.
        }

        // Loads the world scene and checks a member function for using the loading screen.
        public virtual void LoadWorldScene()
        {
            LoadScene(worldScene, loadingScreenEnabled);
        }

        // Goes to the world scene.
        public virtual void LoadWorldScene(bool useLoadingScreen)
        {
            LoadScene(worldScene, useLoadingScreen);
        }

        // Loads the action scene and checks a member function for using the loading screen.
        public virtual void LoadActionScene()
        {
            LoadScene(actionScene, loadingScreenEnabled);
        }

        // Goes to the action scene.
        public virtual void LoadActionScene(bool useLoadingScreen)
        {
            LoadScene(actionScene, useLoadingScreen);
        }

        // Loads the knowledge scene. Uses a loading screen based on a class variable.
        public virtual void LoadKnowledgeScene()
        {
            LoadScene(knowledgeScene, loadingScreenEnabled);
        }

        // Goes to the knowledge scene and has the user set if the loading screen should be used.
        public virtual void LoadKnowledgeScene(bool useLoadingScreen)
        {
            LoadScene(knowledgeScene, useLoadingScreen);
        }


        // Update is called once per frame
        protected virtual void Update()
        {
            // The game isn't paused, add to the game time.
            if (!IsGamePaused())
            {
                gameTimer += Time.unscaledDeltaTime;
            }
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // Resets the game time scale.
            ResetGameTimeScale();

            // Return the time scale to normal.
            Time.timeScale = 1.0F;
        }
    }
}