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

        // TODO: play the animations directly instead of citing action unit?

        // Plays the empty state animation.
        public void PlayEmptyStateAnimation(int layer)
        {
            actionUnit.PlayEmptyStateAnimation(layer);
        }

        // Plays the empty state animation on the base layer.
        public virtual void PlayEmptyStateAnimationBaseLayer()
        {
            actionUnit.PlayEmptyStateAnimationBaseLayer();
        }

        // Plays the empty state animation on the overlay layer.
        public virtual void PlayEmptyStateAnimationOverlayLayer()
        {
            actionUnit.PlayEmptyStateAnimationOverlayLayer();
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}