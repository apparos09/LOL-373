using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Audio for stages.
    public class StageAudio : EDU_GameAudio
    {
        [Header("Stage")]

        // THEME 2 (NORMAL BGM)
        // The stage 01 bgm.
        public AudioClip stageBgm01;

        // The start of the stage 01 bgm loop.
        public float stageBgm01ClipStart;

        // The ned of the stage 01 bgm loop.
        public float stageBgm01ClipEnd;

        // THEME 2 (FINAL BGM)
        // The stage 02 bgm.
        public AudioClip stageBgm02;

        // The start of the stage 02 bgm loop.
        public float stageBgm02ClipStart;

        // The ned of the stage 02 bgm loop.
        public float stageBgm02ClipEnd;


        // // Start is called before the first frame update
        // void Start()
        // {
        // 
        // }
        // 

        // Plays the stage BGM based on the number.
        // 0 = Stop
        // 1 = BGM 01
        // 2 = BGM 02
        public void PlayStageBgm(int number)
        {
            // Checks the number to know which stage BGM to play.
            switch(number)
            {
                default:
                case 0:
                    StopBackgroundMusic();
                    break;

                case 1:
                    PlayBackgroundMusic(stageBgm01, stageBgm01ClipStart, stageBgm01ClipEnd);
                    break;

                case 2:
                    PlayBackgroundMusic(stageBgm02, stageBgm02ClipStart, stageBgm02ClipEnd);
                    break;
            }
        }

        // Plays stage bgm 01.
        public void PlayStageBgm01()
        {
            PlayStageBgm(1);
        }

        // Play stage bgm 02.
        public void PlayStageBgm02()
        {
            PlayStageBgm(2);
        }

        // Called when the stage is being reset.
        public void ResetStage()
        {
            // TODO: restart BGM?
            sfxWorldSource.Stop();
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}