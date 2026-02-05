using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RM_EDU
{
    // The world stage select dialog..
    public class WorldStageSelectDialog : MonoBehaviour
    {
        // The world UI.
        public WorldUI worldUI;

        // The world stage the prompt is for.
        private WorldStage worldStage;

        [Header("UI")]

        // The stage type text.
        public TMP_Text stageTypeText;

        // The resources text.
        public TMP_Text resourcesText;

        // Stage Name Keys
        public const string WORLD_STAGE_KEY = "kwd_worldStage";
        public const string ACTION_STAGE_KEY = "kwd_actionStage";
        public const string KNOWLEDGE_STAGE_KEY = "kwd_knowledgeStage";
        public const string UNKNOWN_STAGE_KEY = "kwd_unknownStage";

        // Start is called before the first frame update
        void Start()
        {
            // Sets the world UI.
            if(worldUI == null)
            {
                worldUI = WorldUI.Instance;
            }
        }

        // Gets the stage name translated. Returns empty string if LOLSDK isn't initialized or the key is blank.
        private string GetStageNameTranslated(string key)
        {
            // Checks if the LOL SDK is initialized.
            if (LOLManager.IsLOLSDKInitialized() && key != "")
            {
                return LOLManager.GetLanguageTextStatic(key);
            }
            else
            {
                return "";
            }
        }

        // Gets the world stage string.
        public string GetWorldStageString()
        {
            if (LOLManager.IsLOLSDKInitialized())
                return GetStageNameTranslated(WORLD_STAGE_KEY);
            else
                return "World Stage";
        }

        // Gets the action stage string.
        public string GetActionStageString()
        {
            if (LOLManager.IsLOLSDKInitialized())
                return GetStageNameTranslated(ACTION_STAGE_KEY);
            else
                return "Action Stage";
        }

        // Gets the knowledge stage string.
        public string GetKnowledgeStageString()
        {
            if (LOLManager.IsLOLSDKInitialized())
                return GetStageNameTranslated(KNOWLEDGE_STAGE_KEY);
            else
                return "Knowledge Stage";
        }

        // Gets the unknown stage string.
        public string GetUnknownStageString()
        {
            if (LOLManager.IsLOLSDKInitialized())
                return GetStageNameTranslated(UNKNOWN_STAGE_KEY);
            else
                return "Unknown Stage";
        }

        // Returns 'true' if the world stage.
        public bool HasWorldStage()
        {
            return worldStage != null;
        }


        // Sets the world stage.
        public void SetWorldStage(WorldStage worldStage)
        {
            this.worldStage = worldStage;

            // Checks if the world stage is an action stage.
            if(worldStage is WorldActionStage)
            {
                stageTypeText.text = GetActionStageString();
            }
            // Checks if a world stage is a knowledge stage.
            else if(worldStage is WorldKnowledgeStage)
            {
                stageTypeText.text = GetKnowledgeStageString();
            }
            // Don't know what stage it is., so set it as unknown.
            else
            {
                stageTypeText.text = GetUnknownStageString();
            }

            // If the world stage exists, get the resources as a string.
            if(worldStage != null)
            {
                resourcesText.text = worldStage.ResourcesToString();
            }
            else
            {
                resourcesText.text = "-";
            }
        }

        // Clears the world stage.
        public void ClearWorldStage()
        {
            worldStage = null;
            stageTypeText.text = GetWorldStageString();
            resourcesText.text = "-";
        }

        // Called to start the stage.
        public void StartStage()
        {
            // If the world stage isn't equal to null, start the stage.
            if(worldStage != null)
            {
                WorldManager.Instance.StartStage(worldStage);
            }
            
        }

        // Opens the dialog using the current world stage by calling the world UI.
        public void OpenDialog()
        {
            // Opens the world stage dialog.
            WorldUI.Instance.OpenWorldStageSelectDialog(worldStage);
        }

        // Opens the dialog given the provided world stage.
        public void OpenDialog(WorldStage worldStage)
        {
            WorldUI.Instance.OpenWorldStageSelectDialog(worldStage);
        }

        // Closes the dialog.
        public void CloseDialog()
        {
            ClearWorldStage();
            WorldUI.Instance.CloseWorldStageSelectDialog();
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}