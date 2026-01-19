using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RM_EDU
{
    // The world stage prompt.
    public class WorldStageDialog : MonoBehaviour
    {
        // The world UI.
        public WorldUI worldUI;

        // The world stage the prompt is for.
        private WorldStage worldStage;

        [Header("UI")]

        // The title text object.
        public TMP_Text titleText;

        // Strings
        // World Stage
        private string worldStageStr = "World Stage";
        private string worldStageKey = "kwd_worldStage";

        // Action Stage
        private string actionStageStr = "Action Stage";
        private string actionStageKey = "kwd_actionStage";

        // Knowledge Stage
        private string knowledgeStageStr = "Knowledge Stage";
        private string knowledgeStageKey = "kwd_knowledgeStage";

        // Unknown
        private string unknownStageStr = "Unknown Stage";
        private string unknownStageKey = "kwd_unknownStage";

        // Start is called before the first frame update
        void Start()
        {
            // Sets the world UI.
            if(worldUI == null)
            {
                worldUI = WorldUI.Instance;
            }

            // If the LOL SDK is initialized, translate the strings.
            if(LOLManager.Instantiated && LOLManager.IsLOLSDKInitialized())
            {
                worldStageStr = LOLManager.Instance.GetLanguageText(worldStageKey);
                actionStageStr = LOLManager.Instance.GetLanguageText(actionStageKey);
                knowledgeStageStr = LOLManager.Instance.GetLanguageText(knowledgeStageKey);
                unknownStageStr = LOLManager.Instance.GetLanguageText(unknownStageKey);
            }
        }

        // Sets the world stage.
        public void SetWorldStage(WorldStage worldStage)
        {
            this.worldStage = worldStage;

            // Checks if the world stage is an action stage.
            if(worldStage is WorldActionStage)
            {
                titleText.text = actionStageStr;
            }
            // Checks if a world stage is a knowledge stage.
            else if(worldStage is WorldKnowledgeStage)
            {
                titleText.text = knowledgeStageStr;
            }
            // Don't know what stage it is.
            else
            {
                titleText.text = unknownStageStr;
            }

        }

        // Sets the world stage, and asks if the prompt should be opened.
        public void SetWorldStage(WorldStage worldStage, bool openPrompt)
        {
            // Sets the world stage.
            SetWorldStage(worldStage);

            // TODO: open prompt.
        }

        // Clears the world stage.
        public void ClearWorldStage()
        {
            worldStage = null;
        }

        // Called to start the stage.
        public void StartStage()
        {
            // If the world stage isn't equal to null, start the stage.
            if(worldStage != null)
            {
                worldUI.worldManager.StartStage(worldStage);
            }
            
        }

        // Back out of the prompt.
        public void ClosePrompt()
        {
            ClearWorldStage();
            worldUI.CloseStageDialog();
            // TODO: implement.
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}