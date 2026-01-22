using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

            // The energy total for the stage.
            public float energyTotal = 0;

            // Marks if the stage is complete.
            public bool complete = false;
        }

        // Enum for the world stage type.
        public enum stageType { unknown, action, knowledge };

        // The world manager.
        public WorldManager worldManager;

        // The sprite renderer for the stage light.
        public SpriteRenderer lightSpriteRenderer;

        // The sprite for the light when it's on.
        [Tooltip("The sprite for when the light is on (bright light).")]
        public Sprite lightOnSprite;

        // The sprite for the light when it's off.
        [Tooltip("The sprite for when the light is off (dimmed light).")]
        public Sprite lightOffSprite;

        // The sprite for when there's no light at all (black).
        [Tooltip("The sprite for when there's no light at all (black).")]
        public Sprite lightNoneSprite;

        // The collider for the world stage.
        public new Collider2D collider;


        [Header("World Stage/Info")]
        // The stage's ID number.
        public int idNumber = 0;

        // The stage's difficulty.
        public int difficulty = 0;

        // The time the stage took (in seconds).
        public float time = 0;

        // The score for this stage.
        public float score = 0;

        // The total amount of energy generated for this stage.
        [Tooltip("The total amount of energy generated in this stage.")]
        public float energyTotal = 0;

        // Checks if the stage is locked. This isn't included in the stage data.
        [Tooltip("If 'true', the stage is locked and cannot be selected.")]
        public bool locked = false;

        // Marker for if the stage is complete.
        [Tooltip("If true, the stage is complete, and cannot be selected.")]
        public bool complete = false;

        [Header("World Stage/Resources")]
        // The natural resources the stage uses.
        [Tooltip("The natural resources for the stage.")]
        public List<NaturalResources.naturalResource> naturalResources = new List<NaturalResources.naturalResource>();

        // The defense units that are unlocked if the player beats the stage.
        [Tooltip("The ids for defense units that're unlocked if the player completes the stage.")]
        public List<int> defenseIdRewards = new List<int>();

        [Header("World Stage/Events")]

        // The stage's unlock event.
        public StageUnlockEvent stageUnlockEvent;

        // The stage's complete event.
        public StageCompleteEvent stageCompleteEvent;


        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the collider has not been set, try grabbing the collider.
            if(collider == null)
                collider = GetComponent<Collider2D>();

            // Gets the world manager.
            if (worldManager == null)
                worldManager = WorldManager.Instance;

            // If the stage unlock event isn't set, try to get it.
            if(stageUnlockEvent == null)
                stageUnlockEvent = GetComponent<StageUnlockEvent>();

            // If the stage complete event isn't set, try to get it.
            if(stageCompleteEvent == null)
                stageCompleteEvent = GetComponent<StageCompleteEvent>();
        }

        // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
        protected virtual void OnMouseDown()
        {
            // If the pointer is over a game object in the event system, don't take in inputs.
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            // If the stage is not complete and isn't locked, open the stage dialog.
            if(!complete && !locked)
            {
                WorldManager.Instance.worldUI.OpenWorldStageDialog(this);
            }
        }


        // LIGHT SPRITE //
        // Sets the light sprite to on sprite (true) or off sprite (false).
        // <= -1 = off, 0 = none, >= 1 = on.
        public void SetLightSprite(int value)
        {
            // The value is negative, so switch it off.
            if(value < 0)
            {
                lightSpriteRenderer.sprite = lightOffSprite;
            }
            // The value is positive, so switch it on.
            else if(value > 0)
            {
                lightSpriteRenderer.sprite = lightOnSprite;
            }
            // No value, so set to none.
            else
            {
                lightSpriteRenderer.sprite = lightNoneSprite;
            }

        }

        // Returns 'true' if the light sprite is the on sprite.
        public bool IsLightSpriteOn()
        {
            return lightSpriteRenderer.sprite == lightOnSprite;
        }

        // Sets the light sprite to the on sprite.
        public void SetLightSpriteToOnSprite()
        {
            SetLightSprite(1);
        }

        // Returns 'true' if the light sprite is the off sprite.
        public bool IsLightSpriteOff()
        {
            return lightSpriteRenderer.sprite == lightOffSprite;
        }

        // Sets the light sprite to the off sprite.
        public void SetLightSpriteToOffSprite()
        {
            SetLightSprite(-1);
        }

        // Returns 'true' if the light sprite is none.
        public bool IsLightSpriteNone()
        {
            return lightSpriteRenderer.sprite == lightNoneSprite;
        }

        // Sets the light sprite to none.
        public void SetLightSpriteToNoneSprite()
        {
            SetLightSprite(0);
        }

        // REWARDS //

        // Gives the player their rewards.
        public void GivePlayerRewards()
        {
            GiveDefenseUnitsToPlayer();
        }

        // Gives the player the defense units for this world stage.
        public void GiveDefenseUnitsToPlayer()
        {
            // If there are defense id rewards to give the player.
            if(defenseIdRewards.Count > 0)
            {
                // The data logger.
                DataLogger dataLogger = DataLogger.Instance;

                // Adds action defense units to the data logger.
                dataLogger.AddActionDefenseUnits(defenseIdRewards);
            }
        }


        // STAGE INFO //
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

        // LOCK
        // Returns 'true' if the stage is locked.
        public bool IsLocked()
        {
            return locked;
        }

        // Sets if the stage is locked.
        public void SetLocked(bool stageLocked)
        {
            // Set value.
            locked = stageLocked;

            // Checks if the stage is locked.
            if(locked)
            {
                // Now locked, so set the sprite to off.
                SetLightSpriteToOffSprite();
            }
            // Stage is unlocked.
            else
            {
                // Chooses sprite based on if the stage is complete or not.
                if(complete)
                {
                    // Light off if complete.
                    SetLightSpriteToOffSprite();
                }
                else
                {
                    // Light on if compelte.
                    SetLightSpriteToOnSprite();
                }
            }
        }

        // Locks the stage.
        public void LockStage()
        {
            SetLocked(true);
        }

        // Unlocks the stage.
        public void UnlockStage()
        {
            SetLocked(false);
        }


        // COMPLETE
        // Returns 'true' if the stage is complete.
        public bool IsComplete()
        {
            return complete;
        }

        // Sets that the stage is complete.
        public void SetComplete(bool stageComplete)
        {
            complete = stageComplete;

            // If the stage is now complete.
            if(complete)
            {
                // If the stage is locked, use the off sprite.
                if (locked)
                {
                    SetLightSpriteToNoneSprite();
                }
                // Set the light sprite to off.
                else
                {
                    SetLightSpriteToOffSprite();
                }


                // Give rewards since the stage is complete.
                GivePlayerRewards();
            }
            // If the stage is not complete.
            else
            {
                // If the stage is locked, turn the light off.
                if(locked)
                {
                    SetLightSpriteToNoneSprite();
                }
                // Stage is unlocked, but not complete, so set light to on sprite.
                else
                {
                    SetLightSpriteToOnSprite();
                }
            }
        }

        // Refreshes the complete parameter by calling set complete.
        public void RefreshComplete()
        {
            SetComplete(complete);
        }

        // Generates the data.
        public WorldStageData GenerateWorldStageData()
        {
            // The data being generated.
            WorldStageData data = new WorldStageData();

            // The values returned.
            data.worldStageIndex = GetWorldStageIndex();
            data.idNumber = idNumber;
            data.stageType = GetStageType();
            data.time = time;
            data.score = score;
            data.energyTotal = energyTotal;
            data.complete = complete;

            return data;
        }

        // Applies world stage data.
        public void ApplyWorldStageData(WorldStageData data)
        {
            // If the world index, id number, or stage type don't match, display a message.
            if(data.worldStageIndex != GetWorldStageIndex() || idNumber != data.idNumber || data.stageType != GetStageType())
            {
                Debug.LogWarning("The id number or stage type doesn't match. Id number changed to match data.");
            }
            
            // Set the values.
            idNumber = data.idNumber;

            time = data.time;
            score = data.score;
            energyTotal = data.energyTotal;

            complete = data.complete;
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }
}