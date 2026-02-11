using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Audio for the title area.
    public class TitleAudio : EDU_GameAudio
    {
        [Header("Title")]

        // The title bgm.
        public AudioClip titleBgm;

        // The start of the title BGM loop.
        public float titleBgmClipStart;
        
        // The ned of the title BGM loop.
        public float titleBgmClipEnd;

        // If 'true', the bgm is played in start.
        public bool playBgmInStart = true;


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Clamp the start and end values.
            titleBgmClipStart = Mathf.Clamp(titleBgmClipStart, 0.0F, titleBgm.length);
            titleBgmClipEnd = Mathf.Clamp(titleBgmClipEnd, 0.0F, titleBgm.length);
        
            // If the BGM should be played in start, play it.
            if(playBgmInStart)
            {
                PlayTitleBgm();
            }
        }

        // Plays the title BGM.
        public void PlayTitleBgm()
        {
            PlayBackgroundMusic(titleBgm, titleBgmClipStart, titleBgmClipEnd);
        }
        
        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}