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

        // The enemy attack audio source (loops).
        public AudioSource enemyAttackSource;

        // The number of calls that have happened for the enemy attack source.
        // Calling play increments this, and calling stop decrements this.
        // When there are no more calls, the source is stopped.
        [Tooltip("Tracks how many play calls have been made for enemy attack. Decreases for every stop call.")]
        public int enemyAttackPlayCalls;

        // Constructor
        private ActionAudio()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected override void Awake()
        {
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


        // // Start is called before the first frame update
        // void Start()
        // {
        // 
        // }
        // 

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
            // Play the enemy attack
            if(!enemyAttackSource.isPlaying)
            {
                enemyAttackSource.Play();
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
                }
            }
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }

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