using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace RM_EDU
{
    // The results manager.
    public class ResultsManager : MonoBehaviour
    {
        // The singleton instance.
        private static ResultsManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The results UI.
        public ResultsUI resultsUI;

        // The results audio.
        public ResultsAudio resultsAudio;

        // The title scene.
        public string titleScene = "TitleScene";

        // Constructor
        private ResultsManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
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
            // TODO: implement results data.

            // // Looks for the result data.
            // ResultsData data = FindObjectOfType<ResultsData>();
            // 
            // // Applies the results data.
            // if (data != null)
            // {
            //     // Applies the data.
            //     ApplyResultsData(data);
            // 
            //     // Destroys the object.
            //     Destroy(data.gameObject);
            // }

        }

        // Gets the instance.
        public static ResultsManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<ResultsManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Results Manager (singleton)");
                        instance = go.AddComponent<ResultsManager>();
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

        // // Applies the results data.
        // public void ApplyResultsData(ResultsData data)
        // {
        //     resultsUI.ApplyResultsData(data);
        // }
        // 
        // // Goes to the title scene.
        // public void ToTitleScene()
        // {
        //     // If the loading screen is being used.
        //     if (LoadingScreenCanvas.Instance.IsUsingLoadingScreen())
        //     {
        //         LoadingScreenCanvas.Instance.LoadScene(titleScene);
        //     }
        //     else
        //     {
        //         SceneManager.LoadScene(titleScene);
        //     }
        // }

        // Update is called once per frame
        void Update()
        {

        }
    }
}