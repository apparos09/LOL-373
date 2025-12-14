using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The start info for the knowledge stage.
    public class KnowledgeStageStartInfo : GameplayStartInfo
    {
        [Header("Knowledge")]

        // The statements in the knowledge stage.
        public List<KnowledgeStatement> statements = new List<KnowledgeStatement>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Applies the start info to the provided manager.
        public override void ApplyStartInfo(GameplayManager manager)
        {
            // If the game manager is a knowledge manager, convert it and send it to the appropriate function.
            if(manager is KnowledgeManager)
            {
                ApplyStartInfo(manager as KnowledgeManager);
            }
        }

        // Applies the start info to the knowledge manager.
        public void ApplyStartInfo(KnowledgeManager manager)
        {
            // TODO: implement.
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}