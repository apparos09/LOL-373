using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The dialog for the game being complete.
    public class GameCompleteDialog : MonoBehaviour
    {
        // The world UI.
        public WorldUI worldUI;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the instance.
            if (worldUI == null)
                worldUI = WorldUI.Instance;
        }

        // Completes the game.
        public void CompleteGame()
        {
            worldUI.CompleteGame();
        }
    }
}