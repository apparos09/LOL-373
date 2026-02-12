using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // Audio for action stages.
    public class ActionAudio : StageAudio
    {
        // The singleton instance.
        private static ActionAudio instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("Action")]

        // The action manager.
        public ActionManager actionManager;

        // The enemy attack audio source (loops).
        public AudioSource enemyAttackSource;

        // The number of calls that have happened for the enemy attack source.
        // Calling play increments this, and calling stop decrements this.
        // When there are no more calls, the source is stopped.
        [Tooltip("Tracks how many play calls have been made for enemy attack. Decreases for every stop call.")]
        public int enemyAttackPlayCalls;

        // Set to true if the enemy attack source is marked as being paused.
        private bool enemyAttackSourceMarkedPaused = false;

        // Constructor
        private ActionAudio()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected override void Awake()
        {
            base.Awake();

            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Sets the instance.
            if (actionManager == null)
            {
                actionManager = ActionManager.Instance;
            }
        }
        

        // Gets the instance.
        public static ActionAudio Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<ActionAudio>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("ActionAudio (singleton)");
                        instance = go.AddComponent<ActionAudio>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Plays the enemy attack.
        public void PlayEnemyAttackSfx()
        {
            // Play the enemy attack if it isn't playing and it isn't paused.
            if(!enemyAttackSource.isPlaying && !enemyAttackSourceMarkedPaused)
            {
                enemyAttackSource.Play();

                // Since it's now playing, it's not paused.
                enemyAttackSourceMarkedPaused = false;
            }

            // Increase the number of calls.
            enemyAttackPlayCalls++;
        }

        // Stops the enemy attack SFX if no one is using it anymore.
        // ignoreCalls: if true, the enemy attack is stopped and calls are set.
        // - If false, the call count is decreased. If there are no more calls, the SFX is stopped.
        public void StopEnemyAttackSfx(bool ignoreCalls)
        {
            // Checks if calls should be ignored.
            if(ignoreCalls)
            {
                // No more calls.
                enemyAttackPlayCalls = 0;

                // Stops the audio source.
                enemyAttackSource.Stop();
                enemyAttackSourceMarkedPaused = false;
            }
            else
            {
                // Reduce calls.
                enemyAttackPlayCalls--;

                // If true, stop the sound.
                if (enemyAttackPlayCalls <= 0)
                {
                    enemyAttackPlayCalls = 0;
                    enemyAttackSource.Stop();
                    enemyAttackSourceMarkedPaused = false;
                }
            }
        }

        // Sets the enemy attack SFX paused.
        public void SetEnemyAttackSfxPaused(bool paused)
        {
            // Pause
            if(paused)
            {
                enemyAttackSource.Pause();
                enemyAttackSourceMarkedPaused = true;
            }
            // Unpause
            else
            {
                // Unpause the enemy attack.
                enemyAttackSource.UnPause();
                enemyAttackSourceMarkedPaused = false;

                // If there aren't any calls, stop the attack sound effect.
                if (enemyAttackPlayCalls <= 0)
                    StopEnemyAttackSfx(true);
            }
        }

        // Pauses the enemy attack SFX.
        public void PauseEnemyAttackSfx()
        {
            SetEnemyAttackSfxPaused(true);
        }

        // Unpauses the enemy attack SFX.
        public void UnpauseEnemyAttackSfx()
        {
            SetEnemyAttackSfxPaused(false);
        }

        // Called when the stage is being reset.
        public override void ResetStage()
        {
            base.ResetStage();

            // Stops the enemy attack audio source.
            StopEnemyAttackSfx(true);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // Safety Check
            // If the enemy attack source is playing.
            if (enemyAttackSource.isPlaying)
            {
                // If the game is paused and the attack source isn't marked as paused.
                if(actionManager.IsGamePaused() && !enemyAttackSourceMarkedPaused)
                {
                    PauseEnemyAttackSfx();
                }

                // No spawned enemies, so stop the attack source.
                if (actionManager.playerEnemy.spawnedEnemies.Count <= 0)
                {
                    StopEnemyAttackSfx(true);
                }
            }
            else
            {
                // If the game isn't paused but the enemy attack source is paused, unpause it.
                if(!actionManager.IsGamePaused() && enemyAttackSourceMarkedPaused)
                {
                    UnpauseEnemyAttackSfx();
                }
            }
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}