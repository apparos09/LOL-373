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
        public float stageBgm01LoopStart;

        // The ned of the stage 01 bgm loop.
        public float stageBgm01LoopEnd;

        // THEME 2 (FINAL BGM)
        // The stage 02 bgm.
        public AudioClip stageBgm02;

        // The start of the stage 02 bgm loop.
        public float stageBgm02LoopStart;

        // The ned of the stage 02 bgm loop.
        public float stageBgm02LoopEnd;

        // The stage results.
        public AudioClip stageResults;

        // Stage results loop start.
        public float stageResultsLoopStart;

        // Stage results loop end.
        public float stageResultsLoopEnd;


        // // Start is called before the first frame update
        // void Start()
        // {
        // 
        // }
        // 

        // Returns the stage BGM count.
        public virtual int GetStageBgmCount()
        {
            return 2;
        }

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
                    PlayBackgroundMusic(stageBgm01, stageBgm01LoopStart, stageBgm01LoopEnd);
                    break;

                case 2:
                    PlayBackgroundMusic(stageBgm02, stageBgm02LoopStart, stageBgm02LoopEnd);
                    break;
            }
        }

        // Returns true if the BGM is set to BGM 01.
        public bool IsStageBgm01Set()
        {
            return bgmSource.clip == stageBgm01;
        }

        // Plays stage bgm 01.
        public void PlayStageBgm01()
        {
            PlayStageBgm(1);
        }

        // Returns true if the BGM is set to BGM 02.
        public bool IsStageBgm02Set()
        {
            return bgmSource.clip == stageBgm02;
        }

        // Play stage bgm 02.
        public void PlayStageBgm02()
        {
            PlayStageBgm(2);
        }

        // Plays the stage results.
        public void PlayStageResultsBgm()
        {
            PlayBackgroundMusic(stageResults, stageResultsLoopStart, stageResultsLoopEnd);
        }

        // Called when the stage is being reset.
        public virtual void ResetStage()
        {
            // NOTE: restarting the BGM is handled in the manage it's relevant to.

            sfxWorldSource.Stop();
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}