using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The action stage unit.
    public class ActionUnit : MonoBehaviour
    {
        // The action manager.
        public ActionManager actionManager;

        // The ID number of the action unit.
        public int idNumber = 0;

        // The sprite renderer.
        public SpriteRenderer spriteRenderer;

        // The animator.
        public Animator animator;

        // Start is called before the first frame update
        virtual protected void Start()
        {
            // Gets the action manager instance.
            if (actionManager == null)
                actionManager = ActionManager.Instance;
        }

        // Update is called once per frame
        virtual protected void Update()
        {

        }
    }
}