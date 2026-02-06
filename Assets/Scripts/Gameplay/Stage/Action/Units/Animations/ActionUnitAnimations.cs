using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Animations for an action unit.
    public class ActionUnitAnimations : MonoBehaviour
    {
        // The action manager.
        public ActionManager actionManager;

        // The animator.
        public Animator animator;

        // The action unit this animations script belongs to.
        public ActionUnit actionUnit;

        [Header("Animations")]

        // The empty state animation.
        public string emptyStateAnim = "Empty State";

        // The explosion animation.
        public string explosionAnim = "Action Unit - Explosion Animation";

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Set manager if null.
            if (actionManager == null)
                actionManager = ActionManager.Instance;

            // Sets the animator if it's not set.
            if(animator == null)
                animator = GetComponent<Animator>();

            // Set action unit if not set.
            if(actionUnit == null)
                actionUnit = GetComponent<ActionUnit>();

        }

        // Plays an animation.
        public void PlayAnimation(string animationName)
        {
            // If animations are enabled, play the animation.
            if (actionUnit.AnimationsEnabled)
            {
                animator.Play(animationName);
            }
        }

        // Plays the provided animation for the given layer.
        public void PlayAnimation(string animationName, int layer)
        {
            // If animations are enabled, play the animation.
            if(actionUnit.AnimationsEnabled)
            {
                animator.Play(animationName, layer);
            } 
        }

        // EMPTY
        // Plays the empty state animation.
        public void PlayEmptyStateAnimation(int layer)
        {
            PlayAnimation(emptyStateAnim, layer);
        }

        // Plays the empty state animation on the base layer.
        public virtual void PlayEmptyStateAnimationBaseLayer()
        {
            PlayEmptyStateAnimation(0);
        }

        // Plays the empty state animation on the overlay layer.
        public virtual void PlayEmptyStateAnimationOverlayLayer()
        {
            PlayEmptyStateAnimation(1);
        }

        // EXPLOSION
        // Plays the explosion animation.
        public void PlayExplosionAnimation()
        {
            animator.Play(explosionAnim);
        }

        // Called when the explosion animation starts.
        public void OnExplosionAnimationStart()
        {
            // ...
        }

        // Called when the explosion animation ends.
        public void OnExplosionAnimationEnd()
        {
            PlayEmptyStateAnimation(1);
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}