using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A defeated enemy. A defeated enemy is spawned when a regular enemy is killed.
    public class ActionUntEnemyDefeated : ActionUnitEnemy
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Since the enemy's been defeated, it shouldn't take damage.
            vulnerable = false;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}