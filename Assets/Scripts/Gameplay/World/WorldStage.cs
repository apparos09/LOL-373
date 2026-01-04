using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The world stage.
    public abstract class WorldStage : MonoBehaviour
    {
        // The world stage data.
        [SerializeField]
        public class WorldStageData
        {
            // The world index of the stage.
            public int worldStageIndex = -1;

            // The id number of the stage.
            public int idNumber = 0;

            // The stage type.
            public stageType stageType;

            // The amount of time the stage took (in seconds).
            public float time = 0;

            // The stage score.
            public float score = 0;

            // Marks if the stage is complete.
            public bool complete = false;
        }

        // Enum for the world stage type.
        public enum stageType { unknown, action, knowledge };

        // The collider for the world stage.
        public new Collider2D collider;

        // The world manager.
        public WorldManager worldManager;

        // The natural resources the stage uses.
        public List<NaturalResources.naturalResource> naturalResources = new List<NaturalResources.naturalResource>();

        // The stage's ID number.
        public int idNumber = 0;

        // The stage's difficulty.
        public int difficulty = 0;

        // The time the stage took (in seconds).
        public float time = 0;

        // The score for this stage.
        public float score = 0;

        // Marker for if the stage is complete.
        public bool complete = false;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the collider has not been set, try grabbing the collider.
            if(collider == null)
                collider = GetComponent<Collider2D>();

            // Gets the world manager.
            if (worldManager == null)
                worldManager = WorldManager.Instance;
        }

        // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
        protected virtual void OnMouseDown()
        {
            WorldManager.Instance.worldUI.OpenStagePrompt(this);
        }

        // TODO: add function to check when the object is enabled to refersh text.
        // Returns the index of this stage in the world manager. If -1 is returned, it's not in the world manager stage list.
        public int GetWorldStageIndex()
        {
            return WorldManager.Instance.GetWorldStageIndex(this);
        }

        // Gets the world area this stage is in.
        public WorldArea GetWorldStageArea()
        {
            return WorldManager.Instance.GetWorldStageArea(this);
        }

        // Gets the stage type.
        public virtual stageType GetStageType()
        {
            return stageType.unknown;
        }

        // Returns 'true' if the stage is complete.
        public bool IsComplete()
        {
            return complete;
        }

        // Generates the data.
        public WorldStageData GenerateWorldStageData()
        {
            // 
            WorldStageData data = new WorldStageData();

            data.worldStageIndex = GetWorldStageIndex();
            data.idNumber = idNumber;
            data.stageType = GetStageType();
            data.time = time;
            data.score = score;
            data.complete = complete;

            return data;
        }

        // Applies world stage data.
        public void ApplyWorldStageData(WorldStageData data)
        {
            // If the world index, id number, or stage type don't match, display a message.
            if(data.worldStageIndex != GetWorldStageIndex() || idNumber != data.idNumber || data.stageType != GetStageType())
            {
                Debug.LogAssertion("The ID number or id number don't match. Id number changed to match data.");
            }
            
            // Set the values.
            idNumber = data.idNumber;
            time = data.time;
            score = data.score;
            complete = data.complete;
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }
}