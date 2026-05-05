using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // A notification that shows up in action stages.
    public class ActionStageNotification : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The background image of the notification.
        public Image backgroundImage;

        // The notification text.
        public TMP_Text text;

        // The TMP text translator.
        public TMP_TextTranslator textTranslator;

        [Header("Animation")]
        // The animator.
        public Animator animator;

        // The empty state animation.
        public string emptyStateAnim = "Empty State";

        // The blink animation.
        public string blinkAnim = "Action Stage Notification - Blinking Animation";

        // Start is called before the first frame update
        void Start()
        {
            // Gets the action UI instance.
            if (actionUI == null)
                actionUI = ActionUI.Instance;
        }

        // Animations
        // Plays an animation.
        public void PlayAnimation(string animationName)
        {
            // This doesn't check for an empty animation since this script...
            // Only works when tied to animations.
            animator.Play(animationName);
        }

        // Plays the empty state animation.
        public void PlayEmptyStateAnimation()
        {
            PlayAnimation(emptyStateAnim);
        }

        // Plays the blink animation.
        public void PlayBlinkingAnimation()
        {
            PlayAnimation(blinkAnim);
        }

        // Called when the blink animation begins.
        public void OnBlinkAnimationStart()
        {
            // ...
        }

        // Called when the blink animation ends.
        public void OnBlinkAnimationEnd()
        {
            PlayEmptyStateAnimation();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}