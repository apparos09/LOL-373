using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The stage UI.
    public abstract class StageUI : GameplayUI
    {
        // The game will skip these overwritten functions in StageUI and go to GameplayUI instead.
        // Uncomment these functions if actual functionaliy is added.

        // The stage manager.
        public StageManager stageManager;

        // // Awake is called when the script is being loaded
        // protected override void Awake()
        // {
        //     base.Awake();
        // }
        // 
        // // Update is called once per frame
        // protected override void Update()
        // {
        //     base.Update();
        // }
        // 
        // protected override void OnDestroy()
        // {
        //     base.OnDestroy();
        // }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Finds the stage manager if it isn't set.
            if (stageManager == null)
                stageManager = FindObjectOfType<StageManager>();

        }

        // Called to finish the stage.
        // This just calls the appropriate function in the manager.
        public virtual void FinishStage()
        {
            stageManager.FinishStage();
        }

    }
}