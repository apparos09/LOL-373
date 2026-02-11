using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Audio for the world scene.
    public class WorldAudio : EDU_GameAudio
    {
        [Header("World")]

        // The world bgm.
        public AudioClip worldBgm;

        // The start of the world BGM loop.
        public float worldBgmClipStart;

        // The ned of the world BGM loop.
        public float worldBgmClipEnd;

        // If 'true', the bgm is played in start.
        public bool playBgmInStart = true;


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Clamp the start and end values.
            worldBgmClipStart = Mathf.Clamp(worldBgmClipStart, 0.0F, worldBgm.length);
            worldBgmClipEnd = Mathf.Clamp(worldBgmClipEnd, 0.0F, worldBgm.length);

            // If the BGM should be played in start, play it.
            if (playBgmInStart)
            {
                PlayWorldBgm();
            }
        }

        // Plays the world BGM.
        public void PlayWorldBgm()
        {
            PlayBackgroundMusic(worldBgm, worldBgmClipStart, worldBgmClipEnd);
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}