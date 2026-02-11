using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The audio for the results screen.
    public class ResultsAudio : EDU_GameAudio
    {
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
    }
}