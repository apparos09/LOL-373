using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // The EDU game audio.
    public class EDU_GameAudio : GameAudio
    {
        [Header("EDU")]

        // The button (UI) SFX.
        public AudioClip buttonUISfx;

        // The slider (UI) SFX.
        public AudioClip sliderUISfx;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // // Update is called once per frame
        // protected override void Update()
        // {
        //     base.upda
        // }
    }
}