using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The world stage.
    public abstract class WorldStage : MonoBehaviour
    {
        // The collider for the world stage.
        public new Collider2D collider;

        // The world manager.
        public WorldManager worldManager;

        // The natural resources the stage uses.
        public List<NaturalResources.naturalResource> naturalResources = new List<NaturalResources.naturalResource>();

        // The stage's difficulty.
        public int difficulty = 0;

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
            worldManager.worldUI.OpenStagePrompt(this);
        }

        // TODO: add function to check when the object is enabled to refersh text.

        // Returns 'true' if this is an action stage.
        public bool IsActionStage()
        {
            return this is WorldActionStage;
        }

        // Returns 'true' if this is a knowledge stage.
        public bool IsKnowledgeStage()
        {
            return this is WorldKnowledgeStage;
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }
}