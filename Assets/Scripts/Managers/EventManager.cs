using System;
using Misc;
using UnityEngine;

namespace Managers
{
    public static class EventManager
    {
        // Event triggered when a correct collectible is collected.
        public static Action OnCorrectCollectibleCollected;
        public static Action OnGoldCollected;
        
        //Event for Fewer Modes
        public static Action<float> OnFewerModeChanged;
        public static Action<Color, ColorType> OnFewerModeActive;
        public static Action OnFewerModeDisable;
        
        // Event triggered when the game state changes.
        public static Action<GameState> OnGameStateChanged;
        
        // Event triggered when a bonus action is performed.
        public static Action OnBonusActionPerformed;
        
        public static Action<Transform> ChangeCameraTarget;
    }
}