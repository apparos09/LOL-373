using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The post processor for the action stage.
    public class ActionPostProcessor : util.PostProcessor
    {
        [Header("Action")]
        // Color Grade Night ID
        public string colorGradeNightId = "_ColorGradeNight";

        // The colour grade for night (RGB).
        public Texture2D colorGradeNight;

        // The lerp T id.
        public string lerpTId = "_LerpT";

        // The lerp value between the day and night textures.
        public float lerpT = 0.0F;

        // Awake is called when the script instance is being loaded
        protected override void Awake()
        {
            base.Awake();

            // Set the textures to the material.
            SetValuesToMaterial();
        }

        // OnRenderImage is called after all rendering is complete to render image
        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // Needs to be set every frame for some reason.
            SetValuesToMaterial();

            base.OnRenderImage(source, destination);
        }

        // Sets the textures to be used for color grading.
        public void SetValuesToMaterial()
        {
            postMaterial.SetTexture(colorGradeNightId, colorGradeNight);
            postMaterial.SetFloat(lerpTId, lerpT);
        }
    }
}