using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A lane blaster.
    public class ActionUnitDefenseBlasterLane : ActionUnitDefenseBlaster
    {
        [Header("Lane")]

        // Kills unit once an attack has been performed.
        [Tooltip("Kills this unit once an attack has been performed.")]
        public bool killOnAttackPerformed = true;

        // The blaster shot sfx.
        public AudioClip shotSfx;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Lane blasters shouldn't take damage or be able to attack by default.
            // Once a shot is triggered, the lane blaster destroys itself.
            vulnerable = false;
            attackingEnabled = false;
        }

        // OnTriggerEnter2D is called when the Collider2D other enters this trigger (2D physics only)
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            // Checks if the entity that triggered the laser is an enemy unit.
            ActionUnitEnemy enemyUnit;

            // Tries to get component.
            if(collision.TryGetComponent(out enemyUnit))
            {
                // Enable attacking.
                attackingEnabled = true;
            }
        }

        // Called when an attack ahs been performed.
        public override void OnUnitAttackPerformed()
        {
            base.OnUnitAttackPerformed();

            // Play the shot sound effect.
            PlayShotSfx();

            // If unit should be killed.
            if(killOnAttackPerformed)
            {
                // A shot has been fired, so kill the lane blaster.
                Kill();
            }
        }

        // Called when the unit has died.
        public override void OnUnitDeath()
        {
            // Tile exists.
            if(tile != null)
            {
                // If this is the unit on the tile, mark the tile as unusable.
                if(tile.HasActionUnitUser(this))
                {
                    tile.SetTileOverlayType(ActionTile.actionTileOverlay.unusable);
                }
            }

            base.OnUnitDeath();
        }

        // AUDIO //
        // Plays the shot sound effect.
        public void PlayShotSfx()
        {
            if(CanPlayAudio())
            {
                ActionAudio.Instance.PlaySoundEffectWorld(shotSfx);
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}