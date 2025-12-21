using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The user action player.
    public class ActionPlayerUser : ActionPlayer
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the player user is null, set it to this.
            if(actionManager.playerUser == null)
            {
                actionManager.playerUser = this;
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}
