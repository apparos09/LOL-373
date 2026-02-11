using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The audio for the results screen.
    public class ResultsAudio : EDU_GameAudio
    {
        // The singleton instance.
        private static ResultsAudio instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;


        [Header("Results")]

        // The results bgm.
        public AudioClip resultsBgm;

        // The start of the results BGM loop.
        public float resultsBgmClipStart;

        // The ned of the results BGM loop.
        public float resultsBgmClipEnd;

        // If 'true', the bgm is played in start.
        public bool playBgmInStart = true;


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Clamp the start and end values.
            resultsBgmClipStart = Mathf.Clamp(resultsBgmClipStart, 0.0F, resultsBgm.length);
            resultsBgmClipEnd = Mathf.Clamp(resultsBgmClipEnd, 0.0F, resultsBgm.length);

            // If the BGM should be played in start, play it.
            if (playBgmInStart)
            {
                PlayResultsBgm();
            }
        }

        // Gets the instance.
        public static ResultsAudio Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<ResultsAudio>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("ResultsAudio (singleton)");
                        instance = go.AddComponent<ResultsAudio>();
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

        // Plays the results BGM.
        public void PlayResultsBgm()
        {
            PlayBackgroundMusic(resultsBgm, resultsBgmClipStart, resultsBgmClipEnd);
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